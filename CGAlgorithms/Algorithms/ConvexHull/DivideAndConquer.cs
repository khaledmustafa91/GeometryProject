using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class DivideAndConquer : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {

            //points.Sort(HelperMethods.sortOnX);
            points = points.OrderBy(x => x.X).ThenBy(x => x.Y).ToList();
            List<Point> Resultpoints = new List<Point>();
            Resultpoints = Divide(points);
            for (int i = 0; i < Resultpoints.Count; i++)
                outPoints.Add(Resultpoints[i]);
            
        }
        public List<Point> Divide(List<Point> points)
        {
            // if point count smaller than 6 we calculate to them CH and return CH points
            if (points.Count < 6) 
            {
                List<Point> output = new List<Point>();
                output = JarvisMarchAlgo(points);
                return output;
            }
            else // Here we divide point and pass it again till number of points equal 6  
            {
                int MI = (points.Count / 2); // get mid point of points 
                if (points.Count % 2 != 0)
                    MI++;
                List<Point> Left = new List<Point>(), Right = new List<Point>();
                for (int i = 0; i < MI; i++)
                    Left.Add(points[i]); // Add all left point in the list to Left list
                for (int i = MI; i < points.Count; i++)
                    Right.Add(points[i]); // Add all right point in the list to Right list
                // pass and return new points 
                List<Point> LCH = Divide(Left); 
                List<Point> RCH = Divide(Right);
                return Merge(LCH, RCH);
            }
        }

         
        public int orientation(Point a, Point b,
                Point c)
        {
            int res =(int) (b.Y - a.Y) * (int)(c.X- b.X) -
                      (int) (c.Y- b.Y) * (int) (b.X- a.X);
            if (res == 0)
                return 0;
            if (res > 0)
                return 1;
            return -1;
        }
        public List<Point> Merge(List<Point> LCH, List<Point> RCH)
        {
            /* Here we get the most right point in Left Concex Hall 
            and most left ponit in Right Convex Hall*/
            #region Get Most Left Point and Most Right Point  
            double x = -100000, y = -10000;
            int LCHindex = 0;
            for (int i = 0; i < LCH.Count; i++)
            {
                if (LCH[i].X > x)
                {
                    x = LCH[i].X;
                    y = LCH[i].Y;
                    LCHindex = i;
                }
                else if (LCH[i].X == x && LCH[i].Y > y)
                {
                    x = LCH[i].X;
                    y = LCH[i].Y;
                    LCHindex = i;
                }
            }

            Point MLP = LCH[LCHindex];
            x = 100000;
            y = -10000;
            int RCHindex = 0;
            for (int i = 0; i < RCH.Count; i++)
            {
                if (RCH[i].X < x)
                {
                    x = RCH[i].X;
                    y = RCH[i].Y;
                    RCHindex = i;
                }
                else if (RCH[i].X == x && RCH[i].Y < y)
                {
                    x = RCH[i].X;
                    y = RCH[i].Y;
                    RCHindex = i;
                }
            }
            Point MRP = null;
            MRP = RCH[RCHindex];
            #endregion

            #region UP Supporting line
            int ULCHCounter = LCHindex;
            int URCHCounter = RCHindex;
            Point ULP = MLP, URP = MRP; // ULP as inda and URP as indb
            bool done = false;
            int oldLCH = -1, oldRCH = -1;
            int iter = 0; 
            while (!done)
            {
                done = true;
                Line FromRtoL = new Line(URP, ULP);
                oldLCH = LCHindex;
                while(orientation(RCH[RCHindex], LCH[LCHindex], LCH[(LCHindex + 1) % LCH.Count]) > 0)
                {
                    LCHindex = (LCHindex + 1) % LCH.Count;
                    FromRtoL = new Line(URP, LCH[LCHindex]);
                    done = false;
                }
                if(done == true && orientation(RCH[RCHindex], LCH[LCHindex], LCH[(LCHindex + 1) % LCH.Count]) == 0)
                    LCHindex = (LCHindex + 1) % LCH.Count;

                oldRCH = RCHindex;
                FromRtoL = new Line(ULP, URP);
                while(orientation(LCH[LCHindex], RCH[RCHindex], RCH[(RCH.Count + RCHindex - 1) % RCH.Count]) < 0)
                {
                    RCHindex = (RCH.Count + RCHindex - 1) % RCH.Count;
                    FromRtoL = new Line(ULP, RCH[RCHindex]);
                    done = false;
                }
                if(done == true && orientation(LCH[LCHindex], RCH[RCHindex], RCH[(RCH.Count + RCHindex - 1) % RCH.Count]) == 0)
                    RCHindex = (RCH.Count + RCHindex - 1) % RCH.Count;
            }
            int uppera = LCHindex, upperb = RCHindex;
            #endregion

            #region Down supporting line
            ULP = MLP;
            URP = MRP;
            LCHindex = ULCHCounter;
            RCHindex = URCHCounter;
            done = false;
            iter = 0;
            while (!done)
            {
                done = true;
                Line FromRtoL = new Line(ULP, URP);
                oldRCH = RCHindex;
                while(orientation(LCH[LCHindex], RCH[RCHindex], RCH[(RCHindex + 1) % RCH.Count]) > 0)
                {
                    RCHindex = (RCHindex + 1) % RCH.Count;
                    FromRtoL = new Line(ULP, RCH[RCHindex]);
                    done = false;
                }
                if(done == true && orientation(LCH[LCHindex], RCH[RCHindex], RCH[(RCHindex + 1) % RCH.Count]) == 0)
                    RCHindex = (RCHindex + 1) % RCH.Count;

                FromRtoL = new Line(URP, ULP);
                oldLCH = LCHindex;
                //while (!HelperMethods.CheckTurn(FromRtoL, LCH[(LCH.Count + LCHindex - 1) % LCH.Count]).Equals(Enums.TurnType.Left))
                while(orientation(RCH[RCHindex], LCH[LCHindex], LCH[(LCH.Count + LCHindex - 1 ) % LCH.Count]) < 0)
                {
                    LCHindex = (LCH.Count + LCHindex - 1) % LCH.Count;
                    FromRtoL = new Line(URP, LCH[LCHindex]);

                    done = false;
                }
                if(done == true && orientation(RCH[RCHindex], LCH[LCHindex], LCH[(LCH.Count + LCHindex - 1) % LCH.Count]) == 0)
                    LCHindex = (LCH.Count + LCHindex - 1) % LCH.Count;
            }
            int lowera = LCHindex, lowerb = RCHindex;
            #endregion

            #region Get results
            List<Point> ret = new List<Point>();
            //ret contains the convex hull after merging the two convex hulls
            //with the points sorted in anti-clockwise order
            int ind = uppera;
            ret.Add(LCH[uppera]);
            while (ind != lowera)
            {
                ind = (ind + 1) % LCH.Count;
                if(!ret.Contains(LCH[ind]))
                    ret.Add(LCH[ind]);
            }
            ind = lowerb;
            ret.Add(RCH[lowerb]);
            while (ind != upperb)
            {
                ind = (ind + 1) % RCH.Count;
                if(!ret.Contains(RCH[ind]))
                    ret.Add(RCH[ind]);
            }
            return ret;
            #endregion
        }
        public override string ToString()
        {
            return "Convex Hull - Divide & Conquer";
        }
        public List<Point> JarvisMarchAlgo(List<Point> points)
        {
            List<Point> output = new List<Point>();
            // Sort points on Y axis 
            points.Sort(HelperMethods.sortOnY);
            int i = 0; // index of output Points 
            List<Point> l = new List<Point>(); // output points list 
            l.Add(points[0]); // Add first point to start from it 
            output.Add(points[0]);

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
                            AB = HelperMethods.getVector(l[i], Virtual);
                        else
                        {
                            if (Virtual == l[i - 1])
                                continue;
                            AB = HelperMethods.getVector(Virtual, l[i - 1]);
                        }
                        Point AC = HelperMethods.getVector(l[i], points[j]);


                        double dotProdct = HelperMethods.DotProduct(AB, AC);

                        double Radian = HelperMethods.magnitude(AB, AC);
                        double CosValue = dotProdct / Radian;
                        double Degree = Math.Acos(CosValue) * (180 / Math.PI);


                        if (Degree < 0)
                            Degree += 360;
                        if (i == 0)
                        {

                            if (Degree < MinDegree || (Degree == MinDegree && HelperMethods.GetDistance(l[i], points[j]) > HelperMethods.GetDistance(l[i], MinPoint)))
                            {
                                MinDegree = Degree;
                                MinPoint = points[j];
                            }
                        }
                        else
                        {
                            if (Degree > MinDegree || (Degree == MinDegree && MinPoint != null && HelperMethods.GetDistance(l[i], points[j]) > HelperMethods.GetDistance(l[i], MinPoint)))
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
                            output.Add(MinPoint);
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

            return output;
        }
    }
}
