using CGUtilities;
using System;
using System.Collections.Generic;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class DivideAndConquer : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {

            points.Sort(HelperMethods.sortOnX);
            List<Point> Resultpoints = new List<Point>();
            //outpoints.Add(Divide(points));
            Resultpoints = Divide(points);
            for (int i = 0; i < Resultpoints.Count; i++)
                outPoints.Add(Resultpoints[i]);
                            
            //outPoints.Add(points[index]);

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
                int MI = (points.Count / 2) - 1; // get mid point of points 
                List<Point> Left = new List<Point>(), Right = new List<Point>();
                for (int i = 0; i < MI; i++)
                    Left.Add(points[i]); // Add all left point in the list to Left list
                for (int i = MI; i < points.Count; i++)
                    Right.Add(points[i]); // Add all right point in the list to Right list
                // pass and return new points 
                List<Point> LCH = Divide(Left); 
                List<Point> RCH = Divide(Right);
                return Merge(LCH,RCH);
            }
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
                else if (RCH[i].X == x && RCH[i].Y > y)
                {
                    x = RCH[i].X;
                    y = RCH[i].Y;
                    RCHindex = i;
                }
            }
            Point MRP = null;
            if (RCH.Count > 0)
                MRP = RCH[RCHindex];
            #endregion
            int ULCHCounter = LCHindex;
            int URCHCounter = RCHindex;
            Point ULP = MLP, URP = MRP;
            Point NextLP = null;
            if (LCH.Count > 0 && ULCHCounter < LCH.Count-1)
                NextLP = LCH[ULCHCounter+ 1];
            Point PreRP = null;
            if (RCH.Count > 0 && URCHCounter > 0)
                PreRP = RCH[URCHCounter -1];
            
            bool changeLCH = false;
            bool changeRCH = false;

            #region Up Supporting Line
            do
            {

                while (true)
                {
                    Line FromRtoL = null;
                    if (URP != null && ULP != null && NextLP != null)
                    {
                        FromRtoL = new Line(URP, ULP);
                        Enum e = HelperMethods.CheckTurn(FromRtoL, NextLP);
                        if (e.Equals(Enums.TurnType.Left) || e.Equals(Enums.TurnType.Colinear))
                        {
                            changeLCH = false;
                            break;
                        }
                        else
                        {

                            changeLCH = true;
                            ULP = NextLP;
                            if (ULCHCounter < LCH.Count - 1)
                            {
                                NextLP = LCH[ULCHCounter + 1];
                                ULCHCounter++;
                            }
                        }
                    }
                    else
                    {
                        changeLCH = false;
                        break;
                    }
                }
                while (true)
                {
                    Line FromRtoL = null;
                    if (URP != null && ULP != null && PreRP != null)
                    {
                        FromRtoL = new Line(ULP, URP);
                        Enum e = HelperMethods.CheckTurn(FromRtoL, PreRP);
                        if (e.Equals(Enums.TurnType.Right) || e.Equals(Enums.TurnType.Colinear))
                        {
                            changeRCH = false;
                            break;
                        }
                        else
                        {
                            changeRCH = true;
                            URP = PreRP;
                            if (URCHCounter > 0)
                            {
                                PreRP = RCH[URCHCounter - 1];
                                URCHCounter--;
                            }
                        }
                    }
                    else
                    {
                        changeRCH = false;
                        break;
                    }
                }
            } while (changeRCH || changeLCH);
            #endregion
            int DLCHCounter = LCHindex;
            int DRCHCounter = RCHindex;
            Point DLP = MLP, DRP = MRP;
            Point PreLP = null;
            if (LCH.Count > 0 && DLCHCounter > 0)
              PreLP = LCH[DLCHCounter-1];
            Point NextRP = null;
            if (RCH.Count > 0 && DRCHCounter < RCH.Count-1)
                NextRP = RCH[DRCHCounter+1];
            changeLCH = false;
            changeRCH = false;

            #region Down Supporting Line
            do
            {

                while (true)
                {
                    Line FromRtoL = null;
                    if (URP != null && ULP != null && PreRP != null)
                    {
                        FromRtoL = new Line(DRP, DLP);
                        Enum e = HelperMethods.CheckTurn(FromRtoL, PreLP);
                        if (e.Equals(Enums.TurnType.Left) || e.Equals(Enums.TurnType.Colinear))
                        {
                            changeLCH = false;
                            break;
                        }
                        else
                        {
                            changeLCH = true;
                            ULP = NextLP;
                            if (DLCHCounter > 0)
                            {
                                NextLP = LCH[DLCHCounter - 1];
                                DLCHCounter--;
                            }
                        }
                    }
                    else
                    {
                        changeLCH = false;

                        break;
                    }
                }
                while (true)
                {
                    Line FromRtoL = null;
                    if (URP != null && ULP != null && NextRP != null)
                    {
                        FromRtoL = new Line(DLP, DRP);
                        Enum e = HelperMethods.CheckTurn(FromRtoL, NextRP);
                        if (e.Equals(Enums.TurnType.Right) || e.Equals(Enums.TurnType.Colinear))
                        {
                            changeRCH = false;
                            break;
                        }
                        else
                        {
                            changeRCH = true;
                            URP = PreRP;
                            if (DRCHCounter < RCH.Count - 1)
                            {
                                PreRP = RCH[DRCHCounter + 1];
                                DRCHCounter++;
                            }
                        }
                    }
                    else
                    {
                        changeRCH = false;

                        break;
                    }
                }
            } while (changeRCH || changeLCH);

            #endregion
            List<Point> AllCH = new List<Point>();
            for (int i = ULCHCounter; i < DLCHCounter;i++)
                AllCH.Add(LCH[i]);
            for (int i = URCHCounter; i < DRCHCounter; i++)
                AllCH.Add(RCH[i]);
            

            return AllCH;
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
