
namespace BlasII.Randomizer.MapTracker.Enums;

[System.Flags]
internal enum Logic
{
    Collected = 0x00,

    Reachable = 0x01,
    UnReachable = 0x02,
    OutOfLogic = 0x04,

    MixedReachableUnreachable = Reachable | UnReachable,
    MixedReachableOutOfLogic = Reachable | OutOfLogic,
    MixedUnReachableOutOfLogic = UnReachable | OutOfLogic,

    MixedAll = Reachable | UnReachable | OutOfLogic,
}
