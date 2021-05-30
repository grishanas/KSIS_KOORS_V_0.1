using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;

namespace KSIS_KOORS_V_0._1
{
    public partial class Form1 : Form
    {
        private Form1 form;
        private User MyUser;
        public ConcurrentQueue<Mes.Imessage> Message;
        public Form1()
        {
            InitializeComponent();
            form = this;
            Message = new ConcurrentQueue<Mes.Imessage>();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var Name = form.textBox1.Text;
            if(Name.Length<1)
            {
                return;
            }
            if (MyUser == null)
            {
                try
                {
                    /*if(form.comboBox1.SelectedIndex<0)
                    {
                        return;
                    }

                    var ChooseIP = form.comboBox1.SelectedItem.ToString();*/
                    MyUser = new User(Name, form);
                }
                catch
                {

                }
                finally
                {

                    
                }
                //form.listBox1.Items.Add(Name);
            }
            

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(form.listBox1.SelectedIndex<0)
            {
                return;
            }
            MyUser.CanStartNewGame = false;
            MyUser.ChooseUser = MyUser.server.users[form.listBox1.SelectedIndex];
            MyUser.SendRequest(form.listBox1.SelectedIndex, 1, "1"+MyUser.Name);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Mes.Imessage mes;
            while (Message.TryPeek(out mes))
            {
                var Mes = (Mes.ReciveName) (mes);
                if (Mes.AddNew == 0)
                {
                    form.listBox1.Items.RemoveAt(Convert.ToInt32(Mes.Content));
                }
                else
                {
                    form.listBox1.Items.Add(Mes.Content);
                }
                Message.TryDequeue(out mes);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MyUser.PlayGround = new PlayGround(MyUser, 0);
            Thread Play = new Thread(MyUser.PlayGround.Show);
            Play.Start();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
           /* string host = Dns.GetHostName();
            IPAddress[] addresses = Dns.GetHostEntry(host).AddressList;
            foreach (var address in addresses)
            {
                if (address.GetAddressBytes().Length == 4)
                {
                    form.comboBox1.Items.Add(address.ToString());   
                }
            }*/
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MyUser != null)
            {
                MyUser.server.SendAllUsers(new Mes.ReciveName("00" + MyUser.ToString()).GetBytes());
                MyUser.server.ServDispose();
            }
            
        }
    }
}
