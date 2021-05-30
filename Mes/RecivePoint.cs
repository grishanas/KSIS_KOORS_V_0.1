using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace KSIS_KOORS_V_0._1.Mes
{
    class RecivePoint:Imessage
    { 
        public byte Type { get { return 2; }}
        public byte color;
        public byte[] Content ;

        
       public RecivePoint(byte code,byte x,byte y)
       {
            color = code;
            Content = new byte[2];
            Content[0] = x;
            Content[1] = y;
            
       }

       public RecivePoint(byte[] content)
       {
            color = content[1];
            Content = new byte[2];
            Content[0] = content[2];
            Content[1] = content[3];
        }

        public Point GetPoint()
        {
            Point result = new Point();
            result.X = Content[0];
            result.Y = Content[1];
            return result;
        }

        public int GetColor()
        {
            return color;
        }

        public byte[] GetBytes()
        {
            var data = new byte[Content.Length + 2];
            data[0] = Convert.ToByte(Type);
            data[1] = color;
            Content.CopyTo(data, 2);
            return data;
        }

    }
}
