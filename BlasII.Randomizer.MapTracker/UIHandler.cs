using BlasII.ModdingAPI.Input;
using BlasII.ModdingAPI.Utils;
using Il2CppSystem.IO;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.UI;
using UnityEngine;
using UnityEngine.UI;

namespace BlasII.Randomizer.MapTracker;

internal class UIHandler
{
    private Transform _locationHolder;
    private Transform _cellHolder;
    private UIPixelTextWithShadow _nameText;

    private Sprite _locationImage;

    private Vector2Int _lastCursor;
    private Vector2Int _currentCursor;
    private int _selectedIndex = 0;

    /// <summary>
    /// Store the marker image
    /// </summary>
    public void LoadImage(Sprite image) => _locationImage = image;

    /// <summary>
    /// Refresh all cell and location UI
    /// </summary>
    public void Refresh(Blas2Inventory inventory, bool showCells, bool showLocations)
    {
        // Create location holder and name text
        if (_locationHolder == null || _cellHolder == null)
            CreateLocationHolder();
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
        foreach (var cell in showCells && Main.MapTracker.DisplayLocations ? allCells : revealedCells)
        {
            _mapCache.Value.uiRenderNormal.ShowCell(cell);
            //_mapCache.Value.uiRenderZoomedOut.ShowCell(cell);
        }

        // Update visibility of location holder
        _locationHolder.SetAsLastSibling();
        _locationHolder.gameObject.SetActive(showLocations && Main.MapTracker.DisplayLocations);

        // Update logic status for all cells
        foreach (var location in Main.MapTracker.AllLocations.Values)
        {
            location.Image.color = Colors.LogicColors[location.GetReachability(inventory)];
        }

        // Clear text for selected location name
        _nameText.SetText(string.Empty);
    }

    /// <summary>
    /// Update the position of the location holder and content of the name text
    /// </summary>
    public void Update(Blas2Inventory inventory)
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
        UpdateNameText(inventory);
    }

    private void UpdateLocationHolder()
    {
        if (_locationHolder == null || _cellHolder == null)
            return;

        // Scroll position to the cell holder's position
        _locationHolder.position = _cellHolder.position;
    }

    private void UpdateNameText(Blas2Inventory inventory)
    {
        if (_nameText == null)
            return;

        // Ensure that the cursor is over a location
        if (!Main.MapTracker.AllLocations.TryGetValue(_currentCursor, out var location) || !Main.MapTracker.DisplayLocations)
        {
            _nameText.SetText(string.Empty);
            return;
        }

        // Set text and color based on hovered location
        _nameText.SetText(location.GetNameAtIndex(_selectedIndex));
        _nameText.SetColor(Colors.LogicColors[location.GetReachabilityAtIndex(_selectedIndex, inventory)]);
    }

    /// <summary>
    /// Create the UI for all of the squares
    /// </summary>
    private void CreateLocationHolder()
    {
        var parent = MapHolder;
        if (parent == null) return;

        // Remove radar ui
        Object.Destroy(NormalRenderer.GetChild(1).gameObject);
        Object.Destroy(ZoomedRenderer.GetChild(1).gameObject);

        // Create rect for ui holder
        Main.MapTracker.Log("Creating new location holder");
        _locationHolder = UIModder.CreateRect("LocationHolder", parent);
        _cellHolder = NormalRenderer.GetChild(0);

        // Create image for each item location
        foreach (var location in Main.MapTracker.AllLocations)
        {
            var rect = UIModder.CreateRect($"Location {location.Key}", _locationHolder);
            rect.localPosition = new Vector3(location.Key.x * 48, location.Key.y * 48);
            rect.sizeDelta = new Vector2(30, 30);

            var image = rect.gameObject.AddComponent<Image>();
            image.sprite = _locationImage;
            image.color = Color.red;

            location.Value.Image = image;
        }
    }

    /// <summary>
    /// Create the UI for the selected location name
    /// </summary>
    private void CreateNameText()
    {
        var parent = MarksHolder;
        if (parent == null) return;

        // Remove mark ui
        if (parent.GetChild(0) != null)
            Object.Destroy(parent.GetChild(0).gameObject);

        // Create text for location name
        Main.MapTracker.Log("Creating new name text");
        _nameText = UIModder.CreateRect("LocationName", parent).AddText().SetFontSize(50).AddShadow();
    }

    private Vector2Int CalculateCursorPosition()
    {
        int x = (int)(_locationHolder.localPosition.x / -48 + 0.5f);
        int y = (int)(_locationHolder.localPosition.y / -48 + 0.5f);
        return new Vector2Int(x, y);
    }

    private readonly ObjectCache<MapWindowLogic> _mapCache = new(() => Object.FindObjectOfType<MapWindowLogic>());
    private Transform MapHolder => _mapCache.Value?.mapContent;
    private Transform MarksHolder => _mapCache.Value?.marksList.transform;
    private Transform NormalRenderer => MapHolder.GetChild(0);
    private Transform ZoomedRenderer => MapHolder.GetChild(1);
}
