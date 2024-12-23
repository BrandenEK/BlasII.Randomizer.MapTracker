using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Files;
using BlasII.Randomizer.MapTracker.Enums;
using BlasII.Randomizer.MapTracker.Locations;
using BlasII.Randomizer.MapTracker.Models;
using System.Collections.Generic;
using System.Linq;
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

    /// <summary>
    /// The current mode that the map is in
    /// </summary>
    internal MapMode MapMode { get; private set; } = MapMode.Closed;
    /// <summary>
    /// Whether locations should be displayed on the map
    /// </summary>
    internal bool DisplayLocations { get; private set; } = true;

    /// <summary>
    /// Initialize all handlers
    /// </summary>
    protected override void OnInitialize()
    {
        if (FileHandler.LoadDataAsSprite("marker.png", out Sprite image, new SpriteImportOptions() { PixelsPerUnit = 10 }))
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

        foreach (var info in locations)
        {
            if (info.Locations == null || info.Locations.Length == 0)
                continue;

            // temp until cherubs are fixed
            info.Locations = info.Locations.Where(x => !x.EndsWith("c0")).ToArray();
            if (info.Locations.Length == 0)
                continue;

            _locationData.Add(new Vector2Int(info.X, info.Y), info.Locations.Length == 1
                ? new SingleLocation(info.Locations[0])
                : new MultipleLocation(info.Locations));
        }
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
        if (MapMode == MapMode.Closed)
            return;

        if (InputHandler.GetKeyDown("ToggleLocations") && MapMode == MapMode.OpenNormal)
        {
            DisplayLocations = !DisplayLocations;
            _ui.Refresh(_inventory.CurrentInventory, true, true);
        }

        _ui.Update(_inventory.CurrentInventory);
    }

    /// <summary>
    /// Called when opening the map
    /// </summary>
    public void OnOpenMap(bool isNormal)
    {
        MapMode = isNormal ? MapMode.OpenNormal : MapMode.OpenTeleport;
        _ui.Refresh(_inventory.CurrentInventory, isNormal, isNormal);
    }

    /// <summary>
    /// Called when closing the map
    /// </summary>
    public void OnCloseMap()
    {
        MapMode = MapMode.Closed;
    }

    /// <summary>
    /// Called when zooming the map
    /// </summary>
    public void OnZoomIn()
    {
        MapMode = MapMode.OpenNormal;
        _ui.Refresh(_inventory.CurrentInventory, true, true);
    }

    /// <summary>
    /// Called when zooming the map
    /// </summary>
    public void OnZoomOut()
    {
        MapMode = MapMode.OpenZoomed;
        _ui.Refresh(_inventory.CurrentInventory, true, false);
    }
}
