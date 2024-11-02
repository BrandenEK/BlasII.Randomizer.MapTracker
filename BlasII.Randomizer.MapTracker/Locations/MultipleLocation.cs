using BlasII.Randomizer.Items;
using BlasII.Randomizer.MapTracker.Enums;
using BlasII.Randomizer.Shuffle;
using UnityEngine.UI;

namespace BlasII.Randomizer.MapTracker.Locations;

internal class MultipleLocation : ILocation
{
    private readonly string[] _ids;

    // Stored when the map UI is created
    public Image Image { get; set; }

    public MultipleLocation(string[] ids) => _ids = ids;

    public Logic GetReachability(Blas2Inventory inventory)
    {
        int numGreen = 0, numRed = 0;

        foreach (string id in _ids)
        {
            if (IsLocationCollected(id))
                continue;

            ItemLocation location = Main.Randomizer.Data.GetItemLocation(id);
            if (inventory.Evaluate(location.logic))
                numGreen++;
            else
                numRed++;
        }

        if (numGreen == 0 && numRed == 0)
            return Logic.Finished;
        else if (numGreen == 0)
            return Logic.NoneReachable;
        else if (numRed == 0)
            return Logic.AllReachable;
        else
            return Logic.SomeReachable;
    }

    public Logic GetReachabilityAtIndex(int index, Blas2Inventory inventory)
    {
        int validIndex = GetValidIndex(index);

        if (IsLocationCollected(_ids[validIndex]))
            return Logic.Finished;

        ItemLocation location = Main.Randomizer.Data.GetItemLocation(_ids[validIndex]);
        return inventory.Evaluate(location.logic) ? Logic.AllReachable : Logic.NoneReachable;
    }

    public string GetNameAtIndex(int index) => Main.Randomizer.Data.GetItemLocation(_ids[GetValidIndex(index)]).name;

    private int GetValidIndex(int index) => (index %= _ids.Length) < 0 ? index + _ids.Length : index;

    private bool IsLocationCollected(string id) => Main.Randomizer.ItemHandler.CollectedLocations.Contains(id);
}
