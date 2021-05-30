using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSIS_KOORS_V_0._1.Mes
{
    class Answer
    {
        byte Type;
        byte Answ;

        public Answer(byte type,byte code)
        {
            Type = type;
            Answ = code;
        }

        public byte[] GetBytes()
        {
            byte[] res = new byte[2];
            res[0] = Type;
            res[1] = Answ;
            return res;
        }


    }
}
