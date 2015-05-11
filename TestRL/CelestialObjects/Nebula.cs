using System;
using RLNET;

namespace TestRL.CelestialObjects
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
                Console.WriteLine("You have no fuel probes. Go buy some!");
                return;
            }
            if (TotalFuel > 0)
            {
                int fuel = TakeFuel();
                if ((Program.Player.Fuel + fuel) > Program.Player.MaxFuel)
                {
                    Console.WriteLine("You couldn't fit " + fuel + " fuel in your hold. Filled up, but " +
                                    ((Program.Player.Fuel + fuel) - Program.Player.MaxFuel) + " wasted. " + (TotalFuel - fuel) +
                                    " remaining in " + Name);
                    Program.Player.Fuel = Program.Player.MaxFuel;
                    Program.Player.FuelProbes = Program.Player.FuelProbes - 1;
                }
                else
                {
                    Program.Player.Fuel += fuel;
                    Console.WriteLine("Added " + fuel + " fuel to your hold. " + (TotalFuel - fuel) +
                                    " remaining in " + Name);
                    Program.Player.FuelProbes = Program.Player.FuelProbes - 1;
                }
                TotalFuel -= fuel;
            }
            else
            {
                Console.WriteLine("No fuel left in nebula. Not using probe!");
            }
        }

        public void Scan()
        {
            Console.WriteLine("Available fuel in Nebula: " + TotalFuel);
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

        public int TakeFuel()
        {
            return new Random().Next(0, TotalFuel + 1);
        }

    }
}
