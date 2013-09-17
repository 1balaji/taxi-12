using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace taxi
{
    class punkt_zu_punkt_steuerung
    {
        public int Index { get; private set; }

        Point[] _punkte;

        public punkt_zu_punkt_steuerung(Point[] Punkte)
        {
            this._punkte = Punkte;
        }

        public Point next_point()
        {
            return new Point();
        }

    }
}
