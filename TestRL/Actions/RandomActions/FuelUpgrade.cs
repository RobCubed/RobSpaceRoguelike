using System;
using RSS.Tools;

namespace RSS.Actions.RandomActions
{
    public class FuelUpgrade
    {
        public static void Go()
        {
            int upgradeFuelCell = 0;//Ran.dom.Next(5, 100);
            double upgradeFuel = Ran.dom.NextDouble();

            if (upgradeFuel < 0.50)
            {
                upgradeFuelCell = Ran.dom.Next(1, 10);
            }
            else if (upgradeFuel < 0.80)
            {
                upgradeFuelCell = Ran.dom.Next(10, 25);
            }
            else
            {
                upgradeFuelCell = Ran.dom.Next(20, 40);
            }
            Program.InfoLog.AddEntry("You found a way to improve your fuel cells! Increasing fuel capacity by " + upgradeFuelCell, true);
            Program.Player.MaxFuel += upgradeFuelCell;
            Program.Player.Fuel += upgradeFuelCell;
        }
    }
}
