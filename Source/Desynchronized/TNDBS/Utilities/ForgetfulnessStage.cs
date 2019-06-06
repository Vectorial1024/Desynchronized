using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Desynchronized.TNDBS.Utilities
{
    public enum ForgetfulnessStage
    {
        UNKNOWN = -1,
        KNOWN = 0,
        FORGOTTEN = 1,
        PERM_LOST = 2
    }
}
