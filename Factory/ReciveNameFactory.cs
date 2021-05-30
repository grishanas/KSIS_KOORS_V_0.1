using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSIS_KOORS_V_0._1.Factory
{
    class ReciveNameFactory:IFactory
    {
        public Mes.Imessage Create(byte[] content)
        {
            var res = new Mes.ReciveName(content);
            return res;
        }
    }
}
