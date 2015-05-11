using System;

namespace RSS.Actions.RandomActions
{
    public class Pirates
    {
        public static void Go()
        {
            Random r = new Random();
            Double pirates = r.NextDouble();

            if (pirates < 0.30)
            {
                Console.WriteLine("You encountered pirates but escaped!");
            }
            else if (pirates < 0.95)
            {
                if (Program.Player.Credits >= 25)
                {
                    Console.WriteLine(
                        "You ran into pirates, who demanded a 25 credit toll. You gave the credits and continued on your way.");
                    Program.Player.Credits -= 25;
                }
                else
                {
                    Console.WriteLine(
                        "You ran into pirates, who demanded a 25 credit toll. You didn't have enough, so they took what you had, and any fuel probes you had left!");
                    Program.Player.Credits = 0;
                    Program.Player.FuelProbes = 0;
                }
            }
            else
            {
                Console.WriteLine("You ran into pirates who took all your fuel, credits, and cargo... and left you stranded. You're pretty much screwed.");
                Program.Player.Credits = 0;
                Program.Player.Fuel = 0;
                Program.Player.FuelProbes = 0;
                Program.Player.CargoHold = 0;
            }
        }
    }
}
