using BlasII.ModdingAPI;
using BlasII.Randomizer.MapTracker.Locations;
using BlasII.Randomizer.MapTracker.Models;
using System.Collections.Generic;
using UnityEngine;

namespace BlasII.Randomizer.MapTracker;

internal class LocationHandler
{
    private readonly List<LocationInfo> _locationData;

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
            {
                ModLog.Warn($"Cell ({info.X},{info.Y}) has no valid locations");
                continue;
            }

            _locationData.Add(info);
            count += info.Locations.Length;
        }

        ModLog.Info($"Loaded {count} map locations");
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

        var settings = Main.Randomizer.CurrentSettings;
        _currentLocations = new LocationList();

        foreach (var info in _locationData)
        {
            _currentLocations.Add(new Vector2Int(info.X, info.Y), info.Locations.Length == 1
                ? new SingleLocation(info.Locations[0])
                : new MultipleLocation(info.Locations));
        }
    }
}
