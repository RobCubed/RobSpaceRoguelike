using System;
using RLNET;

namespace RSS.CelestialObjects
{
    public class Station : ICelestialObject
    {
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public RLColor Color { get; set; }
        public IRlMenu Menu { get; set; }
        public char Symbol { get; set; }

        public void Refuel()
        {
            throw new NotImplementedException();
        }

        public void Scan()
        {
            throw new NotImplementedException();
        }

        public void Salvage()
        {
            throw new NotImplementedException();
        }

        public void BuyProbe()
        {
            throw new NotImplementedException();
        }

        public void SellSalvage()
        {
            throw new NotImplementedException();
        }
    }
}
