using Basalt.LogicParser;
using BlasII.Randomizer.MapTracker.Enums;
using UnityEngine.UI;

namespace BlasII.Randomizer.MapTracker.Locations;

internal interface ILocation
{
    public Image Image { get; set; }

    public Logic GetReachability(GameInventory inventory);
    public Logic GetReachabilityAtIndex(int index, GameInventory inventory);

    public string GetNameAtIndex(int index);
}
