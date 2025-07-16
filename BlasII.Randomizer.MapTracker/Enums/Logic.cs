
namespace BlasII.Randomizer.MapTracker.Enums;

internal enum LogicOld
{
    Finished,
    NoneReachable,
    SomeReachable,
    AllReachable,
}

[System.Flags]
internal enum Logic
{
    Reachable = 0x01,
    UnReachable = 0x02,
    OutOfLogic = 0x04,
    Collected = 0x08,

    MixedReachableUnreachable = Reachable | UnReachable,
    MixedReachableOutOfLogic = Reachable | OutOfLogic,
    MixedUnReachableOutOfLogic = UnReachable | OutOfLogic,

    MixedAll = Reachable | UnReachable | OutOfLogic,
}
