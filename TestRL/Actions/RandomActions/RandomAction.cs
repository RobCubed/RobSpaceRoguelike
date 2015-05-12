using System;

namespace RSS.Actions.RandomActions
{
    public static class RandomAction
    {
        public static void Go()
        {
            Random r = new Random();
            // 10% chance for an event on EVERYthing
            // 25% chance for object, 25% chance for fuelupgrade, 50% chance for pirates

            if (r.NextDouble() > 0.03) return;
            
            Double randomEvent = r.NextDouble();

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
