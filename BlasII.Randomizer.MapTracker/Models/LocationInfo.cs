
namespace BlasII.Randomizer.MapTracker.Models;

/// <summary>
/// Represents a map cell that contains item locations
/// </summary>
public class LocationInfo
{
    /// <summary>
    /// The x position on the map
    /// </summary>
    public int X { get; set; }

    /// <summary>
    /// The y position on the map
    /// </summary>
    public int Y { get; set; }

    /// <summary>
    /// The location ids in this cell
    /// </summary>
    public string[] Locations { get; set; }
}