using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace _GameData.Scripts
{
    public class GridGenerator : MonoBehaviour
    {
        public GridUnit gridUnitPrefab;

        public Vector3 startPos;

        private GridUnitsController _gridUnitsController;
        
        private void Awake()
        {
            var instantiatePos = startPos;
            
            GridUnit[,] gridUnitsArray = new GridUnit[8,8];
            GameObject gridUnits = new GameObject { name = "GridUnits" };
            gridUnits.transform.SetParent(transform);
            
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var gridUnitIns = Instantiate(gridUnitPrefab, instantiatePos,Quaternion.identity,gridUnits.transform);
                    gridUnitsArray[j,i] = gridUnitIns;
                    gridUnitIns.Init(j,i);
                    instantiatePos.x++;
                }
                instantiatePos.y--;
                instantiatePos.x = startPos.x;
            }
            _gridUnitsController = new GridUnitsController(gridUnitsArray);
            _gridUnitsController.CheckGridUnitNeighbours();
        }
    }
}
