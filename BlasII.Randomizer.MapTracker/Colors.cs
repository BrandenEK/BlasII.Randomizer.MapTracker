using System.Collections.Generic;
using BlasII.Randomizer.MapTracker.Enums;
using UnityEngine;

namespace BlasII.Randomizer.MapTracker;

internal static class Colors
{
    public static Dictionary<Logic, Color> LogicColors { get; } = new()
    {
        { Logic.Collected, new Color32(63, 63, 63, 255) },
        { Logic.Reachable, new Color32(41, 175, 52, 255) },
        { Logic.UnReachable, new Color32(179, 6, 0, 255) },
        { Logic.OutOfLogic, new Color32(55, 96, 227, 255) },
    };
}
