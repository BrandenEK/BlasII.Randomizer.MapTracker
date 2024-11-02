using UnityEngine.UI;

namespace BlasII.Randomizer.MapTracker.Locations;

internal interface ILocation
{
    public Image Image { get; set; }

    public Logic GetReachability(Blas2Inventory inventory);
    public Logic GetReachabilityAtIndex(int index, Blas2Inventory inventory);

    public string GetNameAtIndex(int index);
}
