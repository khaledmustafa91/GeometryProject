using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGUtilities;

namespace CGAlgorithms.Algorithms
{
    class PolygonIntersectWithPolygon : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            for (int poly = 0; poly < polygons[0].lines.Count; poly++)
            {
                for(int i = 0;  i < polygons[0].lines.Count; i++)
                {
                    if (TwoLinesIntersectionpoint.SegmentWithLine(polygons[0].lines[poly], polygons[1].lines[i]) != null)
                    {
                        outPoints.Add(TwoLinesIntersectionpoint.SegmentWithLine(polygons[0].lines[poly], polygons[1].lines[i]));
                    }
                }
            }
        }

        public override string ToString()
        {
            return "Polygon intersect with polygon ";
        }
    }
}
