using System;

namespace RSS.Actions.RandomActions
{
    public class RareObject
    {
        public static void Go()
        {
            Random r = new Random();
            int sellArtifacts = r.Next(5, 100);
            Console.WriteLine("You found a rare alien artifact! You were able to sell it for " + sellArtifacts + " credits.");
            Program.Player.Credits += sellArtifacts;
            Program.Player.Score += sellArtifacts;
        }
    }
}
