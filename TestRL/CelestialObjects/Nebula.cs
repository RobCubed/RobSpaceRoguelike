using System;
using RLNET;
using RSS.Tools;

namespace RSS.CelestialObjects
{
    class Nebula : ICelestialObject
    {
        public IRlMenu Menu { get; set; }
        
        public string Name { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public RLColor Color { get; set; }

        public char Symbol { get; set; }

        public int TotalFuel { get; set; }

        public void Refuel()
        {
            if (Program.Player.FuelProbes < 1)
            {
                Program.InfoLog.AddEntry("You have no fuel probes. Go buy some at a station.", true);
                return;
            }
            if (TotalFuel > 0)
            {
                int fuel = TakeFuel();
                if ((Program.Player.Fuel + fuel) > Program.Player.MaxFuel)
                {
                    Program.InfoLog.AddEntry("You couldn't fit " + fuel + " fuel in your hold. Filled up, but " +
                                    ((Program.Player.Fuel + fuel) - Program.Player.MaxFuel) + " wasted. " + (TotalFuel - fuel) +
                                    " remaining in " + Name, true);
                    Program.Player.Fuel = Program.Player.MaxFuel;
                    Program.Player.FuelProbes = Program.Player.FuelProbes - 1;
                }
                else
                {
                    Program.Player.Fuel += fuel;
                    Program.InfoLog.AddEntry("Added " + fuel + " fuel to your hold. " + (TotalFuel - fuel) +
                        " remaining in " + Name, true);
                    Program.Player.FuelProbes = Program.Player.FuelProbes - 1;
                }
                TotalFuel -= fuel;
            }
            else
            {
                Program.InfoLog.AddEntry("No fuel left in nebula. Probe not used.", true);
            }
        }

        public void Scan()
        {
            Program.InfoLog.AddEntry("Available fuel in Nebula: " + TotalFuel, true);
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
            throw new NotImplementedException();
        }

        public int TakeFuel()
        {
            return Ran.dom.Next(0, TotalFuel + 1);
        }

    }
}
