using HarmonyLib;
using Il2CppTGK.Game.Components.UI;

namespace BlasII.Randomizer.MapTracker
{
    /// <summary>
    /// Process opening and closing the map
    /// </summary>
    [HarmonyPatch(typeof(MapWindowLogic), nameof(MapWindowLogic.SetMapMode))]
    class Map_Show_Patch
    {
        public static void Postfix(MapWindowLogic.MapMode mode) =>
            Main.MapTracker.OnOpenMap(mode == MapWindowLogic.MapMode.Normal);
    }
    [HarmonyPatch(typeof(MapWindowLogic), nameof(MapWindowLogic.OnClose))]
    class Map_Close_Patch
    {
        public static void Postfix() => Main.MapTracker.OnCloseMap();
    }

    /// <summary>
    /// Toggle location display when zooming the map
    /// </summary>
    [HarmonyPatch(typeof(MapWindowLogic), nameof(MapWindowLogic.ZoomIn))]
    class Map_ZoomIn_Patch
    {
        public static void Postfix() => Main.MapTracker.OnZoomIn();
    }
    [HarmonyPatch(typeof(MapWindowLogic), nameof(MapWindowLogic.ZoomOut))]
    class Map_ZoomOut_Patch
    {
        public static void Postfix() => Main.MapTracker.OnZoomOut();
    }

    /// <summary>
    /// Always prevent placing marks
    /// </summary>
    [HarmonyPatch(typeof(MapWindowLogic), nameof(MapWindowLogic.CanPlaceMarker))]
    class Map_Marker_Patch
    {
        public static bool Prefix(ref bool __result) => __result = false;
    }
}
