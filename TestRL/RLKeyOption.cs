using RLNET;
using RSS.CelestialObjects;

namespace RSS
{
    public interface IRlKeyOption
    {
        RLKey Key { get; set; }
        string OptionText { get; set; }
        void Action(ICelestialObject ce);
    }
}
