using BlasII.Randomizer.MapTracker.Locations;
using System.Collections.Generic;
using UnityEngine;

namespace BlasII.Randomizer.MapTracker.Models;

/// <summary>
/// Stores all the map cells based on their position
/// </summary>
internal class LocationList : Dictionary<Vector2Int, ILocation>
{
}
