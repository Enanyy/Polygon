using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polygon
{
    class Class1
    {
        public static void Main(String[] args)
        {
            System.Console.WriteLine("efefwef");
            List<Point> tmpPointList = new List<Point>();
            tmpPointList.Add(new Point(0, 0));
            tmpPointList.Add(new Point(1, 0));
            tmpPointList.Add(new Point(1, 1));
            tmpPointList.Add(new Point(0f, 1));

            Point tmpPoint = new Point(0.5f, 0.5f);
          Console.WriteLine(  Utility.CheckPointInPolygon(tmpPoint, new Polygon(tmpPointList)));
        }
    }
}
