using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGUtilities;

namespace CGAlgorithms.Algorithms
{
    class Lineandpolygonintersection : Algorithm
    {
        /*public Point SegmentWithLine(Line one , Line Two)
        {
            double slope1, slope2, b1, b2;
            slope1 = (one.Start.Y - one.End.Y) / (one.Start.X - one.End.X);
            slope2 = (Two.Start.Y - Two.End.Y) / (Two.Start.X - Two.End.X);
            b1 = one.Start.Y - (slope1 * one.Start.X);
            b2 = Two.Start.Y - (slope2 * Two.Start.X);
            double x = (b2 - b1) / (slope1 - slope2);
            double y = slope1 * x + b1;
            Point intersectionPoint = new Point(x, y);
            /*if ( (( x >= lines[0].Start.X && y >= lines[0].Start.Y) && (x <= lines[0].End.X && y <= lines[0].End.Y)
                || (x <= lines[0].Start.X && y <= lines[0].Start.Y) && (x >= lines[0].End.X && y >= lines[0].End.Y)
                ) &&( (x >= lines[1].Start.X && y >= lines[1].Start.Y) && (x <= lines[1].End.X && y <= lines[1].End.Y)
                || (x <= lines[1].Start.X && y <= lines[1].Start.Y) && (x >= lines[1].End.X && y >= lines[1].End.Y) ) )
            {
                Point p = new Point(x, y);
                outPoints.Add(p);
            }
            double distance1 = segmentItersection.Distance(one.Start, one.End);// Math.Pow(lines[0].Start.X - lines[0].End.X, 2) + Math.Pow(lines[0].Start.Y - lines[0].End.Y, 2);
            double distance2 = segmentItersection.Distance(Two.Start, Two.End);// Math.Pow(lines[1].Start.X - lines[1].End.X, 2) + Math.Pow(lines[1].Start.Y - lines[1].End.Y, 2);


            if ((segmentItersection.Distance(intersectionPoint, one.Start) <= distance1 && segmentItersection.Distance(intersectionPoint, one.End) <= distance1)
                && segmentItersection.Distance(intersectionPoint, Two.Start) <= distance2 && segmentItersection.Distance(intersectionPoint, Two.End) <= distance2)
            {
                return intersectionPoint;
            }

            return null;
        }*/
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            for (int poly = 0; poly < polygons.Count; poly++)
            {
                for (int i = 0; i <  polygons[poly].lines.Count; i++)
                {
                    if (TwoLinesIntersectionpoint.SegmentWithLine(lines[0], polygons[0].lines[i]) != null)
                    {
                        outPoints.Add(TwoLinesIntersectionpoint.SegmentWithLine(lines[0], polygons[0].lines[i]));
                    }
                }
            }
        }

        public override string ToString()
        {
            return "Segment intersect with Polygon ";
        }
    }
}
