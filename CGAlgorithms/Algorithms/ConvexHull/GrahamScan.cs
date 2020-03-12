using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
	public class GrahamScan : Algorithm
	{
		public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
		{
			// 1- find the point (anchor) with the lowest Y and lowest X: O(n)
			Point Anchor = Get_Anchor(points);
			Point Random_Horizontal = new Point(Anchor.X + 10000000, Anchor.Y);
			Line Horizontal_Line = new Line(Anchor, Random_Horizontal);
			points.Remove(Anchor);

			// 2- Sorting the remaining points by angle : O(nlogn)
			List<KeyValuePair<Point, double>> Sorted_Points = Sorting_By_Angle(points, Horizontal_Line);

			KeyValuePair<Point, double> tmpporary = new KeyValuePair<Point, double>(Anchor , 0);
			Sorted_Points.Add(tmpporary);

			// 3- Dealing with stack and orientation test:
			Stack<Point> Convex_Hull_points = new Stack<Point>();
			//initialization
			Convex_Hull_points.Push(Anchor);
			Convex_Hull_points.Push(Sorted_Points[0].Key);
			Line segment;
			Point Top, Pre_top;

			for (int i = 1; i < Sorted_Points.Count; i++)
			{
				Top = Convex_Hull_points.Pop();
				Pre_top = Convex_Hull_points.Pop();
				Convex_Hull_points.Push(Pre_top);
				Convex_Hull_points.Push(Top);
				segment = new Line(Top, Pre_top);

				while (Convex_Hull_points.Count > 2 && CGUtilities.HelperMethods.CheckTurn(segment, Sorted_Points[i].Key) != CGUtilities.Enums.TurnType.Left)
				{
					Convex_Hull_points.Pop();
					Top = Convex_Hull_points.Pop();
					Pre_top = Convex_Hull_points.Pop();
					Convex_Hull_points.Push(Pre_top);
					Convex_Hull_points.Push(Top);
					segment = new Line(Top, Pre_top);
				}
				Convex_Hull_points.Push(Sorted_Points[i].Key);
			}
			
			while (Convex_Hull_points.Count > 0)
			{
				outPoints.Add(Convex_Hull_points.Pop());
			}
			outPoints.RemoveAt(outPoints.Count - 1);
		}
		//---------------------------------------------------------
		public override string ToString()
		{
			return "Convex Hull - Graham Scan";
		}
		//-----------------------------------------------------------------------------------
		// 1- find the point (anchor) with the lowest Y and lowest X: O(n)
		public Point Get_Anchor(List<Point> C_H_Points)
		{
			double MIN = 10000000;
			int index = 0;
			for (int i = 0; i < C_H_Points.Count; i++)
			{
				if (C_H_Points[i].Y < MIN)
				{
					MIN = C_H_Points[i].Y;
					index = i;
				}
			}
			return C_H_Points[index];
		}
		//------------------------------------------------------------------------------------
		// Converting Radians to Dregree :
		public double Convert_RadiansTo_Degrees(double radians)
		{
			double degrees = (180 / Math.PI) * radians;
			return (degrees);
		}
		//-------------------------------------------------------------------
		// 2- Sorting the remaining points by angle : O(nlogn)
		public List<KeyValuePair<Point, double>> Sorting_By_Angle(List<Point> Un_Sorted_Points, Line Horizontal_Line)
		{
			List<KeyValuePair<Point, double>> Non_Sorted_Points = new List<KeyValuePair<Point, double>>();
			double Cross_Product, Dot_Product, Radian_Angel, Degree_Angel;
			KeyValuePair<Point, double> tmp_Pair;
			Point tmp_Point;

			// 2.1- Calculating Angels:
			Point Vector = new Point((Horizontal_Line.End.X - Horizontal_Line.Start.X), (Horizontal_Line.End.Y - Horizontal_Line.Start.Y));
			for (int i = 0; i < Un_Sorted_Points.Count; i++) //O(n)
			{
				tmp_Point = new Point((Un_Sorted_Points[i].X - Horizontal_Line.Start.X), (Un_Sorted_Points[i].Y - Horizontal_Line.Start.Y));
				Cross_Product = CGUtilities.HelperMethods.CrossProduct(Vector, tmp_Point);
				Dot_Product = CGUtilities.HelperMethods.DotProduct(Vector, tmp_Point);
				Radian_Angel = Math.Atan2(Dot_Product, Cross_Product);
				Degree_Angel = Convert_RadiansTo_Degrees(Radian_Angel);

				tmp_Pair = new KeyValuePair<Point, double>(Un_Sorted_Points[i], Degree_Angel);
				Non_Sorted_Points.Add(tmp_Pair);
			}
			// 2.2- Sorting Angels:
			Non_Sorted_Points.Sort((x, y) => x.Value.CompareTo(y.Value));
			return Non_Sorted_Points;
		}
	}
}

