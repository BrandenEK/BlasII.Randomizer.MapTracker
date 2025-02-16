using Basalt.LogicParser;
using BlasII.Randomizer.MapTracker.Enums;
using BlasII.Randomizer.Models;
using UnityEngine.UI;

namespace BlasII.Randomizer.MapTracker.Locations;

internal class SingleLocation : ILocation
{
    private readonly string _id;

    // Stored when the map UI is created
    public Image Image { get; set; }

    public SingleLocation(string id) => _id = id;

    public Logic GetReachability(GameInventory inventory)
    {
        if (IsCollected)
            return Logic.Finished;

        ItemLocation location = Main.Randomizer.ItemLocationStorage[_id];
        return inventory.Evaluate(location.Logic) ? Logic.AllReachable : Logic.NoneReachable;
    }

    public Logic GetReachabilityAtIndex(int index, GameInventory inventory) => GetReachability(inventory);

    public ItemLocation GetLocationAtIndex(int index) => Main.Randomizer.ItemLocationStorage[_id];

    private bool IsCollected => Main.Randomizer.ItemHandler.CollectedLocations.Contains(_id);
}
