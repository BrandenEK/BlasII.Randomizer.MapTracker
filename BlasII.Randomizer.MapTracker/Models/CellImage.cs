using UnityEngine.UI;

namespace BlasII.Randomizer.MapTracker.Models;

/// <summary>
/// Stores all image components for a map cell
/// </summary>
public class CellImage
{
    /// <summary>
    /// The top left inner section
    /// </summary>
    public Image TopLeftInner { get; }

    /// <summary>
    /// The bottom right inner section
    /// </summary>
    public Image BottomRightInner { get; }

    /// <summary>
    /// Initializes a new cell image
    /// </summary>
    public CellImage(Image topLeft, Image bottomRight)
    {
        TopLeftInner = topLeft;
        BottomRightInner = bottomRight;
    }
}
