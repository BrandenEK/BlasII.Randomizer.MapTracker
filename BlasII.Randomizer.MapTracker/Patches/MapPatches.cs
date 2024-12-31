using BlasII.ModdingAPI;
using HarmonyLib;
using Il2CppTGK.Game.Components.UI;

namespace BlasII.Randomizer.MapTracker.Patches;

/// <summary>
/// Process opening and closing the map
/// </summary>
[HarmonyPatch(typeof(MapWindowLogic), nameof(MapWindowLogic.SetMapMode))]
class MapWindowLogic_SetMapMode_Patch
{
    public static void Postfix(MapWindowLogic.MapMode mode)
    {
        ModLog.Warn("Set map mode");
        //Main.MapTracker.OnOpenMap(mode == MapWindowLogic.MapMode.Normal);
    }
}
[HarmonyPatch(typeof(MapWindowLogic), nameof(MapWindowLogic.SetCurrentRender))]
class MapWindowLogic_SetCurrentRender_Patch
{
    public static void Postfix(MapWindowLogic.Renders newIdx)
    {
        ModLog.Warn("set render");
        Main.MapTracker.OnOpenMap(newIdx == MapWindowLogic.Renders.Normal);
    }
}
[HarmonyPatch(typeof(MapWindowLogic), nameof(MapWindowLogic.OnClose))]
class MapWindowLogic_OnClose_Patch
{
    public static void Postfix() => Main.MapTracker.OnCloseMap();
}

/// <summary>
/// Toggle location display when zooming the map
/// </summary>
[HarmonyPatch(typeof(MapWindowLogic), nameof(MapWindowLogic.ZoomIn))]
class MapWindowLogic_ZoomIn_Patch
{
    public static void Postfix() => Main.MapTracker.OnZoomIn();
}
[HarmonyPatch(typeof(MapWindowLogic), nameof(MapWindowLogic.ZoomOut))]
class MapWindowLogic_ZoomOut_Patch
{
    public static void Postfix() => Main.MapTracker.OnZoomOut();
}

/// <summary>
/// Always prevent placing marks
/// </summary>
[HarmonyPatch(typeof(MapWindowLogic), nameof(MapWindowLogic.CanPlaceMarker))]
class MapWindowLogic_CanPlaceMarker_Patch
{
    public static bool Prefix(ref bool __result) => __result = false;
}
