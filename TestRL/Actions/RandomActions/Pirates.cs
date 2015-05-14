using System;
using RSS.Tools;

namespace RSS.Actions.RandomActions
{
    public class Pirates
    {
        public static void Go()
        {
            double pirates = Ran.dom.NextDouble();

            if (pirates < 0.40)
            {
                Program.InfoLog.AddEntry("You encountered pirates but escaped!", true);
            }
            else if (pirates < 0.95)
            {
                if (Program.Player.Credits >= 25)
                {
                    Program.InfoLog.AddEntry(
                        "You ran into pirates, who demanded a 25 credit toll. You gave the credits and continued on your way.", true);
                    Program.Player.Credits -= 25;
                }
                else
                {
                    Program.InfoLog.AddEntry(
                        "You ran into pirates, who demanded a 25 credit toll. You didn't have enough, so they took what you had, and a fuel probe instead!", true);
                    Program.Player.Credits = 0;
                    Program.Player.FuelProbes -= 1;
                }
            }
            else
            {
                Program.InfoLog.AddEntry("You ran into bastard pirates who took all your credits, and cargo... Good luck!", true);
                Program.Player.Credits = 0;
                Program.Player.CargoHold = 0;
            }
        }
    }
}
