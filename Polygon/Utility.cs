using System;
using System.Collections.Generic;

namespace Polygon
{
    //2条线段之间的关系
    public enum SegmentsRelations
    {
        SR_Independent = -1,            //不相交
        SR_Cross = 0,                   //相交
        SR_Cross_In = 1,                //相交 一个点重合
        SR_Cross_Coincide = 3,          //相交 共线
    }

    //直线和线段之间的关系
    public enum LineSegmentRelations
    {
        LSR_Independent = -1,           //不相交
        LSR_Cross = 0,                  //相交
        LSR_Cross_In = 1,               //相交 线段 一个点重合 
        LSR_Cross_Coincide = 3,         //相交 重合
    }


    public static class Utility
    {
        public static float getAbs(float src)
        {
            return src < 0 ? (-src) : src;
        }

        public static float getMax(float arg0, float arg1)
        {
            return arg0 > arg1 ? arg0 : arg1;
        }
        public static float getMin(float arg0, float arg1)
        {
            return arg0 > arg1 ? arg1 : arg0;
        }
        /*---------------------*/
        // 判断直线是否与线段相交
        /*---------------------*/
        public static LineSegmentRelations CheckSegmentIsCrossLine(Line line, Segment segment)
        {
            if (line == null || segment == null)
                return LineSegmentRelations.LSR_Independent;

            return line.CheckSegmentIsCrossLine(segment);
        }

        public static bool CheckPointIsInSegment(Segment se, Point point)
        {
            if (se == null || point == null)
                return false;
            return se.CheckPointIsInSegment(point);
        }

        /*---------------------------------------------------------------------------*/
        //  
        //                      判断两个边垂直与x轴或者y轴的矩形是否相交
        //
        //  参数为 矩形的对角线 端点
        //  原理 两个矩形的中心点在x轴，y轴上的距离 都小于等于 两个矩形的宽或者高的一半之和
        //  即使对角线垂直x轴或者y轴 不影响逻辑判断
        /*---------------------------------------------------------------------------*/
        public static bool CheckRectanglesIntersect(Point Rect0_p0, Point Rect0_p1, Point Rect1_p0, Point Rect1_p1)
        {
            /*float Halfheight = (getAbs(Rect0_p0.y - Rect0_p1.y) + getAbs(Rect1_p0.y - Rect1_p1.y)) / 2.0f;
            float Halfwide = (getAbs(Rect0_p0.x - Rect0_p1.x) + getAbs(Rect1_p0.x - Rect1_p1.x)) / 2.0f;
            float centerdistanceX = getAbs((Rect0_p0.x + Rect0_p1.x)  - (Rect1_p0.x + Rect1_p1.x))/ 2f;
            float centerdistanceY = getAbs((Rect0_p0.y + Rect0_p1.y)  - (Rect1_p0.y + Rect1_p1.y)) / 2f;
            if (centerdistanceX <= Halfwide && centerdistanceY <= Halfheight)
                return true;*/
            //简化为
            if (getAbs((Rect0_p0.x + Rect0_p1.x) - (Rect1_p0.x + Rect1_p1.x)) <= getAbs(Rect0_p0.x - Rect0_p1.x) + getAbs(Rect1_p0.x - Rect1_p1.x) && getAbs((Rect0_p0.y + Rect0_p1.y) - (Rect1_p0.y + Rect1_p1.y)) <= getAbs(Rect0_p0.y - Rect0_p1.y) + getAbs(Rect1_p0.y - Rect1_p1.y))
                return true;
            return false;
        }

        /*********************************************************************************************
         *              
         *                                  判断 2条线段是否相交
         *              
         *  原理 1快速排除 （排除2条线段在同一直线上时 不相交）
         *       2 两条线段相交 必然相互 相交， 两条线段的两个端点各自在另一线段的两侧或有一端点在另一线段上
         *
        **********************************************************************************************/
        public static SegmentsRelations CheckSegmentsIntersect(Segment s0, Segment s1)
        {
            if (!CheckRectanglesIntersect(s0.start, s0.end, s1.start, s1.end))
                return SegmentsRelations.SR_Independent;
            //跨立原理
            /***********************************************************************************************************************************
             * 向量的叉乘 几何意义垂直与两向量的向量(实际运算得出的是伪向量，绝对值为垂直与两向量的模，也等于以两向量为2边组成的平行四边形的面积）
             * 设向量P（A1,A2) 向量Q (B1,B2)
             * P X Q = -（Q X P)
             * P X Q = A1*B2 - A2*B1   //（A1*B2 - A2*B1 的绝对值为平行四边形的面积）
             * 右手原理 右手半握，大拇指垂直向上，四指右向量P握向Q，大拇指的方向就是叉积的方向
             * 当 A1*B2 - A2*B1  大于 0 时 P 在 Q的顺时针方向 （右手原理 ）
             * 
             *   !!!!!!!!!!!!!以上大一数学有学!!!!!!!!!!!!!!
             * 
             * 当P X Q > 0   //P在Q的顺时针方向 拇指向下
             * 当P X Q < 0   //P在Q的逆时针方向 拇指向上
             * 当P X Q = 0   //p Q 共线 方向不确定是否相同 拇指范围为垂直与共线的平面的圆
             * 
             *  p 跨立 Q  (A1 - B1) X (B2 - B1)   （A2 - B1) X (B2 - B1)  得出的值正负相反时 跨立
             *  所以 (A1 - B1) X (B2 - B1) *（A2 - B1) X (B2 - B1) < 0 当 = 0 时 因为已通过快速排除 2线段必然重合
             *  简化为 (A1 - B1) X (B2 - B1) * (B2 - B1) X （A2 - B1) >= 0
             ***********************************************************************************************************************************/
            float flag0 = CrossMultiply(s0.start, s1.end, s1.start);
            float flag1 = CrossMultiply(s1.end, s0.end, s1.start);
            if (flag0 == 0f && flag1 == 0f)
                return SegmentsRelations.SR_Cross_Coincide;
            else if (flag0 == 0 || flag1 == 0)
            {
                if (s0.CheckPointIsInSegment(s1.start) || s0.CheckPointIsInSegment(s1.end) || s1.CheckPointIsInSegment(s0.start) || s1.CheckPointIsInSegment(s0.end))
                    return SegmentsRelations.SR_Cross_In;
                else
                    return SegmentsRelations.SR_Independent;
            }
            else if (flag0 * flag1 > 0)
            {
                float flag2 = CrossMultiply(s1.start, s0.end, s0.start);
                float flag3 = CrossMultiply(s0.end, s1.end, s0.start);
                if (flag2 * flag3 > 0)
                    return SegmentsRelations.SR_Cross;
                else
                    return SegmentsRelations.SR_Independent;
            }
            return SegmentsRelations.SR_Independent;
        }
        //向量 AC BC的叉乘
        public static float CrossMultiply(Point pa, Point pb, Point pc)
        {
            return (pa.x - pc.x) * (pb.y - pc.y) - (pa.y - pc.y) * (pb.x - pc.x);
        }

        /******************************************************************
         *
         *                      判断一个点是否在多边形内  
         * 
         *    以该点做平行与x轴直线 当且仅当 直线在点两侧与矩形的交点个数为奇数时
         *                          该点在多边形内
         *                          
         *     当一侧的交点为奇数时，另一侧肯定也为奇数 所以只用证明一侧即可
         * 
        ******************************************************************/
        public static bool CheckPointInPolygon(Point point, Polygon polygon)
        {
            if (point == null || polygon == null)
                return false;
            // 排除 点在多边形边山的情况
            List<Segment> segmentlist = polygon.getSegments();
            for (int index = 0; index < segmentlist.Count; index++)
            {
                if (segmentlist[index].CheckPointIsInSegment(point))
                    return true;
            }
            // 直线 y = point.y
            Line lineX = new Line(0f, 1f, -point.y);
            //检测右边
            int crossCount = 0;
            for (int index = 0; index < segmentlist.Count; index++)
            {
                LineSegmentRelations type = CheckSegmentIsCrossLine(lineX, segmentlist[index]);
                if (type == LineSegmentRelations.LSR_Independent || type == LineSegmentRelations.LSR_Cross_Coincide)
                {
                    //不相交 和 重合的边不算在交点计数里面
                    continue;
                }
                else if (type == LineSegmentRelations.LSR_Cross)
                {
                    //判断交点是否在右侧 判断线段两端点与 point 的向量是否在 以线段所在的向量的 夹角

                    Point hPoint = null;
                    Point lPoint = null;
                    if (segmentlist[index].start.y > segmentlist[index].end.y)
                    {
                        hPoint = segmentlist[index].start;
                        lPoint = segmentlist[index].end;
                    }
                    else
                    {
                        hPoint = segmentlist[index].end;
                        lPoint = segmentlist[index].start;
                    }
                    if (CrossMultiply(point, lPoint, hPoint) > 0f)
                    {
                        crossCount++;
                    }
                }
                else if (type == LineSegmentRelations.LSR_Cross_In)
                {

                    //在 point y 轴上面的不计数  
                    if ((segmentlist[index].start.y == point.y && segmentlist[index].end.y < point.y) || (segmentlist[index].end.y == point.y && segmentlist[index].start.y < point.y))
                    {
                        //判断右侧
                        Point hPoint = null;
                        Point lPoint = null;
                        if (segmentlist[index].start.y > segmentlist[index].end.y)
                        {
                            hPoint = segmentlist[index].start;
                            lPoint = segmentlist[index].end;
                        }
                        else
                        {
                            hPoint = segmentlist[index].end;
                            lPoint = segmentlist[index].start;
                        }
                        if (CrossMultiply(point, lPoint, hPoint) > 0f)
                        {
                            crossCount++;
                        }
                    }
                }

            }
            return crossCount % 2 == 1;
        }
    }
}


