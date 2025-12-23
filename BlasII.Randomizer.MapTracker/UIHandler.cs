using Basalt.LogicParser;
using BlasII.Framework.UI;
using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Input;
using BlasII.ModdingAPI.Utils;
using BlasII.Randomizer.MapTracker.Enums;
using BlasII.Randomizer.MapTracker.Locations;
using BlasII.Randomizer.MapTracker.Models;
using BlasII.Randomizer.Models;
using Il2CppSystem.IO;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlasII.Randomizer.MapTracker;

internal class UIHandler
{
    private Transform _locationHolder;
    private Transform _cellHolder;
    private UIPixelTextWithShadow _nameText;

    private Sprite[] _markerSprites;
    private readonly Dictionary<ILocation, CellImage> _cellImages = [];

    private Vector2Int _lastCursor;
    private Vector2Int _currentCursor;
    private int _selectedIndex = 0;

    public bool IsShowingCells => _mapCache.Value != null
        && _mapCache.Value.currentMapMode == MapWindowLogic.MapMode.Normal;

    public bool IsShowingLocations => _mapCache.Value != null
        && _mapCache.Value.currentMapMode == MapWindowLogic.MapMode.Normal
        && _mapCache.Value.currentRenderIdx == MapWindowLogic.Renders.Normal;

    /// <summary>
    /// Store the marker sprites
    /// </summary>
    public void LoadImages(Sprite[] images) => _markerSprites = images;

    /// <summary>
    /// Refresh all cell and location UI
    /// </summary>
    public void Refresh(GameInventory inventory, LocationList locations, bool showEverything)
    {
        ModLog.Info("Refreshing map cells and locations");

        // Create location holder and name text
        if (_locationHolder == null || _cellHolder == null)
            CreateLocationHolder(locations);
        if (_nameText == null)
            CreateNameText();

        // Update visibility of all cells
        var allCells = CoreCache.Map.GetAllCells().ToArray();
        var revealedCells = CoreCache.Map.GetRevealedCells().ToArray();
        foreach (var cell in allCells)
        {
            _mapCache.Value.uiRenderNormal.HideCell(cell);
            //_mapCache.Value.uiRenderZoomedOut.HideCell(cell);
        }
        foreach (var cell in IsShowingCells && showEverything ? allCells : revealedCells)
        {
            _mapCache.Value.uiRenderNormal.ShowCell(cell);
            //_mapCache.Value.uiRenderZoomedOut.ShowCell(cell);
        }

        // Update visibility of location holder
        _locationHolder.SetAsLastSibling();
        _locationHolder.gameObject.SetActive(IsShowingLocations && showEverything);

        // Update logic status for all cells
        foreach (var location in locations.Values)
        {
            UpdateCellColor(_cellImages[location], location.GetReachability(inventory));
        }

        // Clear text for selected location name
        _nameText.SetText(string.Empty);
    }

    /// <summary>
    /// Update the position of the location holder and content of the name text
    /// </summary>
    public void Update(GameInventory inventory, LocationList locations, bool showEverything)
    {
        // Process changing cursor positions
        _currentCursor = CalculateCursorPosition();
        if (_currentCursor != _lastCursor)
            _selectedIndex = 0;
        _lastCursor = _currentCursor;

        // Process bumper input
        if (Main.MapTracker.InputHandler.GetButtonDown(ButtonType.UIBumperLeft))
            _selectedIndex--;
        if (Main.MapTracker.InputHandler.GetButtonDown(ButtonType.UIBumperRight))
            _selectedIndex++;

        // Process location holder and name text
        UpdateLocationHolder();
        UpdateNameText(inventory, locations, showEverything);

#if DEBUG
        // Debug info for gathering positions
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            ModLog.Info($"Cursor pos: {_currentCursor}");
        }
#endif
    }

    private void UpdateLocationHolder()
    {
        if (_locationHolder == null || _cellHolder == null)
            return;

        // Scroll position to the cell holder's position
        _locationHolder.position = _cellHolder.position;
    }

    private void UpdateNameText(GameInventory inventory, LocationList locations, bool showEverything)
    {
        if (_nameText == null)
            return;

        // Ensure that the cursor is over a location
        if (!locations.TryGetValue(_currentCursor, out var location) || !showEverything)
        {
            _nameText.SetText(string.Empty);
            return;
        }

        // Set text and color based on hovered location
        ItemLocation itemLocation = location.GetLocationAtIndex(_selectedIndex);
        _nameText.SetText($"- {Main.Randomizer.NameStorage.GetRoomName(itemLocation)} -\n{itemLocation.Name}");
        _nameText.SetColor(Colors.ByReachability(location.GetReachabilityAtIndex(_selectedIndex, inventory)));
    }

    private void UpdateCellColor(CellImage image, Logic logic)
    {
        StyleInfo style = CalculateStyle(logic);
        image.TopLeftInner.color = style.TopLeftColor;
        image.BottomRightInner.color = style.BottomRightColor;
    }

    /// <summary>
    /// Create the UI for all of the squares
    /// </summary>
    private void CreateLocationHolder(LocationList locations)
    {
        var parent = MapHolder;
        if (parent == null)
            return;

        // Clear image list
        _cellImages.Clear();

        // Remove radar ui
        NormalRenderer.GetChild(1).gameObject.SetActive(false);
        ZoomedRenderer.GetChild(1).gameObject.SetActive(false);

        // Create rect for ui holder
        ModLog.Info("Creating new location holder");
        _locationHolder = UIModder.Create(new RectCreationOptions()
        {
            Name = "LocationHolder",
            Parent = parent,
        });
        _cellHolder = NormalRenderer.GetChild(0);

        // Create image for each item location
        foreach (var location in locations)
        {
            var rect = UIModder.Create(new RectCreationOptions()
            {
                Name = $"Location {location.Key}",
                Parent = _locationHolder,
                Size = new Vector2(30, 30),
            });
            rect.localPosition = new Vector3(location.Key.x * 48, location.Key.y * 48);

            var tl = CreateCellImage("tl", 0, rect);
            var br = CreateCellImage("br", Mathf.PI, rect);

            _cellImages.Add(location.Value, new CellImage(tl, br));
        }
    }

    /// <summary>
    /// Create the UI for a single cell
    /// </summary>
    private Image CreateCellImage(string name, float angle, Transform parent)
    {
        var rect = UIModder.Create(new RectCreationOptions()
        {
            Name = name,
            Parent = parent,
            Size = new Vector2(30, 30),
        });
        rect.RotateAroundLocal(Vector3.forward, angle);
        
        return rect.AddImage(new ImageCreationOptions()
        {
            Sprite = _markerSprites[0],
            Color = Color.magenta
        });
    }

    /// <summary>
    /// Create the UI for the selected location name
    /// </summary>
    private void CreateNameText()
    {
        var parent = MarksHolder;
        if (parent == null)
            return;

        // Remove mark ui
        if (parent.GetChild(0) != null)
            Object.Destroy(parent.GetChild(0).gameObject);

        // Create text for location name
        ModLog.Info("Creating new name text");
        _nameText = UIModder.Create(new RectCreationOptions()
        {
            Name = "LocationName",
            Parent = parent,
        }).AddText(new TextCreationOptions()
        {
            FontSize = 32,
        }).AddShadow();
    }

    private Vector2Int CalculateCursorPosition()
    {
        int x = (int)(_locationHolder.localPosition.x / -48 + 0.5f);
        int y = (int)(_locationHolder.localPosition.y / -48 + 0.5f);
        return new Vector2Int(x, y);
    }

    private StyleInfo CalculateStyle(Logic logic)
    {
        if (logic == Logic.Collected)
            return new StyleInfo(Colors.Gray);

        if (logic.HasFlag(Logic.Reachable))
        {
            if (logic.HasFlag(Logic.UnReachable))
                return new StyleInfo(Colors.Green, Colors.Red);

            if (logic.HasFlag(Logic.OutOfLogic))
                return new StyleInfo(Colors.Green, Colors.Blue);

            return new StyleInfo(Colors.Green);
        }

        if (logic.HasFlag(Logic.OutOfLogic))
        {
            if (logic.HasFlag(Logic.UnReachable))
                return new StyleInfo(Colors.Blue, Colors.Red);

            return new StyleInfo(Colors.Blue);
        }

        if (logic.HasFlag(Logic.UnReachable))
        {
            return new StyleInfo(Colors.Red);
        }

        return new StyleInfo(Colors.Invalid);
    }

    private readonly ObjectCache<MapWindowLogic> _mapCache = new(Object.FindObjectOfType<MapWindowLogic>);
    private Transform MapHolder => _mapCache.Value?.mapContent;
    private Transform MarksHolder => _mapCache.Value?.marksList.transform;
    private Transform NormalRenderer => MapHolder.GetChild(0);
    private Transform ZoomedRenderer => MapHolder.GetChild(1);
}
