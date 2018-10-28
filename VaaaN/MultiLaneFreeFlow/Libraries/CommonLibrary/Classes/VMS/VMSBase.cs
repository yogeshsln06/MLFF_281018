using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.Classes.VMS
{
    public abstract class VMSBase
    {
        public abstract Boolean SendTollRateMessage(VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCollection tollRates);
    }
}
