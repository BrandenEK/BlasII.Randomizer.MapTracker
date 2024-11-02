using System.Collections.Generic;
using BlasII.Randomizer.MapTracker.Enums;
using UnityEngine;

namespace BlasII.Randomizer.MapTracker;

internal static class Colors
{
    public static Dictionary<Logic, Color> LogicColors { get; } = new()
    {
        { Logic.Finished, new Color32(63, 63, 63, 1) },
        { Logic.NoneReachable, new Color32(207, 16, 16, 1) },
        { Logic.SomeReachable, new Color32(255, 159, 32, 1) },
        { Logic.AllReachable, new Color32(32, 255, 32, 1) },
    };
}
