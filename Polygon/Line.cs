using System;
using System.Collections.Generic;

namespace Polygon
{
    public class Line
    {
        public enum ExpressionType
        {
            ETYPE_NULL = 0,
            ETYPE_NORMAL = 1,   //一般式
            ETYPE_KXY = 2,      //点斜式
        }
        private ExpressionType mType = ExpressionType.ETYPE_NULL;

        private float ma = 1;
        private float mb = 0;
        private float mc = 0;

        private Point mPoint = new Point(0, 0);    //某一坐标点
        private float mk = 0;                       //斜率
        private bool mIsVerticalWithX = false;          //垂直与x轴斜率不存在 mk 没有意义

        //一般式
        public Line(float a, float b, float c)
        {
            if (a == 0 && b == 0)
            {
                throw new Exception("can't to create a line");
            }
            ma = a;
            mb = b;
            mc = c;
            mType = ExpressionType.ETYPE_NORMAL;
        }
        //点斜式
        public Line(float k, Point pos, bool isVerticalWithX = false)
        {
            mType = ExpressionType.ETYPE_KXY;
            mPoint = pos.Clone();
            mk = k;
            mIsVerticalWithX = isVerticalWithX;
        }

        public static Line GetLine(float a, float b, float c)
        {
            Line line = null;
            try
            {
                line = new Line(a, b, c);
            }
            catch (System.Exception ex)
            {
                return null;
            }
            return line;
        }

        public static Line GetLine(float k, Point pos, bool isVerticalWithX = false)
        {
            return new Line(k, pos, isVerticalWithX);
        }

        //判断一个点是否在直线上
        public bool CheckPointIsInLine(Point pos)
        {
            if (mType == ExpressionType.ETYPE_NORMAL)
            {
                if (((ma * pos.x) + (mb * pos.y) + mc) == 0)
                {
                    return true;
                }
            }
            else if (mType == ExpressionType.ETYPE_KXY)
            {
                if ((mk * (pos.x - mPoint.x)) == (pos.y - mPoint.y))
                {
                    return true;
                }
            }

            return false;
        }
        //得到 x 确定 求y
        public Point GetPointY(float x)
        {
            Point point = new Point();
            point.x = x;
            if (mType == ExpressionType.ETYPE_NORMAL)
            {
                if (mb == 0)    //无数种情况 或一种也不符合
                    return null;
                point.y = (-mc - ma * x) / mb;
            }
            else if (mType == ExpressionType.ETYPE_KXY)
            {
                if (mIsVerticalWithX)
                    return null;
                point.y = mk * (x - mPoint.x) + mPoint.y;
            }
            return point;
        }
        //得到 y 确定 求 x
        public Point GetPointX(float y)
        {
            Point point = new Point();
            point.y = y;
            if (mType == ExpressionType.ETYPE_NORMAL)
            {
                if (ma == 0f)    //无数种情况 或一种也不符合
                    return null;
                point.x = (-mc - mb * y) / ma;
            }
            else if (mType == ExpressionType.ETYPE_KXY)
            {
                if (mk == 0f)
                    return null;
                point.x = (y - mPoint.y) / mk + mPoint.x;
            }
            return point;
        }

        //判断线段是否与直线相交
        //原理：线段2端(x1,y1)和(x2,y2)，直线方程f(x,y)=0，则f(x1,y1)*f(x2,y2)<=0时线段和直线相交
        public LineSegmentRelations CheckSegmentIsCrossLine(Segment segment)
        {
            if (segment == null)
                return LineSegmentRelations.LSR_Independent;
            float flag0 = 1;
            float flag1 = 1;
            if (mType == ExpressionType.ETYPE_NORMAL)
            {
                flag0 = (ma * segment.start.x) + (mb * segment.start.y) + mc;
                flag1 = (ma * segment.end.x) + (mb * segment.end.y) + mc;
            }
            else if (mType == ExpressionType.ETYPE_KXY)
            {
                flag0 = mIsVerticalWithX ? segment.start.x - mPoint.x : mk * (segment.start.x - mPoint.x) - (segment.start.y - mPoint.y);
                flag1 = mIsVerticalWithX ? segment.end.x - mPoint.x : mk * (segment.end.x - mPoint.x) - (segment.end.y - mPoint.y);
            }
            if (flag0 == 0 && flag1 == 0)
                return LineSegmentRelations.LSR_Cross_Coincide;
            else if (flag0 == 0 || flag1 == 0)
                return LineSegmentRelations.LSR_Cross_In;
            else if (flag0 * flag1 < 0)
                return LineSegmentRelations.LSR_Cross;
            return LineSegmentRelations.LSR_Independent;
        }


        public ExpressionType GetExpressionType()
        {
            return mType;
        }

        public bool GetXYKLineIsVerticalWithX()
        {
            return (mType == ExpressionType.ETYPE_KXY) && mIsVerticalWithX;
        }
    }
}
