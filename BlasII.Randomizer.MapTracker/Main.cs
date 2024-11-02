using BlasII.ModdingAPI.Helpers;
using MelonLoader;

namespace BlasII.Randomizer.MapTracker;

internal class Main : MelonMod
{
    public static MapTracker MapTracker { get; private set; }
    public static Randomizer Randomizer { get; private set; }

    public override void OnLateInitializeMelon()
    {
        MapTracker = new MapTracker();
        Randomizer = ModHelper.GetModByName("Randomizer") as Randomizer;
    }
}
