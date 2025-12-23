using BlasII.ModdingAPI;
using BlasII.Randomizer.MapTracker.Locations;
using BlasII.Randomizer.MapTracker.Models;
using BlasII.Randomizer.Shuffle;
using Il2CppMono.CSharp;
using System.Linq;
using UnityEngine;

namespace BlasII.Randomizer.MapTracker;

internal class LocationHandler
{
    private LocationInfo[] _staticLocationData;

    private LocationList _currentLocations;
    private bool _needsRefresh = true;

    public LocationList CurrentLocations
    {
        get
        {
            if (_needsRefresh)
            {
                _needsRefresh = false;
                CalculateLocations();
            }

            return _currentLocations;
        }
    }

    /// <summary>
    /// Load the location infos from the data file
    /// </summary>
    public void Initialize()
    {
        if (!Main.MapTracker.FileHandler.LoadDataAsJson("locations.json", out LocationInfo[] locations))
        {
            ModLog.Error("Failed to load location data!");
            return;
        }

        int count = 0;
        foreach (var info in locations)
        {
            if (info.Locations == null || info.Locations.Length == 0)
                continue;

            count += info.Locations.Length;
        }

        // maybe remove emptys, but there shouldnt be any

        ModLog.Info($"Loaded {count} map locations");
        _staticLocationData = locations;
    }

    /// <summary>
    /// Force the locations to be recalculated next time they are needed
    /// </summary>
    public void Refresh() => _needsRefresh = true;

    /// <summary>
    /// Recalculate the list of locations
    /// </summary>
    private void CalculateLocations()
    {
        ModLog.Info("Calculating new locations list");



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



        var settings = Main.Randomizer.CurrentSettings;
        _currentInventory = BlasphemousInventory.CreateNewInventory(settings);
        _currentInventory.Add(GetStartingWeaponId(settings));

        foreach (string itemId in Main.Randomizer.ItemHandler.CollectedItems)
        {
            _currentInventory.Add(itemId);
        }
    }

    public void Refreshc()
    {
        // Clear the current list of location data, recalculate it when entering a new game
        // Only ever give out the current list of info based on settings.

        // This way unshuffled locations are never processed anywhere, like they were simply not loaded
    }
}
