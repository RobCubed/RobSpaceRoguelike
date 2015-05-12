using System;
using RLNET;
using RSS.CelestialObjects;

namespace RSS.Actions.StationActions
{
    public class BuyProbe : IRlKeyOption
    {
        public RLKey Key { get; set; }
        public string OptionText { get; set; }

        public BuyProbe()
        {
            Key = RLKey.B;
            OptionText = "Buy fuel probes for 25 credits";
        }

        public void Action(ICelestialObject ce)
        {
            if (Program.Player.Credits < 25)
            {
                Console.WriteLine("You don't have enough to buy anything!");
                return;
            }
            Program.Player.Credits -= 25;
            Program.Player.FuelProbes++;
        }
    }
}
