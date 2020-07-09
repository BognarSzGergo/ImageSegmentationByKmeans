using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ImageSegmentationByKmeans
{
    class KMCPoint<T>
    {
        private T m_X;
        private T m_Y;
        private Color m_Color;

        public KMCPoint(T X, T Y, Color Clr) { this.X = X; this.Y = Y; this.Clr = Clr; }

        public T X { get { return m_X; } set { m_X = value; } }

        public T Y { get { return m_Y; } set { m_Y = value; } }

        public Color Clr { get { return m_Color; } set { m_Color = value; } }

    }
}
