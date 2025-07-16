using Basalt.LogicParser;
using BlasII.Randomizer.MapTracker.Enums;
using BlasII.Randomizer.Models;

namespace BlasII.Randomizer.MapTracker.Locations;

internal class SingleLocation : ILocation
{
    private readonly string _id;

    public SingleLocation(string id) => _id = id;

    public Logic GetReachability(GameInventory inventory)
    {
        if (IsCollected)
            return Logic.Collected;

        ItemLocation location = Main.Randomizer.ItemLocationStorage[_id];
        return inventory.Evaluate(location.Logic) ? Logic.Reachable : Logic.UnReachable;
    }

    public Logic GetReachabilityAtIndex(int index, GameInventory inventory) => GetReachability(inventory);

    public ItemLocation GetLocationAtIndex(int index) => Main.Randomizer.ItemLocationStorage[_id];

    private bool IsCollected => Main.Randomizer.ItemHandler.CollectedLocations.Contains(_id);
}
