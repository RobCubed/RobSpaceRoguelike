using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using TestRL.CelestialObjects;

namespace TestRL.Actions.StationActions
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
