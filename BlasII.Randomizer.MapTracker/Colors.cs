using System.Collections.Generic;
using BlasII.Randomizer.MapTracker.Enums;
using UnityEngine;

namespace BlasII.Randomizer.MapTracker;

internal static class Colors
{
    //public static Dictionary<Logic, Color> LogicColors { get; } = new()
    //{
    //    { Logic.Collected, new Color32(63, 63, 63, 255) },
    //    { Logic.Reachable, new Color32(41, 175, 52, 255) },
    //    { Logic.UnReachable, new Color32(179, 6, 0, 255) },
    //    { Logic.OutOfLogic, new Color32(55, 96, 227, 255) },
    //};

    public static Color Green { get; } = new Color32(41, 175, 52, 255);

    public static Color Blue { get; } = new Color32(55, 96, 227, 255);

    public static Color Red { get; } = new Color32(179, 6, 0, 255);

    public static Color Gray { get; } = new Color32(63, 63, 63, 255);

    public static Color Invalid { get; } = Color.magenta;

    public static Color ByReachability(Logic logic)
    {
        if (logic == Logic.Collected)
            return Gray;
        
        if (logic == Logic.Reachable)
            return Green;

        if (logic == Logic.OutOfLogic)
            return Blue;

        if (logic == Logic.UnReachable)
            return Red;

        return Invalid;
    }
}
