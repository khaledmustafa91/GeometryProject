using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class Incremental : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
           


            if (points.Count <= 3)
            {
                for (int i = 0; i < points.Count; i++)
                    outPoints.Add(points[i]);
                return; 
            }


            Point b = points[0];
            Point b2 = points[1];
            Point b3 = points[2];

            Point Mid_Point = new Point((b.X + b2.X) / 2, (b.Y + b2.Y) / 2);

            Mid_Point.X = (Mid_Point.X + b3.X) / 2; Mid_Point.Y = (Mid_Point.Y + b3.Y) / 2;

            Point NB = new Point(Mid_Point.X + 10, Mid_Point.Y);
            Line Horizontel = new Line(Mid_Point, NB);
            //#############################

            List<KeyValuePair<Point, double>> CH = new List<KeyValuePair<Point, double>>();
            CH.Add(new KeyValuePair<Point, double>(b, Calculate_Angle(Horizontel, b)));
            CH.Add(new KeyValuePair<Point, double>(b2, Calculate_Angle(Horizontel, b2))); 
            CH.Add(new KeyValuePair<Point, double>(b3, Calculate_Angle(Horizontel, b3)));
            CH.Sort((x, y) => x.Value.CompareTo(y.Value));
            for (int i = 3; i < points.Count;i ++)
            {
                Point p = points[i];
                double anglep = Calculate_Angle(Horizontel, p); 
                Point pre = Pre(CH, p ,anglep );
                Point next = Next(CH, p, anglep) ;

               // double anglepre = Calculate_Angle(Horizontel, pre);
                //double anglenext = Calculate_Angle(Horizontel, next);
                //double angleb2 = Calculate_Angle(Horizontel, b2); 
                // Out side The polygon 
                if (HelperMethods.CheckTurn(new Line(pre,next) ,p)== Enums.TurnType.Right)
                {
                    //Left Supporting Line 
                    Point newpre = Pre(CH, pre , Calculate_Angle(Horizontel, pre));

                    while (HelperMethods.CheckTurn(new Line(p, pre), newpre) == Enums.TurnType.Left 
                        || HelperMethods.CheckTurn(new Line(p, pre), newpre) ==Enums.TurnType.Colinear )// Or Colinear
                    {
                        CH.Remove( new KeyValuePair<Point, double>(pre, Calculate_Angle(Horizontel, pre)));
                        pre = newpre;
                        newpre = Pre(CH, pre, Calculate_Angle(Horizontel, pre)); 
                    }
                    // Right Supporting Line
                    Point newnext = Next(CH, next, Calculate_Angle(Horizontel, next)); 
                    while(HelperMethods.CheckTurn(new Line (p,next) , newnext) == Enums.TurnType.Right
                        || HelperMethods.CheckTurn(new Line(p, pre), newpre) == Enums.TurnType.Colinear)// Or Colinear) // or Colinear
                    {
                        CH.Remove(new KeyValuePair<Point, double>(next, Calculate_Angle(Horizontel, next)));
                        next = newnext;
                        newnext = Next(CH, next, Calculate_Angle(Horizontel, next)); 
                    }
                    CH.Add(new KeyValuePair<Point, double>(p, Calculate_Angle(Horizontel, p)));
                    CH.Sort((x, y) => x.Value.CompareTo(y.Value));
                } 
            }
            for (int i = 0; i < CH.Count; i ++)
            {
                outPoints.Add(CH[i].Key); 
            }
        }

        

        public override string ToString()
        {
            return "Convex Hull - Incremental";
        }
       
       
        public Point Pre(List<KeyValuePair<Point, double>> CH, Point p , double Angle)
        {
           
            // Angle = Biggesnt one 
            if (Angle == CH[CH.Count - 1].Value)
                return CH[0].Key;

            // Angle = Smallest one >> Handled in Loop .. = CH[i+1]

            // Angle > Biggest one 
            if (Angle > CH[CH.Count - 1].Value)
                return CH[0].Key;
            // Angle < Smallest one
            if (Angle < CH[0].Value)
                return CH[0].Key;

            for (int i = 0; i < CH.Count-1; i++)
                {
                    if (CH[i].Value == Angle)
                    {
                        return CH[i + 1].Key;
                    }
                    if (Angle > CH[i].Value && Angle < CH[i + 1].Value)
                    {
                        return CH[i + 1].Key;
                    }


                }

            return null;
            
        }

     
        public Point Next (List<KeyValuePair<Point, double>> CH, Point p,Double Angle)
        { 
            
            
            // Angle = Biggest one 
            if (Angle == CH[CH.Count - 1].Value)
                return CH[0].Key;
            
            // Angle = Smallest One 
            if (Angle == CH[0].Value)
                return CH[CH.Count - 1].Key;

            // Angle < smallest one
            if (Angle < CH[0].Value)
                return CH[CH.Count - 1].Key;

            // Angle > Biggest one 
            if (Angle > CH[CH.Count - 1].Value)
                return CH[CH.Count - 1].Key;


            for (int i = 0; i < CH.Count-1 ; i++)
            {
                // if (CH[i].Key.X == p.X && CH[i].Key.Y == p.Y )
                //   return CH[i+1].Key;

                if (CH[i].Value == Angle)
                {
                    return CH[i - 1].Key;
                }

                if (Angle > CH[i].Value && Angle < CH[i+1].Value)
                    return CH[i].Key ; 
            }

            return null; 
        }

        //------------------------------------------------------------------------------------
        // Converting Radians to Dregree :
        public double Convert_RadiansTo_Degrees(double radians)
        {
            double degrees = (180 / Math.PI) * radians;
            return (degrees);
        }
        public double  Calculate_Angle (Line Horizontal_Line, Point p)
        {
            double Cross_Product, Dot_Product, Radian_Angel, Degree_Angel;
            Point Vector = new Point((Horizontal_Line.End.X - Horizontal_Line.Start.X), (Horizontal_Line.End.Y - Horizontal_Line.Start.Y));

           Point tmp_Point = new Point((p.X - Horizontal_Line.Start.X), (p.Y - Horizontal_Line.Start.Y));

            Cross_Product = CGUtilities.HelperMethods.CrossProduct(Vector, tmp_Point);
            Dot_Product = CGUtilities.HelperMethods.DotProduct(Vector, tmp_Point);
            Radian_Angel = Math.Atan2(Dot_Product, Cross_Product);
            Degree_Angel = Convert_RadiansTo_Degrees(Radian_Angel);
            if (Degree_Angel < 0)
                Degree_Angel += 360;

            return Degree_Angel; 


        }
       
       

    }
}
