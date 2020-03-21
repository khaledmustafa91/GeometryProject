using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class QuickHull : Algorithm
    {
		public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
		{
			// getting the point  with the lowest X and greatest X: O(n)
			List<Point> Min_Max_Points = new List<Point>();
			Min_Max_Points = Get_Min_Max_X(points);

			// Add to convex hull
			outPoints.Add(Min_Max_Points[0]);
			outPoints.Add(Min_Max_Points[1]);

			// Divide the points for 2 Sides:
			List<Point> Side1_Points = new List<Point>();
			Line Base_Line_1 = new Line(Min_Max_Points[0], Min_Max_Points[1]);
			Side1_Points = Get_Side(Base_Line_1, points);
			Find_Hull(Side1_Points, Base_Line_1, ref outPoints);


			List<Point> Side2_Points = new List<Point>();
			Line Base_Line_2 = new Line(Min_Max_Points[1], Min_Max_Points[0]);
			Side2_Points = Get_Side(Base_Line_2, points);
			Find_Hull(Side2_Points , Base_Line_2, ref outPoints);
			
			if(points.Count == 1)
			{
				outPoints.Clear();
				outPoints.Add(points[0]);
			}
		}
		//-----------------------------------------------------------------------------------
		// find the point  with the lowest X and greatest X: O(n)
		public List<Point> Get_Min_Max_X(List<Point> C_H_Points)
		{
			double MIN = 10000000;
			double MAX = 0;
			int index_X_Min = 0;
			int index_X_Max = 0;
			List<Point> Min_Max_Points = new List<Point>();

			for (int i = 0; i < C_H_Points.Count; i++)
			{
				if (C_H_Points[i].X < MIN)
				{
					MIN = C_H_Points[i].X;
					index_X_Min = i;
				}
				if (C_H_Points[i].X > MAX)
				{
					MAX = C_H_Points[i].X;
					index_X_Max = i;
				}
			}
			Min_Max_Points.Add(C_H_Points[index_X_Min]);
			Min_Max_Points.Add(C_H_Points[index_X_Max]);
			return Min_Max_Points;
		}
		//-----------------------------------------------------------------------------------
		// getting the Right_List :
		public List<Point> Get_Side(Line Base_Line, List<Point> Points)
		{
			List<Point> Right_Points = new List<Point>();
			for (int i = 0; i < Points.Count ; i++)
			{
				if (CGUtilities.HelperMethods.CheckTurn(Base_Line, Points[i]) == CGUtilities.Enums.TurnType.Left)
				{
					Right_Points.Add(Points[i]);
				}
			}
			return Right_Points;
		}
		//-----------------------------------------------------------------------------------
		// getting the Orthogonal Distances to find the maximum Hieght:
		public Point Getting_The_Orthogonal_Distances (Line Base_Line , List<Point> Points)
		{
			Point Maximum_Hieght ;
			Point Vector_BA ;
			Point Vector_BC;
			double Cross_Product , Max_Hieght , AC ;
			List<KeyValuePair<Point, double>> Final = new List<KeyValuePair<Point, double>>();
			KeyValuePair<Point, double> tmp ;

			for (int i = 0; i < Points.Count; i++)
			{
				Vector_BA = new Point((Base_Line.Start.X - Points[i].X) , (Base_Line.Start.Y - Points[i].Y));
				Vector_BC = new Point((Base_Line.End.X - Points[i].X), (Base_Line.End.Y - Points[i].Y));
				Cross_Product = CGUtilities.HelperMethods.CrossProduct(Vector_BA , Vector_BC);
				AC = (Math.Sqrt(Math.Pow((Base_Line.Start.X - Base_Line.End.X), 2) + Math.Pow((Base_Line.Start.Y - Base_Line.End.Y), 2)));
				Max_Hieght = Cross_Product / AC ;
				tmp = new KeyValuePair<Point, double>(Points[i] , Max_Hieght);			
	            Final.Add(tmp);
			}

			Final.Sort((x, y) => x.Value.CompareTo(y.Value));
			Maximum_Hieght = Final[Final.Count - 1].Key;
			return Maximum_Hieght;
		}
		//-------------------------------------------------------------------------------------
		public List<Point> Find_Hull(List<Point> Points , Line Segment , ref List<Point> Hull)
		{
			// Stopping Condition
			if(Points.Count == 0)
			{
				return Hull;
			}

			// getting the Orthogonal Distance for all points in Left side AND Right side:
			Point Maximum_Hieght = new Point(0, 0);
			Maximum_Hieght = Getting_The_Orthogonal_Distances(Segment , Points);
			Hull.Add(Maximum_Hieght);

			// Points are devided into 3 regoins 
			//1- S0 inside traingle neglect
			//2- S1 left BC
			List<Point> Side1 = new List<Point>();
			Line BC = new Line(Segment.Start , Maximum_Hieght);
			Side1 = Get_Side(BC, Points);
			Find_Hull(Side1, BC, ref Hull);

			//3- S2 Right BA
			List<Point> Side2 = new List<Point>();
			Line BA = new Line(Maximum_Hieght, Segment.End);
			Side2 = Get_Side(BA, Points);
			Find_Hull(Side2 , BA , ref Hull);

			return Hull;
		}
		//-------------------------------------------------------------------------------
		public override string ToString()
        {
            return "Convex Hull - Quick Hull";
        }
    }
}
