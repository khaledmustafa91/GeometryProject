using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGUtilities;
namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class JarvisMarch : Algorithm
    {
       
       
       
      
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            // Sort points on Y axis 
            points.Sort(HelperMethods.sortOnY);
            int i = 0; // index of output Points 
            List<Point> l = new List<Point>(); // output points list 
            l.Add(points[0]); // Add first point to start from it 
            outPoints.Add(points[0]);

            // virtual point to make line at first then we will use it as Line betwwen points to calculate the angle 
            Point Virtual = new Point(l[i].X + 100, points[i].Y);

            // calculate angle betwwen this line and all lines 
            Line Xaxis = new Line(l[i], Virtual);
            
            do
            {
                // this point make minimum angle betwwen the virtual line 
                Point MinPoint = null;
                
                double MinDegree;
                // two cases first we will compute min angle second one compute max angle 
                if (i == 0)
                    MinDegree = 100000;
                else
                    MinDegree = 0;
                for (int j = 0; j < points.Count; j++)
                {
                    if (points[j] != l[i])
                    {
                        Line TestLine = new Line(l[i], points[j]);
                        
                        Point AB;
                        if (i == 0)
                            AB =HelperMethods. getVector(l[i], Virtual);
                        else
                        {
                            if (Virtual == l[i - 1])
                                continue;
                            AB =HelperMethods. getVector(Virtual, l[i - 1]);
                        }
                        Point AC =HelperMethods. getVector(l[i], points[j]);


                        double dotProdct = HelperMethods.DotProduct(AB, AC);

                        double Radian = HelperMethods. magnitude(AB, AC);
                        double CosValue = dotProdct / Radian;
                        double Degree = Math.Acos(CosValue) * (180 / Math.PI);


                        if (Degree < 0)
                            Degree += 360;
                        if (i == 0)
                        {

                            if (Degree < MinDegree || (Degree == MinDegree && HelperMethods. GetDistance(l[i], points[j]) >  HelperMethods. GetDistance(l[i], MinPoint)))
                            {
                                MinDegree = Degree;
                                MinPoint = points[j];
                            }
                        }
                        else
                        {
                            if (Degree > MinDegree || (Degree == MinDegree && MinPoint != null && HelperMethods. GetDistance(l[i], points[j]) > HelperMethods. GetDistance(l[i], MinPoint)))
                            {
                                MinDegree = Degree;
                                MinPoint = points[j];
                            }
                        }
                    }
                }
                Xaxis = new Line(MinPoint, l[i]);
                int isExist = -1;
                if (MinPoint != null)
                {
                    Virtual = MinPoint;
                    if (MinPoint != points[0])
                    {
                        
                        foreach (Point po in l)
                            if (po == MinPoint)
                            {
                                isExist = 1;
                                break;
                            }

                        if (isExist == -1)
                        {
                            l.Add(MinPoint);
                            outPoints.Add(MinPoint);
                            i++;
                        }
                        else
                            break;
                       
                    }
                    else
                        break;
                }
                else
                    break;
            }
            while (l[l.Count - 1] != points[0]);

            for (int j = 0; j < l.Count - 1; j++)
            {
                outLines.Add(new Line(l[j], l[j + 1]));
            }
            if (l.Count >= 2)
                outLines.Add(new Line(l[l.Count - 1], l[0]));
            PassConvexHall(outPoints);
        }
        public static List<Point> PassConvexHall(List<Point> point)
        {
            return point;
        }
        public override string ToString()
        {
            return "Convex Hull - Jarvis March";
        }
    }
}
