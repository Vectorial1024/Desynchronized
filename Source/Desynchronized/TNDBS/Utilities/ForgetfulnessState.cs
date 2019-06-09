using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Desynchronized.TNDBS.Utilities
{
    public enum ForgetfulnessState
    {
        UNKNOWN = -1,
        KNOWN = 0,
        LOCALLY_FORGOT = 1,
        PERM_FORGOT = 2
    }
}
