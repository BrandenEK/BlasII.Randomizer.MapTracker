using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Files;
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
    private readonly LocationHandler _locations = new();
    private readonly UIHandler _ui = new();

    private bool _isMapOpen = false;
    private bool _showEverything = true;

    /// <summary>
    /// Initialize all handlers
    /// </summary>
    protected override void OnInitialize()
    {
        if (FileHandler.LoadDataAsFixedSpritesheet("cells.png", new Vector2(30, 30), out Sprite[] images))
        {
            _ui.LoadImages(images);
        }
        InputHandler.RegisterDefaultKeybindings(new Dictionary<string, KeyCode>()
        {
            { "ToggleLocations", KeyCode.F7 }
        });
        MessageHandler.AllowReceivingBroadcasts = true;
        MessageHandler.AddMessageListener("BlasII.Randomizer", "ITEM", (content) =>
        {
            _inventory.Refresh();
        });

        _locations.Initialize();
    }

    /// <summary>
    /// Refresh inventory and locations when exiting game
    /// </summary>
    protected override void OnExitGame()
    {
        _inventory.Refresh();
        _locations.Refresh();
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
            _ui.Refresh(_inventory.CurrentInventory, _locations.CurrentLocations, _showEverything);
        }

        _ui.Update(_inventory.CurrentInventory, _locations.CurrentLocations, _showEverything);
    }

    /// <summary>
    /// Called when opening/zooming the map
    /// </summary>
    public void OnOpenMap()
    {
        _isMapOpen = true;
        _ui.Refresh(_inventory.CurrentInventory, _locations.CurrentLocations, _showEverything);
    }

    /// <summary>
    /// Called when closing the map
    /// </summary>
    public void OnCloseMap()
    {
        _isMapOpen = false;
    }
}
