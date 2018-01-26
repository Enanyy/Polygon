using System;
using System.Collections.Generic;


namespace Polygon
{
    public class Point
    {
        public Point()
        {

        }
        public Point(float tx, float ty)
        {
            x = tx;
            y = ty;
        }
        public float x = 0;
        public float y = 0;

        public Point Clone()
        {
            return new Point(x, y);
        }

        public override string ToString()
        {
            return "Point( " + x.ToString() + " , " + y.ToString() + " )";
        }
    }

}
