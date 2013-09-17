using System;
using System.Drawing;
using System.Windows.Forms;
namespace taxi
{
    class roboter_steuerung
    {
        public PictureBox this_roboter;
        int aktuelle_richtung = 2;
        int _meter;
        int durchlauf=0;
        Timer t;
        
        public roboter_steuerung(PictureBox Roboter)
        {
            this_roboter = Roboter;
        }

        enum Richtung
        {
            oben = 1,
            rechts = 2,
            unten = 3,
            links = 4
        }

        public void rechts_drehen()
        {
            this.this_roboter.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            aktuelle_richtung++;
            if (aktuelle_richtung == 5) aktuelle_richtung = 1;
            

        }

        public void links_drehen()
        {
            this_roboter.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            aktuelle_richtung--;
            if (aktuelle_richtung == 0) aktuelle_richtung = 4;
        }

        public void vorwärts(int meter)
        {
            this._meter = meter;
            t = new Timer();
            t.Interval= 120;
            t.Tick += new EventHandler(t_Tick);
            t.Enabled = true;
        }

        private void _vorwärts()
        {
            switch (aktuelle_richtung)
            {
                case (int)Richtung.oben:
                    {
                        this_roboter.Location = new Point(this_roboter.Location.X, this_roboter.Location.Y - 5);
                    } break;

                case (int)Richtung.rechts:
                    {
                        this_roboter.Location = new Point(this_roboter.Location.X + 5, this_roboter.Location.Y);
                    } break;

                case (int)Richtung.unten:
                    {
                        this_roboter.Location = new Point(this_roboter.Location.X, this_roboter.Location.Y + 5);
                    } break;

                case (int)Richtung.links:
                    {
                        this_roboter.Location = new Point(this_roboter.Location.X - 5, this_roboter.Location.Y);
                    } break;
            }
        }

        void  t_Tick(object sender, EventArgs e)
        {
            if (durchlauf == _meter)
            {
                t.Enabled = false;
                return;
            }

            _vorwärts();
            durchlauf++;
        }

        public void rechts(int meter)
        {
            this_roboter.Location = new Point(this_roboter.Location.X + meter, this_roboter.Location.Y);
        }

        public void links(int meter)
        {
            this_roboter.Location = new Point(this_roboter.Location.X - meter, this_roboter.Location.Y);
        }

        public void unten(int meter)
        {
            this_roboter.Location = new Point(this_roboter.Location.X, this_roboter.Location.Y + meter);
        }

        public void oben(int meter)
        {
            this_roboter.Location = new Point(this_roboter.Location.X, this_roboter.Location.Y - meter);
        }

        /*void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (
            switch (aktuelle_richtung)
            {
                case (int)Richtung.oben:
                    {
                        this_roboter.Location = new Point (this_roboter.Location.X,this_roboter.Location.Y - 5);
                    }break;

                case (int)Richtung.rechts:
                    {
                        this_roboter.Location = new Point(this_roboter.Location.X+5, this_roboter.Location.Y);
                    }break;

                case (int)Richtung.unten:
                    {
                        this_roboter.Location = new Point(this_roboter.Location.X, this_roboter.Location.Y + 5);
                    }break;

                case (int)Richtung.links:
                    {
                        this_roboter.Location = new Point(this_roboter.Location.X -5 , this_roboter.Location.Y);
                    }break;
        }
        }*/

       public void stop()
        {
            t.Enabled = false;
        }


    }
}
