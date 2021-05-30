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

namespace KSIS_KOORS_V_0._1
{
    partial class PlayGround : Form
    {
        public PlayGround form;
        User MyUser;
        public UserPlayGround UserPlay;
        public ConcurrentQueue<Mes.Imessage> Message;
        private bool MyStep;
 
        public PlayGround(User user,int i)
        {
            InitializeComponent();
            DoubleBuffered = true;
            form = this;
            UserPlay = new UserPlayGround(form, this.pictureBox1.Width, this.pictureBox1.Height);
            Message = new ConcurrentQueue<Mes.Imessage>();
            MyUser = user;
            if(i>0)
            {
                UserPlay.MyColor = UserPlayGround.color.red;
                form.label1.Text = "Ваш ход";
                MyStep = true;
            }
            else
            {
                UserPlay.MyColor = UserPlayGround.color.blue;
                form.label1.Text = "Ход противника";
                MyStep = false;
            }
            
        }

        private void PlayGround_Activated(object sender, EventArgs e)
        {
            UserPlay.PlayGround_Redrow();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserPlay.PlayGround_Redrow();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (!MyStep)
                return;
            System.Drawing.Point Location = ((MouseEventArgs)e).Location;
            if (UserPlay.AddNewPoint(Location, (int)(UserPlay.MyColor)))
            {
                var win = UserPlay.PlayGround_Redrow();
                var mes = new Mes.RecivePoint((byte)UserPlay.MyColor, ((byte)((Location.X) / (int)(UserPlay.SizeWidht / UserPlayGround.Widht))), ((byte)((Location.Y) / (int)(UserPlay.SizeHeight / UserPlayGround.Height)))).GetBytes();
                MyUser.ChooseUser.WriteToStream(mes);
                switch (win)
                {
                    case 1:
                        {
                            MessageBox.Show("Красный игрок выйграл");
                            MyUser.ChooseUser = null;
                            MyUser.CanStartNewGame = true;
                            MyUser.PlayGround = null;
                            form.Close();
                            break;
                        }
                    case 2:
                        {
                            MessageBox.Show("Синий игрок выйграл");
                            MyUser.ChooseUser = null;
                            MyUser.CanStartNewGame = true;
                            MyUser.PlayGround = null;
                            form.Close();
                            break;
                        }
                }
                var item = UserPlay.FindScore();
                form.label2.Text = "Синий - " + item.Item1.ToString();
                form.label3.Text = "Красный - " + item.Item2.ToString();
                form.label1.Text = "Ход противника";
                MyStep = false;
            }
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Mes.Imessage mes;
            while (Message.TryPeek(out mes))
            {

                UserPlay.AddNewPoint(((Mes.RecivePoint)mes).GetPoint(), ((Mes.RecivePoint)mes).GetColor());
                var win = UserPlay.PlayGround_Redrow();
                if (win != 0)
                {
                    switch (win)
                    {
                        case 1:
                            {
                                MessageBox.Show("Красный игрок выйграл");
                                MyUser.ChooseUser = null;
                                MyUser.CanStartNewGame = true;
                                MyUser.PlayGround = null;
                                form.Close();
                               break;
                            }
                        case 2:
                            {
                                MessageBox.Show("Синий игрок выйграл");
                                MyUser.ChooseUser = null;
                                MyUser.CanStartNewGame = true;
                                MyUser.PlayGround = null;
                                form.Close();
                                break;
                            }
                    }
                }
                var item = UserPlay.FindScore();
                form.label2.Text = "Синий - " + item.Item1.ToString();
                form.label3.Text = "Красный - " + item.Item2.ToString();
                form.label1.Text = "Ваш ход";
                MyStep = true;
                Message.TryDequeue(out mes);
            }
        }

        public void EnemyLeav()
        {
            MessageBox.Show("Противник покинул игру");
            form.BeginInvoke(new Action(() => form.Close()));
            
        }
        private void PlayGround_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MyUser.ChooseUser != null)
            {
                var mes = new Mes.ReciveName("02" + MyUser.Name.ToString()).GetBytes();
                MyUser.ChooseUser.WriteToStream(mes);
                MyUser.ChooseUser = null;
                MyUser.CanStartNewGame = true;
            }
        }

        private void PlayGround_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
