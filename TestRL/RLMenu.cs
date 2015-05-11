using System.Collections.Generic;

namespace RSS
{
    public interface IRlMenu
    {
        List<IRlKeyOption> Options { get; set; }
    }
}
