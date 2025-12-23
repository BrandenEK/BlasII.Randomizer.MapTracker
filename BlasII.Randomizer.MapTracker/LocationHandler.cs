
using BlasII.Randomizer.MapTracker.Locations;
using System.Collections.Generic;
using UnityEngine;

namespace BlasII.Randomizer.MapTracker;

internal class LocationHandler
{
    public Dictionary<Vector2Int, ILocation> AllLocations { get; }

    public Dictionary<Vector2Int, ILocation> CurrentLocations { get; }

    public void Initialize()
    {
        // Load the location data from json
    }
}
