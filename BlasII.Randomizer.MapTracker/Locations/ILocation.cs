using Basalt.LogicParser;
using BlasII.Randomizer.MapTracker.Enums;
using BlasII.Randomizer.Models;

namespace BlasII.Randomizer.MapTracker.Locations;

internal interface ILocation
{
    public Logic GetReachability(GameInventory inventory);
    public Logic GetReachabilityAtIndex(int index, GameInventory inventory);

    public ItemLocation GetLocationAtIndex(int index);
}
