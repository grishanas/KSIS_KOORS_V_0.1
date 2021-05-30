using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace KSIS_KOORS_V_0._1
{
    class UserPlayGround
    {
        PlayGround Form;
        public const int Widht = 39;
        public const int Height = 32;
        public int SizeWidht;
        public int SizeHeight;
        private int rad;
        public color MyColor;
        Bitmap bm;

        SolidBrush Blue = new SolidBrush(Color.Blue);
        SolidBrush Red = new SolidBrush(Color.Red);

        SolidBrush RedPolig = new SolidBrush(Color.FromArgb(124, 255, 0, 0));

        Pen BluePen = new Pen(new SolidBrush(Color.Blue));
        Pen RedPen = new Pen(new SolidBrush(Color.Red));
        Pen GrayPen = new Pen(new SolidBrush(Color.Gray));

        public enum color
        {
           white=0,
           red,
           blue
        }
        private struct MyPoint{
            public Point Coord;
            public color brush;
            public color color;
            public bool view;
        }

        MyPoint[] PointArr;

        private List<MyPoint[]> ListOfPoligon;

        public UserPlayGround(PlayGround form,int widht,int height)
        {
            PointArr = new MyPoint[32 * 39];
            SizeWidht = widht;
            SizeHeight = height;
            Form = form;
            ListOfPoligon = new List<MyPoint[]>();
            bm = new Bitmap(SizeWidht, SizeHeight);
            CalculatePlace();
        }

        public (int,int) FindScore()
        {
            int BlueCount = 0, RedCount = 0;
            for (int i = 0; i < 32 * 38; i++)
            {
                if (PointArr[i].brush == color.blue)
                    BlueCount++;
                if (PointArr[i].brush == color.red)
                    RedCount++;

            }
            return (BlueCount,RedCount);
        }

        public int PlayGround_Redrow()
        {

            int i = 0, j = 0;
            Form.pictureBox1.Image = null;
            Form.pictureBox1.Refresh();
            
            using (var G = Graphics.FromImage(bm))
            {
                G.Clear(Color.White);
                for (i = 0; i < Height; i++)
                {
                    G.DrawLine(GrayPen, PointArr[i].Coord, PointArr[Height * (Widht - 1) + i].Coord);

                }
                for (j = 0; j < Widht; j++)
                {
                    G.DrawLine(GrayPen, PointArr[j * Height].Coord, PointArr[(j + 1) * Height - 1].Coord);
                }

                i = 0;
                while (i < Widht)
                {
                    j = 0;
                    while (j < Height)
                    {
                        if (PointArr[i * Height + j].color != color.white)
                        {
                            var br = Red;
                            if (PointArr[i * Height + j].color == UserPlayGround.color.red)
                                br = Red;
                            if (PointArr[i * Height + j].color == UserPlayGround.color.blue)
                                br = Blue;
                            
                            G.FillEllipse(br, PointArr[i * Height + j].Coord.X-rad*2, PointArr[i * Height + j].Coord.Y-rad*2, rad*4, 4*rad);
                        }
                        j++;
                    }
                    i++;
                }

                foreach(var item in ListOfPoligon)
                {
                    Point[] polig = new Point[item.Length];
                    for( i = 0;i<item.Length;i++)
                    {
                        polig[i] = item[i].Coord;

                    }
                    var Pe = RedPen;
                    var PolCol = RedPolig;
                    if (item[0].color == color.blue)
                    {
                        Pe = BluePen;
                        PolCol = Blue;
                    }
                    G.DrawPolygon(RedPen, polig);
                    G.FillPolygon(RedPolig, polig);
                }

                
            }
            Form.pictureBox1.Image = bm;
            Form.pictureBox1.Refresh();
            int BlueCount = 0, RedCount = 0 ;
            bool White = false;
            for(i=0; i<32*38;i++)
            {
                if (PointArr[i].brush == color.blue)
                    BlueCount++;
                if (PointArr[i].brush == color.red)
                    RedCount++;
                
                if(PointArr[i].color==color.white)
                {
                    White = true;
                }
            }
            if (!White)
            {
                return BlueCount > RedCount ?(int) color.blue :(int)color.red;
            }
            return (int)color.white;

        }

        private void CalculatePlace()
        {
            int i = 0, j = 0;
            int Size_x, Size_y;

            Size_x = (int)(SizeWidht / Widht);
            Size_y = (int)(SizeHeight / Height);
            if(Size_x>Size_y)
            {
                rad = (int)(Size_x / 5);
            }
            else
            {
                rad = (int)(Size_y / 5);
            }
            while (i < Widht)
            {
                j = 0;
                while(j<Height)
                {
                    PointArr[i * Height + j].Coord.X = i * Size_x + rad * 2;
                    PointArr[i * Height + j].Coord.Y = j * Size_y + rad * 2;
                    PointArr[i * Height + j].color = color.white;
                    PointArr[i * Height + j].brush = color.white;
                    j++;
                }
                i++;
            }

        }
        public void DrawGrayLine()
        {
            Form.pictureBox1.Image = null;
            Form.pictureBox1.Refresh();
            var bm = new Bitmap(SizeWidht, SizeHeight);
            using (var gr = Graphics.FromImage(bm))
            {
                for (int i = 0; i < Height; i++)
                {
                    gr.DrawLine(GrayPen, PointArr[i].Coord, PointArr[Height * (Widht - 1) + i].Coord);

                }
                for (int j = 0; j < Widht; j++)
                {
                    gr.DrawLine(GrayPen, PointArr[j * Height].Coord, PointArr[(j + 1) * Height - 1].Coord);
                }
           
            }
            Form.pictureBox1.Image = bm;
            Form.pictureBox1.Refresh();
            
        }

        public bool AddNewPoint(Point point,int color)
        {
            int i = 0;
            int j = 0;
            if ((color)color != MyColor)
            {
                i = point.X;
                j = point.Y;
                PointArr[i * Height + j].color = (UserPlayGround.color)color;
                TryBrush(i, j);
                return true;

            }

            i = (int)((point.X) / (int)(SizeWidht / Widht));
            j = (int)((point.Y) / (int)(SizeHeight / Height));
            

            if (!((point.X > PointArr[i * Height+j].Coord.X - rad*2) && (point.X < PointArr[i * Height+j].Coord.X + rad*2)))
            {
                return false;
            }
            if (!((point.Y >= PointArr[i * Height+j].Coord.Y - rad*2) && (point.Y <= PointArr[i * Height+j].Coord.Y + rad*2))){
                return false;
            }
            if (PointArr[i * Height + j].color!= UserPlayGround.color.white)
            {
                return false;
            }
            if (PointArr[i * Height + j].brush!= UserPlayGround.color.white)
            {
                return false;
                
            }
            PointArr[i * Height + j].color = (UserPlayGround.color)color;

            TryBrush(i, j);
            return true;
        }

        private void TryBrush(int i,int j)
        {
            int Direction = 0, d1 = 0, d2=0;
            int Near_x=0, Near_y = 0;
            bool loop1=false;
            while (Direction < 8)
            {
                int index = 0;
                while (index < Widht * Height)
                {
                    PointArr[index].view = false;
                    index++;
                }
                index = 0;
                switch (Direction)
                {
                    case 0:
                        {
                            if (i * Height + j + 1 < 0)
                                break;
                            if (PointArr[i*Height+j].color!=PointArr[i*Height+j+1].color)
                            {
                                Near_x = i;
                                Near_y = j + 1;
                                loop1 = true;
                                d1 = Direction;
                                d2 = Direction + 1;

                            }
                            break;
                        }
                    case 1:
                        {
                            if ((i+1) * Height + j + 1 < 0)
                                break;
                            if (PointArr[i * Height + j].color !=PointArr[(i + 1) * Height + j + 1].color)
                            {
                                Near_x = i + 1;
                                Near_y = j + 1;
                                loop1 = true;
                                d1 = Direction;
                                d2 = Direction + 1;
                            }
                            break;
                        }
                    case 2:
                        {
                            if ((i+1) * Height + j  < 0)
                                break;
                            if (PointArr[i * Height + j].color != PointArr[(i + 1) * Height + j].color)
                            {
                                Near_x = i + 1;
                                Near_y = j;
                                
                                d1 = Direction;
                                d2 = Direction + 1;
                                loop1 = true;
                            }
                            break;
                        }
                    case 3:
                        {
                            if ((i + 1) * Height + j - 1 < 0)
                                break;
                            if (PointArr[i * Height + j].color != PointArr[(i + 1) * Height + j - 1].color)
                            {
                                Near_x = i + 1;
                                Near_y = j - 1;
                         
                                d1 = Direction;
                                d2 = Direction + 1;
                                loop1 = true;
                            }
                            break;
                        }
                    case 4:
                        {
                            if ((i) * Height + j - 1 < 0)
                                break;
                            if (PointArr[i * Height + j].color != PointArr[(i) * Height + j - 1].color)
                            {
                                Near_x = i;
                                Near_y = j - 1;
                                
                                d1 = Direction;
                                d2 = Direction + 1;
                                loop1 = true;
                            }
                            break;
                        }
                    case 5:
                        {
                            if ((i - 1) * Height + j - 1<0)
                                break;
                            if (PointArr[i * Height + j].color != PointArr[(i - 1) * Height + j - 1].color)
                            {
                                Near_x = i - 1;
                                Near_y = j - 1;
                               
                                d1 = Direction;
                                d2 = Direction + 1;
                                loop1 = true;
                            }
                            break;
                        }
                    case 6:
                        {
                            if ((i - 1) * Height + j<0)
                                break;
                            if (PointArr[i * Height + j].color != PointArr[(i - 1) * Height + j].color)
                            {
                                Near_x = i - 1;
                                Near_y = j;
                                
                                d1 = Direction;
                                d2 = Direction + 1;
                                loop1 = true;
                            }
                            break;
                        }
                    case 7:
                        {
                            if ((i - 1) * Height + j + 1 < 0)
                                break;
                            if (PointArr[i * Height + j].color != PointArr[(i - 1) * Height + j + 1].color)
                            {
                                Near_x = i - 1;
                                Near_y = j + 1;
                                
                                d1 = Direction;
                                d2 = 0;
                                loop1 = true;
                            }
                            break;
                        }
                }
                var ListPoint = new List<MyPoint>();
                if (loop1)
                {
                    ListPoint.Add(PointArr[i * Height + j]);
                    PointArr[i * Height + j].view = true;
                }
                bool StartRedrow = false;
                while (loop1)
                {
                    switch (d2)
                    {
                        case 0:
                            {
                                if (((Near_x + 1) * Height + Near_y >= 0))
                                {
                                    if ((PointArr[(Near_x + 1) * Height + Near_y].color == PointArr[i * Height + j].color) && (!PointArr[(Near_x + 1) * Height + Near_y].view) && (PointArr[(Near_x + 1) * Height + Near_y].brush == (color)0))
                                    {
                                        d1 = d2 = 6;
                                        PointArr[(Near_x + 1) * Height + Near_y].view = true;

                                        ListPoint.Add(PointArr[(Near_x + 1) * Height + Near_y]);


                                    }
                                    else
                                    {
                                        if ((Near_x == i) && (Near_y == j))
                                            StartRedrow = true;
                                        else
                                            Near_x++;
                                    }
                                }
                                else
                                {
                                    if ((Near_x == i) && (Near_y == j))
                                        StartRedrow = true;
                                    else
                                        Near_x++;
                                }
                                break;
                            }
                        case 1:
                            {
                                if (((Near_x + 1) * Height + Near_y >= 0))
                                {
                                    if ((PointArr[(Near_x + 1) * Height + Near_y].color == PointArr[i * Height + j].color) && (!PointArr[(Near_x + 1) * Height + Near_y].view) && (PointArr[(Near_x + 1) * Height + Near_y].brush == (color)0))
                                    {
                                        PointArr[(Near_x + 1) * Height + Near_y].view = true;
                                        d2 = 6;
                                        d1 = 6;
                                        ListPoint.Add(PointArr[(Near_x + 1) * Height + Near_y]);
                                    }
                                    else
                                    {
                                        if ((Near_x == i) && (Near_y == j))
                                            StartRedrow = true;
                                        else
                                            Near_x++;
                                    }
                                }
                                else
                                {
                                    if ((Near_x == i) && (Near_y == j))
                                        StartRedrow = true;
                                    else
                                        Near_x++;
                                }
                                break;
                            }
                        case 2:
                            {
                                if (((Near_x) * Height + Near_y - 1 >= 0))
                                {
                                    if ((PointArr[(Near_x) * Height + Near_y - 1].color == PointArr[i * Height + j].color) && (!PointArr[(Near_x) * Height + Near_y - 1].view) && (PointArr[(Near_x) * Height + Near_y - 1].brush == (color)0))
                                    {
                                        PointArr[(Near_x) * Height + Near_y - 1].view = true;
                                        d2 = 0;
                                        d1 = 0;
                                        ListPoint.Add(PointArr[(Near_x) * Height + Near_y - 1]);
                                    }
                                    else
                                    {
                                        if ((Near_x == i) && (Near_y == j))
                                            StartRedrow = true;
                                        else
                                            Near_y--;
                                    }
                                }
                                else
                                {
                                    if ((Near_x == i) && (Near_y == j))
                                        StartRedrow = true;
                                    else
                                        Near_y--;

                                }
                                break;
                                
                            }
                        case 3:
                            {
                                if (((Near_x) * Height + Near_y - 1 >= 0))
                                {
                                    if ((PointArr[(Near_x) * Height + Near_y - 1].color == PointArr[i * Height + j].color) && (!PointArr[(Near_x) * Height + Near_y - 1].view) && (PointArr[(Near_x) * Height + Near_y - 1].brush == (color)0))
                                    {
                                        PointArr[(Near_x) * Height + Near_y - 1].view = true;
                                        d1 = d2 = 0;
                                        ListPoint.Add(PointArr[(Near_x) * Height + Near_y - 1]);
                                    }
                                    else
                                    {
                                        if ((Near_x == i) && (Near_y == j))
                                            StartRedrow = true;
                                        else
                                            Near_y--;
                                    }
                                }
                                else
                                {
                                    if ((Near_x == i) && (Near_y == j))
                                        StartRedrow = true;
                                    else
                                        Near_y--;
                                }
                                break;
                            }
                        case 4:
                            {
                                if (((Near_x - 1) * Height + Near_y >= 0))
                                {
                                    if ((PointArr[(Near_x - 1) * Height + Near_y].color == PointArr[i * Height + j].color) && (!PointArr[(Near_x - 1) * Height + Near_y].view) && (PointArr[(Near_x - 1) * Height + Near_y].brush ==(color)0))
                                    {
                                        PointArr[(Near_x - 1) * Height + Near_y].view = true;
                                        d1 = d2 = 2;

                                        ListPoint.Add(PointArr[(Near_x - 1) * Height + Near_y]);
                                    }
                                    else
                                    {
                                        if ((Near_x == i) && (Near_y == j))
                                            StartRedrow = true;
                                        else
                                            Near_x--;
                                    }
                                }
                                else
                                {
                                    if ((Near_x == i) && (Near_y == j))
                                        StartRedrow = true;
                                    else
                                        Near_x--;
                                }
                                break;
                            }
                        case 5:
                            {
                                if (((Near_x - 1) * Height + Near_y >= 0))
                                {
                                    if ((PointArr[(Near_x - 1) * Height + Near_y].color == PointArr[i * Height + j].color) && (!PointArr[(Near_x - 1) * Height + Near_y].view) && (PointArr[(Near_x - 1) * Height + Near_y].brush == (color)0))
                                    {
                                        PointArr[(Near_x - 1) * Height + Near_y].view = true;
                                        d1 = d2 = 2;
                                        ListPoint.Add(PointArr[(Near_x - 1) * Height + Near_y]);
                                    }
                                    else
                                    {
                                        if ((Near_x == i) && (Near_y == j))
                                            StartRedrow = true;
                                        else
                                            Near_x--;
                                    }
                                }
                                else
                                {
                                    if ((Near_x == i) && (Near_y == j))
                                        StartRedrow = true;
                                    else
                                        Near_x--;
                                }
                                break;
                            }
                        case 6:
                            {
                                if (((Near_x) * Height + Near_y+1 >= 0))
                                {
                                    if ((PointArr[(Near_x) * Height + Near_y + 1].color == PointArr[i * Height + j].color) && (!(PointArr[(Near_x) * Height + Near_y + 1].view)) && (PointArr[(Near_x) * Height + Near_y + 1].brush == (color)0))
                                    {
                                        PointArr[(Near_x) * Height + Near_y + 1].view = true;
                                        d1 = d2 = 4;


                                        ListPoint.Add(PointArr[(Near_x) * Height + Near_y + 1]);
                                    }
                                    else
                                    {
                                        if ((Near_x == i) && (Near_y == j))
                                            StartRedrow = true;
                                        else
                                            Near_y++;
                                    }
                                }
                                else
                                {
                                    if ((Near_x == i) && (Near_y == j))
                                        StartRedrow = true;
                                    else
                                        Near_y++;
                                }
                                break;
                            }
                        case 7:
                            {
                                if (((Near_x) * Height + Near_y + 1 >= 0))
                                {
                                    if ((PointArr[(Near_x) * Height + Near_y + 1].color == PointArr[i * Height + j].color) && (!(PointArr[(Near_x) * Height + Near_y + 1].view)) && (PointArr[(Near_x) * Height + Near_y + 1].brush == (color)0))
                                    {
                                        PointArr[(Near_x) * Height + Near_y + 1].view = true;
                                        d1 = d2 = 4;
                                        ListPoint.Add(PointArr[(Near_x) * Height + Near_y + 1]);
                                    }
                                    else
                                    {
                                        if ((Near_x == i) && (Near_y == j))
                                            StartRedrow = true;
                                        else
                                            Near_y++;
                                    }
                                }
                                else
                                {
                                    if ((Near_x == i) && (Near_y == j))
                                        StartRedrow = true;
                                    else
                                        Near_y++;
                                }
                                break;
                            }

                    }
                    d2++;
                    if (d2 == 8)
                        d2 = 0;
                    if (d2 == d1)
                        loop1 = false;


                }
                if(StartRedrow)
                {
                    if (IsPoligonUniq(ListPoint, (UserPlayGround.color)this.MyColor))
                    {
                        StartRedrow = false;
                        ListOfPoligon.Add(ListPoint.ToArray());
                    }

                }
                Direction++;
            }
        }
        private bool IsPoligonUniq(List<MyPoint> MyPoints,UserPlayGround.color color)
        {
            bool result = false;
            if (MyPoints.Count < 4)
                return false;

            foreach(var item in ListOfPoligon)
            {
                item.ToList();
                var intersecting = item.Intersect(MyPoints);
                if(intersecting.Count() == MyPoints.Count)
                {
                    return false;
                }
                    
            }

            return Brush(color);
        }

        private bool Brush(UserPlayGround.color color)
        {
            int i = 0, j = 0;
            int count = 0, ResultWertical = 0, ResultHorizontal = 0;
            bool brush = false;

            while (i < Widht)
            {
                j = 0;
                count = 0;
                while (j < Height)
                {

                    if (PointArr[i * Height + j].view == true)
                    {
                        count++;
                    }

                    j++;
                }
                j = 0;
                brush = false;
                while ((j < Height) && (count > 0))
                {
                    if ((brush) && (PointArr[i * Height + j].view != true) && (PointArr[i * Height + j].brush != color))
                    {
                        ResultWertical++;
                    }
                    if ((PointArr[i * Height + j].view == true))
                    {
                        brush = !brush;
                        count--;
                    }
                    j++;
                }
                i++;
            }

            j = 0;
            while (j < Height)
            {
                i = 0;
                count = 0;
                while (i < Widht)
                {

                    if (PointArr[i * Height + j].view == true)
                    {
                        count++;
                    }

                    i++;
                }
                i = 0;
                brush = false;
                while ((i < Widht) && (count > 0))
                {
                    if ((brush) && (PointArr[i * Height + j].view != true) && (PointArr[i * Height + j].brush != color))
                    {
                        ResultHorizontal++;
                    }
                    if ((PointArr[i * Height + j].view == true))
                    {
                        brush = !brush;
                        count--;
                    }
                    i++;
                }
                j++;
            }
            if (ResultWertical == 0)
                return false;
            if (ResultHorizontal == 0)
                return false;
            if ((ResultWertical - ResultHorizontal) != 0)
                return false;
            i = 0;
            while (i < Widht)
            {
                j = 0;
                count = 0;
                while (j < Height)
                {

                    if (PointArr[i * Height + j].view == true)
                    {
                        count++;
                    }

                    j++;
                }
                j = 0;
                brush = false;
                while ((j < Height) && (count > 0))
                {
                    if ((brush) && (PointArr[i * Height + j].view != true) && (PointArr[i * Height + j].brush != color))
                    {
                        PointArr[i * Height + j].brush = color;
                    }
                    if ((PointArr[i * Height + j].view == true))
                    {
                        PointArr[i * Height + j].view = false;
                        brush = !brush;
                        count--;
                    }
                    j++;
                }
                i++;
            }
            return true;
        }



    }
}
