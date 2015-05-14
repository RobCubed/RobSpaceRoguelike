using System;
using RSS.Tools;

namespace RSS.Actions.RandomActions
{
    public class RareObject
    {
        public static void Go()
        {
            int sellArtifacts = Ran.dom.Next(5, 100);
            Program.InfoLog.AddEntry("You found a rare alien artifact! You were able to sell it for " + sellArtifacts + " credits.", true);
            Program.Player.Credits += sellArtifacts;
            Program.Player.Score += sellArtifacts;
        }
    }
}
