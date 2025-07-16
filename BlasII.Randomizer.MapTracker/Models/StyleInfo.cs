using UnityEngine;

namespace BlasII.Randomizer.MapTracker.Models;

/// <summary>
/// Models the style for a cell UI
/// </summary>
public class StyleInfo
{
    /// <summary>
    /// The color for the top left section
    /// </summary>
    public Color TopLeftColor { get; }

    /// <summary>
    /// The color for the bottom right section
    /// </summary>
    public Color BottomRightColor { get; }

    /// <summary>
    /// Initializes a new style info
    /// </summary>
    public StyleInfo(Color color1, Color color2)
    {
        TopLeftColor = color1;
        BottomRightColor = color2;
    }

    /// <summary>
    /// Initializes a new style info
    /// </summary>
    public StyleInfo(Color color)
    {
        TopLeftColor = color;
        BottomRightColor = color;
    }
}
