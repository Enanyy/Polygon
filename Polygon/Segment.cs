using System;
using System.Collections;


namespace Polygon
{
   public class Segment
    {
        public Point start;
        public Point end;
        public Segment(Point varStart, Point varEnd)
        {
            if (varStart == varEnd)
            {
                throw new Exception("can't to create a segment");
            }

            start = varStart.Clone();
            end = varEnd.Clone();
        }

        public static bool CheckSegment(Point varStart, Point varEnd)
        {
            return !(varStart == varEnd);
        }

        public bool CheckPointIsInSegment(Point point)
        {
            return Utility.CrossMultiply(point, start, end) == 0f && (point.x >= Utility.getMin(start.x, end.x) && point.x <= Utility.getMax(start.x, end.x));
        }

    }
}
