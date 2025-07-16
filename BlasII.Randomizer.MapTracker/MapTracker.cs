using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Files;
using BlasII.Randomizer.MapTracker.Locations;
using BlasII.Randomizer.MapTracker.Models;
using System.Collections.Generic;
using UnityEngine;

namespace BlasII.Randomizer.MapTracker;

/// <summary>
/// An in-game map tracker for the Blasphemous 2 Randomizer
/// </summary>
public class MapTracker : BlasIIMod
{
    internal MapTracker() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    private readonly InventoryHandler _inventory = new();
    private readonly UIHandler _ui = new();

    private readonly Dictionary<Vector2Int, ILocation> _locationData = new();
    internal Dictionary<Vector2Int, ILocation> AllLocations => _locationData;

    private bool _isMapOpen = false;
    private bool _showEverything = true;

    /// <summary>
    /// Initialize all handlers
    /// </summary>
    protected override void OnInitialize()
    {
        if (FileHandler.LoadDataAsSprite("cell.png", out Sprite image))
        {
            _ui.LoadImage(image);
        }
        InputHandler.RegisterDefaultKeybindings(new Dictionary<string, KeyCode>()
        {
            { "ToggleLocations", KeyCode.F7 }
        });
        MessageHandler.AllowReceivingBroadcasts = true;
        MessageHandler.AddMessageListener("BlasII.Randomizer", "LOCATION", (content) =>
        {
            _inventory.Refresh();
        });

        LoadLocationData();
    }

    /// <summary>
    /// Load the Single/Multiple location data from the json list
    /// </summary>
    private void LoadLocationData()
    {
        if (!FileHandler.LoadDataAsJson("locations.json", out LocationInfo[] locations))
        {
            ModLog.Error("Failed to load location data!");
            return;
        }

        int count = 0;
        foreach (var info in locations)
        {
            if (info.Locations == null || info.Locations.Length == 0)
                continue;

            _locationData.Add(new Vector2Int(info.X, info.Y), info.Locations.Length == 1
                ? new SingleLocation(info.Locations[0])
                : new MultipleLocation(info.Locations));

            count += info.Locations.Length;
        }

        ModLog.Info($"Loaded {count} map locations");
    }

    /// <summary>
    /// Refresh inventory when exiting game
    /// </summary>
    protected override void OnExitGame()
    {
        _inventory.Refresh();
    }

    /// <summary>
    /// Process input and update UI when on map screen
    /// </summary>
    protected override void OnLateUpdate()
    {
        if (!_isMapOpen)
            return;

        if (InputHandler.GetKeyDown("ToggleLocations") && _ui.IsShowingCells && _ui.IsShowingLocations)
        {
            _showEverything = !_showEverything;
            _ui.Refresh(_inventory.CurrentInventory, _showEverything);
        }

        _ui.Update(_inventory.CurrentInventory, _showEverything);
    }

    /// <summary>
    /// Called when opening/zooming the map
    /// </summary>
    public void OnOpenMap()
    {
        _isMapOpen = true;
        _ui.Refresh(_inventory.CurrentInventory, _showEverything);
    }

    /// <summary>
    /// Called when closing the map
    /// </summary>
    public void OnCloseMap()
    {
        _isMapOpen = false;
    }
}
