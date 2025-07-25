﻿using Basalt.LogicParser;
using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Assets;
using BlasII.Randomizer.Shuffle;

namespace BlasII.Randomizer.MapTracker;

internal class InventoryHandler
{
    private GameInventory _currentInventory;
    private bool _needsRefresh = true;

    public GameInventory CurrentInventory
    {
        get
        {
            if (_needsRefresh)
            {
                _needsRefresh = false;
                CalculateInventory();
            }

            return _currentInventory;
        }
    }

    /// <summary>
    /// Force the inventory to be recalculated next time it is needed
    /// </summary>
    public void Refresh() => _needsRefresh = true;

    /// <summary>
    /// Recalculate the inventory of the playthrough
    /// </summary>
    private void CalculateInventory()
    {
        ModLog.Info("Calculating new inventory");

        var settings = Main.Randomizer.CurrentSettings;
        _currentInventory = BlasphemousInventory.CreateNewInventory(settings);
        _currentInventory.Add(GetStartingWeaponId(settings));

        foreach (string itemId in Main.Randomizer.ItemHandler.CollectedItems)
        {
            _currentInventory.Add(itemId);
        }
    }

    /// <summary>
    /// Calculates the item id of the chosen starting weapon
    /// </summary>
    private string GetStartingWeaponId(RandomizerSettings settings)
    {
        return ((WEAPON_IDS)settings.RealStartingWeapon).ToString();
    }
}
