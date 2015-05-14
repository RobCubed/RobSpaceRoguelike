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
            if (Program.Player.Credits < 25)
            {
                Program.InfoLog.AddEntry("You don't have enough to buy anything!", true);
                return;
            }
            Program.InfoLog.AddEntry("Bought one probe for 25 credits.", true);
            Program.Player.Credits -= 25;
            Program.Player.FuelProbes++;
        }

        public void SellSalvage()
        {
            if (Program.Player.CargoHold < 1)
            {
                Program.InfoLog.AddEntry("You have nothing to sell!", true);
                return;
            }

            Program.InfoLog.AddEntry("Selling " + Program.Player.CargoHold + " salvage for " + Program.Player.CargoHold + " credits!", true);
            Program.Player.Credits += Program.Player.CargoHold;
            Program.Player.Score += Program.Player.CargoHold;
            Program.Player.CargoHold = 0;
        }

        public void Jump()
        {
            throw new NotImplementedException();
        }
    }
}
