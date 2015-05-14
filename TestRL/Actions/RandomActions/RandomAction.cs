using System;
using RSS.Tools;

namespace RSS.Actions.RandomActions
{
    public static class RandomAction
    {
        public static void Go()
        {
            // 10% chance for an event on EVERYthing
            // 25% chance for object, 25% chance for fuelupgrade, 50% chance for pirates

            if (Ran.dom.NextDouble() > 0.01) return;

            Double randomEvent = Ran.dom.NextDouble();

            if (randomEvent < 0.25)
            {
                FuelUpgrade.Go();
            }
            else if (randomEvent < 0.50)
            {
                RareObject.Go();
            }
            else
            {
                Pirates.Go();
            }
        }
    }
}
