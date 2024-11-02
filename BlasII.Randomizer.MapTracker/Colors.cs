using System.Collections.Generic;
using UnityEngine;

namespace BlasII.Randomizer.MapTracker
{
    internal static class Colors
    {
        public static readonly Dictionary<Logic, Color> LogicColors = new()
        {
            { Logic.Finished, RGBColor(63, 63, 63) },
            { Logic.NoneReachable, RGBColor(207, 16, 16) },
            { Logic.SomeReachable, RGBColor(255, 159, 32) },
            { Logic.AllReachable, RGBColor(32, 255, 32) },
        };

        private static Color RGBColor(int r, int g, int b)
        {
            return new Color(r / 255f, g / 255f, b / 255f);
        }
    }
}
