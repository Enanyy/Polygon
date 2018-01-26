using System;
using System.Collections.Generic;


namespace Polygon
{
   public class Polygon
    {
        public List<Point> pointList = new List<Point>();
        public Polygon(List<Point> varPointList, bool checkLegal = true)
        {
            if (varPointList.Count < 3)
            {
                throw new Exception("can't to create a polygon");
            }

            if (checkLegal && !CheckIsPolygon(varPointList))
                throw new Exception("can't to create a polygon");
            else if (!checkLegal)
            {
                //三个点组成的直线
                //即使三个点在一条直线上，也认为是一个多边形
                //组成的多边形有交叉，是多个多变形的组合， 也认为是一个多边形
            }

            pointList.Clear();
            for (int index = 0; index < varPointList.Count; index++)
            {
                pointList.Add(varPointList[index].Clone());
            }
        }

        //组成矩形的边不能重合， 不能与第三条边相交
        public static bool CheckIsPolygon(List<Point> varPointList)
        {
            if (varPointList.Count < 3)
                return false;
            List<Segment> segmentlist = new List<Segment>();
            //提取线段
            for (int index = 0; index < varPointList.Count; index++)
            {
                Segment tmpSegment = new Segment(varPointList[index].Clone(), varPointList[index == varPointList.Count - 1 ? 0 : index + 1].Clone());
                segmentlist.Add(tmpSegment);
            }
            //判断相交
            for (int x = 0; x < segmentlist.Count; x++)
            {
                for (int y = x + 1; y < segmentlist.Count; y++)
                {
                    SegmentsRelations flag = Utility.CheckSegmentsIntersect(segmentlist[x], segmentlist[y]);
                    //当 当前线段与不相邻的线段相交 肯定不符合
                    //当矩形的2条相邻边重合和这两条边的另外两条领边必有一条边与其重合的边中非领边的边有焦点
                    if ((y - x > 1 && !(x == 0 && y == segmentlist.Count - 1)) && flag > SegmentsRelations.SR_Independent)
                        return false;
                }
            }
            return true;
        }

        public List<Segment> getSegments()
        {
            List<Segment> segmentlist = new List<Segment>();
            //提取线段
            for (int index = 0; index < pointList.Count; index++)
            {
                segmentlist.Add(new Segment(pointList[index].Clone(), pointList[index == pointList.Count - 1 ? 0 : index + 1].Clone()));
            }
            return segmentlist;
        }
    }
}
