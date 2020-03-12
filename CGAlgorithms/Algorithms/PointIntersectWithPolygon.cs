using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGUtilities;

namespace CGAlgorithms.Algorithms
{
    class PointIntersectWithPolygon : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            Random r = new Random();
            int x = r.Next(1, 200),y = r.Next(1,200);
            Point one = points[0], two = new Point(x ,y);
            Line L = new Line(one, two);
            int intersections = 0;
            for (int i = 0; i < polygons[0].lines.Count; i++)
            {
                if (TwoLinesIntersectionpoint.SegmentWithLine(L, polygons[0].lines[i]) != null)
                {
                    intersections++;
                    outPoints.Add(TwoLinesIntersectionpoint.SegmentWithLine(L, polygons[0].lines[i]));
                }
            }
            if (intersections % 2 == 1)
                outPoints.Add(one);
        }

        public override string ToString()
        {
            return "Point with Polygon";
        }
    }
}
