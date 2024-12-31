using HarmonyLib;
using Il2CppTGK.Game.Components.UI;

namespace BlasII.Randomizer.MapTracker.Patches;

/// <summary>
/// Process opening and closing the map
/// </summary>
[HarmonyPatch(typeof(MapWindowLogic), nameof(MapWindowLogic.SetCurrentRender))]
class MapWindowLogic_SetCurrentRender_Patch
{
    public static void Postfix() => Main.MapTracker.OnOpenMap();
}
[HarmonyPatch(typeof(MapWindowLogic), nameof(MapWindowLogic.OnClose))]
class MapWindowLogic_OnClose_Patch
{
    public static void Postfix() => Main.MapTracker.OnCloseMap();
}

/// <summary>
/// Always prevent placing marks
/// </summary>
[HarmonyPatch(typeof(MapWindowLogic), nameof(MapWindowLogic.CanPlaceMarker))]
class MapWindowLogic_CanPlaceMarker_Patch
{
    public static bool Prefix(ref bool __result) => __result = false;
}
