using System;
using RLNET;

namespace RSS.CelestialObjects
{
    public class Wreckage : ICelestialObject
    {
        public string Name { get; set; }
        public int SalvageAvailable { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public RLColor Color { get; set; }
        public IRlMenu Menu { get; set; }
        public char Symbol { get; set; }

        public void Refuel() { throw new NotImplementedException(); }

        public void Scan()
        {
            Console.WriteLine("Total salvage available at " + Name + " : " + SalvageAvailable);
        }

        public void Salvage()
        {
            if (Program.Player.CargoHold >= Program.Player.CargoHoldMax)
            {
                Console.WriteLine("Your hold is full! Go sell your cargo.");
            }

            int maxToCarry = Program.Player.CargoHoldMax - Program.Player.CargoHold;

            if (maxToCarry > SalvageAvailable)
            {
                Console.WriteLine("You've cleared out all " + SalvageAvailable + " units of salvage and have " +
                                  (maxToCarry - SalvageAvailable) + " space left.");
                Program.Player.CargoHold += SalvageAvailable;
                SalvageAvailable = 0;
            }
            else
            {
                Console.WriteLine("You can only carry " + maxToCarry + ", leaving " + (SalvageAvailable - maxToCarry) + " in the wreckage for later.");
                Program.Player.CargoHold += maxToCarry;
                SalvageAvailable -= maxToCarry;
            }
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
