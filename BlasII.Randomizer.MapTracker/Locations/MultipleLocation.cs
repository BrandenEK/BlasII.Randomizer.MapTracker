using Basalt.LogicParser;
using BlasII.Randomizer.MapTracker.Enums;
using BlasII.Randomizer.Models;

namespace BlasII.Randomizer.MapTracker.Locations;

internal class MultipleLocation : ILocation
{
    private readonly string[] _ids;

    public MultipleLocation(string[] ids) => _ids = ids;

    public Logic GetReachability(GameInventory inventory)
    {
        Logic logic = Logic.Collected;

        foreach (string id in _ids)
        {
            if (IsLocationCollected(id))
                continue;

            ItemLocation location = Main.Randomizer.ItemLocationStorage[id];
            if (inventory.Evaluate(location.Logic))
                logic |= Logic.Reachable;
            else
                logic |= Logic.UnReachable;
        }

        return logic;
    }

    public Logic GetReachabilityAtIndex(int index, GameInventory inventory)
    {
        int validIndex = GetValidIndex(index);

        if (IsLocationCollected(_ids[validIndex]))
            return Logic.Collected;

        ItemLocation location = Main.Randomizer.ItemLocationStorage[_ids[validIndex]];
        return inventory.Evaluate(location.Logic) ? Logic.Reachable : Logic.UnReachable;
    }

    public ItemLocation GetLocationAtIndex(int index) => Main.Randomizer.ItemLocationStorage[_ids[GetValidIndex(index)]];

    private int GetValidIndex(int index) => (index %= _ids.Length) < 0 ? index + _ids.Length : index;

    private bool IsLocationCollected(string id) => Main.Randomizer.ItemHandler.CollectedLocations.Contains(id);
}
