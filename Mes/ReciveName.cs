using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSIS_KOORS_V_0._1.Mes
{
    class ReciveName:Imessage
    {
        public byte Type { get { return 0; } set { } }
        public string Content;
       

        // 0- удалить пользователя из списка
        // 1 - добавить пользователя в список
        public byte AddNew = 0;
        public ReciveName(string content)
        {
            Type = Convert.ToByte(content[0]);
            AddNew = Convert.ToByte(content[1]-'0');

            var con = new char[content.Length - 2];
            content.CopyTo(2, con, 0,content.Length-2);
            if (AddNew == 0)
            {
                Content = new string(con);

            }
            else
                Content = Convert.ToString(con);

        }
        public ReciveName(byte[] content)
        {
            this.Type = content[0];
            AddNew = content[1];
            Content = Encoding.UTF8.GetString(content, 2, content.Length - 2);
        }


        public byte[] GetBytes()
        {
            var paylod = Encoding.UTF8.GetBytes(Content);
            var data = new byte[paylod.Length + 2];
            data[0] = Type;
            data[1] = AddNew;
            paylod.CopyTo(data, 2);
            return data;
        }

        public override string ToString()
        {
            return Content;
        }
    }
}
