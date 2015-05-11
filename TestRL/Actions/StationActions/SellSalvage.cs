using System;
using RLNET;
using RSS.CelestialObjects;

namespace RSS.Actions.StationActions
{
    public class SellSalvage : IRlKeyOption
    {
        public RLKey Key { get; set; }
        public string OptionText { get; set; }

        public SellSalvage()
        {
            Key = RLKey.S;
            OptionText = "Sell salvage";
        }

        public void Action(ICelestialObject ce)
        {
            if (Program.Player.CargoHold < 1)
            {
                Console.WriteLine("You have nothing to sell!");
                return;
            }

            Console.WriteLine("Selling " + Program.Player.CargoHold + " salvage for " + Program.Player.CargoHold + " credits!");
            Program.Player.Credits += Program.Player.CargoHold;
            Program.Player.Score += Program.Player.CargoHold;
            Program.Player.CargoHold = 0;
        }
    }
}
