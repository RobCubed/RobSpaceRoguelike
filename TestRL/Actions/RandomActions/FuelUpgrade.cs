using System;

namespace RSS.Actions.RandomActions
{
    public class FuelUpgrade
    {
        public static void Go()
        {
            Random r = new Random();
            int upgradeFuelCell = r.Next(5, 100);
            Console.WriteLine("You found a way to improve your fuel cells! Increasing fuel capacity by " + upgradeFuelCell);
            Program.Player.MaxFuel += upgradeFuelCell;
            Program.Player.Fuel += upgradeFuelCell;
        }
    }
}
