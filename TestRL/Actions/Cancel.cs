using RLNET;
using RSS.CelestialObjects;

namespace RSS.Actions
{
    class Cancel : IRlKeyOption
    {
        public RLKey Key { get; set; }

        public string OptionText { get; set; }

        public Cancel()
        {
            Key = RLKey.C;
            OptionText = "Cancel";
        }

        public void Action(ICelestialObject ce)
        {
            Program.PopupMenu.FallBack();
        }
   }
}
