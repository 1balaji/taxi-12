using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

//TODO: Interpolation um damit das Punkte Array zu erstellen ---------------->Point[] Strecke
//TODO: Taxi bzw. Picturebox soll das Abfahren (später selbst gezeichnet)---->foreach (Point next_point in Strecke)
//TODO: Mechanismen wie Haltestellen implementieren-------------------------->timer.enable = false
//TODO: Happy sein-----------------------------------------------------------> :)
//TODO: Winkel miteinbeziehen------------------------------------------------> Noch kein plan wie, der Lehrer kann es

//Dieser Code is so scheiße, Kommentare sind notwendig
namespace taxi
{
    public partial class Form1 : Form
    {
        Timer next_point_driver = new Timer();
        int next_point_index =0;
        bool next_point_interrupted = false;
        bool draw_mousepoints = true;
        List<Point> Mouse_points = new List<Point>(); //Punkte für das Zeichnen der Strecke (Polygon)
        List<Point> interpolation_points = new List<Point>();//Errechnete Punkte zwischen den einzelnen Punkten des Polygons
        List<int> X = new List<int>();//alle X Punkte ...annahme das x und y unteschiuedlich berechnet werden
        List<int> Y = new List<int>();//alle Y Punkte
        int X_mouse;//X-achse des Cursor
        int Y_mouse;//Y-Achse des Cursor
        public Form1()
        {
            InitializeComponent();
            MouseUp += new MouseEventHandler(Form1_MouseUp);
            MouseMove += new MouseEventHandler(Form1_MouseMove);
            MouseClick += new MouseEventHandler(Form1_MouseClick);
        }

        void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                draw_mousepoints = false;
                MouseMove -= new MouseEventHandler(Form1_MouseMove);
                MouseUp -= new MouseEventHandler(Form1_MouseUp);
                //Mouse_points.RemoveAt(Mouse_points.Count);
                
                Refresh();
                //g.DrawPolygon(new Pen(new SolidBrush(Color.Red)), Mouse_points.ToArray()); //Polygon aus vorhanden Punkten
                //g.DrawClosedCurve(new Pen(new SolidBrush(Color.Red)), Mouse_points.ToArray());
            }
            else
            {
                Mouse_points.Add(new Point(X_mouse - 4, Y_mouse - 4));
            }
        }
        /*static public float interpolate(float x, float x0, float x1, float y0, float y1) //Andere Interpolationsformel
        {
            if ((x1 - x0) == 0)
            {
                return (y0 + y1) / 2;
            }
            return y0 + (x - x0) * (y1 - y0) / (x1 - x0);
        }*/

        static float interpolate(float x0, float y0, float x1, float y1, float x) //INterpolations formel
        {
            return y0 * (x - x1) / (x0 - x1) + y1 * (x - x0) / (x1 - x0);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillEllipse(new SolidBrush(Color.Red), new Rectangle(X_mouse-4, Y_mouse-4, 5, 5));//Vorgezeichneter Punkt für die Orientierung
            if (draw_mousepoints == true)
            {
                for (int i = 0; i < Mouse_points.Count; i++)
                {
                    e.Graphics.FillEllipse(new SolidBrush(Color.Red), new Rectangle(Mouse_points[i], new Size(5, 5))); //Damit die Punkte da bleiben...eigendlich unnötig, da sie nicht überschrieben werden..is aber ordenlicher
                }
            }
            else
            {
                //e.Graphics.DrawClosedCurve(new Pen(new SolidBrush(Color.Red)), Mouse_points.ToArray());
                e.Graphics.DrawPolygon(new Pen(new SolidBrush(Color.Red)), Mouse_points.ToArray()); //Polygon aus vorhanden Punkten
            }
            
        }

        void Form1_MouseMove(object sender, MouseEventArgs e) //Maus Koordianten aktuell halten
        {
            X_mouse = e.X;
            Y_mouse = e.Y;
            Refresh();
        }

        void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) return;
             //Wenn geklickt Punkt hinzufügen
            
        }

        roboter_steuerung taxi1;

        public Image rotateImageUsi(Image bitmap, float angle)    // Quelle: http://code-bude.net/2011/07/12/bilder-rotieren-mit-csharp-bitmap-rotateflip-vs-gdi-graphics/
        {
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.TranslateTransform((float)bitmap.Width / 2, (float)bitmap.Height / 2);
                graphics.RotateTransform(angle);
                graphics.TranslateTransform(-(float)bitmap.Width / 2, -(float)bitmap.Height / 2);
                graphics.DrawImage(bitmap, new Point(0, 0));
            }
            return bitmap;
        }

        private void pic_station1_Click(object sender, EventArgs e)//test auslöser..soll später die haltestellen funktio nsein...(hier_halten, veränderung des Bildes etc.)
        {
             /*taxi1 = new roboter_steuerung(taxi); //Unnötige Klasse mit der meist verbrauchten Zeit ._.'
             taxi1.rechts(50);
             taxi1.oben(50);
            List<Point> point = new List<Point>();

            Point a = new Point(50,50); // Test Punkt zur Berechnung
            Point b = new Point (100,100);
            for(int i=0;i < b.X ;i += 2) //Test um irgendwie nen Ergebnis zu bekommen
            {
             point.Add(new Point(Convert.ToInt32(interpolate(a.X,a.Y ,b.X ,b.Y ,i )),Convert.ToInt32(interpolate(b.X,b.Y,a.X ,a.Y ,i ))));
                X.Add(Convert.ToInt32(interpolate(a.X,a.Y,b.X,b.Y,i)));
                Y.Add(Convert.ToInt32(interpolate(b.X, b.Y, a.X, a.Y, i)));
            }
             foreach (Point poin in point.ToArray())
             {
                 MessageBox.Show(poin.ToString());
             }*/

            taxi1 = new roboter_steuerung(taxi);
            next_point_driver.Interval = 50;
            next_point_driver.Tick += new EventHandler(next_point_driver_Tick);
            next_point_driver.Enabled = true;
        }

        void next_point_driver_Tick(object sender, EventArgs e)
        {
            do
            {

                if (next_point_index > Mouse_points.Count-1) next_point_index = 0;

                taxi1.this_roboter.Location = Mouse_points[next_point_index];
                next_point_index++;

            } while (next_point_interrupted != false);
        }

        public void next_point ()
        {

            do
            {
                next_point_index++;
                taxi1.this_roboter.Location = Mouse_points[next_point_index];
            } while (next_point_interrupted != false);
        }

            
            /*foreach (Point poin in point.ToArray())
            {
                taxi1.this_roboter.Location = new Point(poin.X, poin.Y);
                MessageBox.Show(poin.ToString());
            }*/
            
        }





        }

    

