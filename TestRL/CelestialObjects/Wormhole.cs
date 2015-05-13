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
        public readonly int JumpTo;

        public Wormhole(int jumpTo)
        {
            JumpTo = jumpTo;

            Sector jumpToSector = null;
            Program._sectorMap.TryGetValue(JumpTo, out jumpToSector);
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
            Program._sectorMap.TryGetValue(JumpTo, out jumpToSector);
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
            Program.JumpToSector(JumpTo);
        }
    }
}
