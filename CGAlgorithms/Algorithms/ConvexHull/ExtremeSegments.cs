using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremeSegments : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {

            List<bool> visted_list = new List<bool>(points.Count);
            for (int i = 0; i < points.Count; i ++)
                visted_list.Add(true);
              
            //######### Extrem Segments Algorithm ######## 

            for (int i = 0; i < points.Count; i ++)
            {

                for (int j = 0; j < points.Count; j++)
                {
                    if (j == i)
                        continue;
                    // Exvlude Repetition Points  
                    if (points[i].X == points[j].X && points[i].Y == points[j].Y)
                    {
                        points.Remove(points[j]);
                         continue; 
                    }
                    bool flag = true ;
                    int direction = -500;

                    for (int k = 0; k < points.Count; k++)
                    {
                        if (k == j || k == i)
                            continue;
                        // First We Have Three Cases :
                        //1 - No Colinear 
                        //2 - Colinear with In Between 
                        //3 - Colinear With Not In Between 
                        if (direction == -500)
                        {
                            if (HelperMethods.CheckTurn(new Line(points[i], points[j]), points[k]) == Enums.TurnType.Colinear)
                            {
                                // Case 2 
                                if (HelperMethods.IN_Between(new Line(points[i], points[j]), points[k]))
                                    visted_list[k] = false;   
                                // Case 3 
                                else
                                {
                                    flag = false;
                                    break;
                                }
                            }
                            //Case 1 
                            else
                                direction = Convert.ToInt32(HelperMethods.CheckTurn(new Line(points[i], points[j]), points[k])); 

                            continue; 
                        }
                        // In iterations always we have 4 Cases 
                        // 1 -Point Has Same Directin >> No Action Keep Iterateing .. 
                        // 2 - Point has Different Direction .. (Left Or Right ) 
                        // 3 - point has Different Direction With Colinear and In Between Segment 
                        // 4 -  point has Different Direction With Colinear and Not In Between Segment 

                        if (Convert.ToInt32(HelperMethods.CheckTurn(new Line(points[i], points[j]), points[k])) != direction)
                        {
                          
                            if (HelperMethods.CheckTurn(new Line(points[i], points[j]), points[k]) == Enums.TurnType.Colinear)
                            {
                                // Case 3 : 
                                if (HelperMethods.IN_Between(new Line(points[i], points[j]), points[k]))
                                { 
                                    visted_list[k] = false;
                                    continue; 
                                }
                                // else >> Case 4 .. Same Action With Case 2 
                            } 
                            // Case 2  .. 
                            flag = false; 
                            break;
                        }
                    }

                    if (flag)
                    {
                        
                        if (visted_list[i] || visted_list[j])
                        {
                            if (visted_list[i])
                                outPoints.Add(points[i]) ;
                            if (visted_list[j] )
                                outPoints.Add(points[j]);
                            visted_list[i] = visted_list[j] =  false; 
                        }

                             
                    }
                        
                }
            }
            
            // Constent Time To Handle Base Case less than 2 after delete repetition
            if (points.Count <= 2 )
            {
                for (int i = 0; i <points.Count; i++ )
                    if (visted_list[i])
                        outPoints.Add(points[i]);

            }
            
          
            
            
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Segments";
        }
    }
}
