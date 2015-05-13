using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;

namespace RSS.CelestialObjects
{
    class Wormhole : ICelestialObject
    {
        private readonly int _jumpTo;

        public Wormhole(int jumpTo)
        {
            _jumpTo = jumpTo;

            Sector jumpToSector = null;
            Program._sectorMap.TryGetValue(_jumpTo, out jumpToSector);
            if (jumpToSector != null)
            {
                Name = "Wormhole to " + jumpToSector.Name;
            }
            else
            {
                Name = "Wormhole to unexplored sector";
            }
        }

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
            Sector jumpToSector = null;
            Program._sectorMap.TryGetValue(_jumpTo, out jumpToSector);
            if (jumpToSector != null)
            {
                Debug.WriteLine("This jumps to " + jumpToSector.Name);
            }
            else
            {
                Debug.WriteLine("This jumps to another sector.");
            }
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

        public void Jump()
        {
            Program.JumpToSector(_jumpTo);
        }
    }
}
