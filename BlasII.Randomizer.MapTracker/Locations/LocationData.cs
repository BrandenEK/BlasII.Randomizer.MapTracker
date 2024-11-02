
namespace BlasII.Randomizer.MapTracker.Locations;

internal class LocationData
{
    public readonly int x;
    public readonly int y;
    public readonly string[] locations;

    public LocationData(int x, int y, string[] locations)
    {
        this.x = x;
        this.y = y;
        this.locations = locations;
    }
}
