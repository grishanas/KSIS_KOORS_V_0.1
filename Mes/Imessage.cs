using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSIS_KOORS_V_0._1.Mes
{
    public interface Imessage
    {
        public byte Type {get;}

        public byte[] GetBytes();

    }
}
