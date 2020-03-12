using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGUtilities;

namespace CGAlgorithms.Algorithms
{
    class BasicOrientationTest : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            Enum e = HelperMethods.CheckTurn(lines[0], points[0]);
            if (e.Equals(Enums.TurnType.Right))
                outPoints.Add(points[0]);
            else
                outLines.Add(lines[0]);

        }

        public override string ToString()
        {
            return "Orientation test";
        }
    }
}
