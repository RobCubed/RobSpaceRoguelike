using RLNET;

namespace RSS.CelestialObjects
{
    public interface ICelestialObject
    {
        string Name { get; set; }
        int X { get; set; }
        int Y { get; set; }
        RLColor Color { get; set; }
        IRlMenu Menu { get; set; }
        char Symbol { get; set; }
        void Refuel();
        void Scan();
        void Salvage();
        void BuyProbe();
        void SellSalvage();
    }
}
