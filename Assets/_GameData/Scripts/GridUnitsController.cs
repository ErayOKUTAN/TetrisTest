using System.Collections.Generic;
using UnityEngine;

namespace _GameData.Scripts
{
    public class GridUnitsController 
    {
        public static GridUnit[,] GridUnits { get; set; }

        public GridUnitsController(GridUnit[,] gridUnits)
        {
            GridUnits = gridUnits;
        }
        
        public static List<GridUnit> FindGridsBetween(GridUnit A, GridUnit B)
        {
            List<GridUnit> betweenGrids = new List<GridUnit>();
            int x1 = A.IndexX, y1 = A.IndexY;
            int x2 = B.IndexX, y2 = B.IndexY;

            if (y1 == y2) // Aynı satırda
            {
                if (x1 < x2)
                {
                    for (int x = x1 + 1; x < x2; x++)
                    {
                        betweenGrids.Add(GridUnits[x, y1]);
                    }
                }
                else
                {
                    for (int x = x1 - 1; x > x2; x--)
                    {
                        betweenGrids.Add(GridUnits[x, y1]);
                    }
                }
            }
            else if (x1 == x2) // Aynı sütunda
            {
                if (y1 < y2)
                {
                    for (int y = y1 + 1; y < y2; y++)
                    {
                        betweenGrids.Add(GridUnits[x1, y]);
                    }
                }
                else
                {
                    for (int y = y1 - 1; y > y2; y--)
                    {
                        betweenGrids.Add(GridUnits[x1, y]);
                    }
                }
            }

            return betweenGrids;
        }

        public void CheckGridUnitNeighbours()
        {
            foreach (var gridUnit in GridUnits)
            {
                gridUnit.CheckNeighbours();
            }
        }
    }
}
