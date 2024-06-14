using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _GameData.Scripts
{
    public class HighlightController : MonoBehaviour
    {
        public List<Vector3> willBeCheckedDirections;
        public LayerMask blockLayer;
        public LayerMask gridLayer;

        private GridUnit _findGridUnit;
        private List<GridUnit> _secondGridUnits;

        private GridUnit _firstHighLightGridUnit;
        private GridUnit _secondHighLightGridUnit;
        private Block _block;


        public void Awake()
        {
            _secondGridUnits = new List<GridUnit>();
            _block = GetComponent<Block>();
        }

        private void Start()
        {
            StartCoroutine(Delay());
        }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(.1f);
            FindEmptyDirection();
        }

        public void FindEmptyDirection()
        {
            var firstIndex = willBeCheckedDirections.Count - 1;
            for (int i = firstIndex;  i > -1 ; i--)
            {
                if (Physics.Raycast(transform.position, willBeCheckedDirections[i], out var hitInfo, 0.62f, blockLayer))
                {
                    
                    willBeCheckedDirections.Remove(willBeCheckedDirections[i]);
                }
            }
        }

        public void Highlight(GridUnit gridUnit)
        {
            if (_findGridUnit != gridUnit)
            {
                HighlightManager.OnResetHighlightGrids.Invoke();
            }
            _findGridUnit = gridUnit;
           
            foreach (var direction in willBeCheckedDirections)
            {
                if (Physics.Raycast(gridUnit.transform.position, direction, out var blockHitInfo, 50, blockLayer) && blockHitInfo.transform.TryGetComponent(out Block block) )
                {
                    if (block.ColorsType == _block.ColorsType && !block.IsOnSlot)
                    {
                        _secondGridUnits.Add(block.CurrentGridUnit);
                        HighlightManager.OnHighlightGrids.Invoke(gridUnit,block.CurrentGridUnit,block.ColorsType);
                    }
                }
            } 
        }
        
        public GridUnit FindGrid()
        {
            if (Physics.Raycast(transform.position, Vector3.forward, out var gridHitInfo, 50, gridLayer) && gridHitInfo.transform.TryGetComponent(out GridUnit gridUnit))
            {
                return gridUnit;
            }
            return null;
        }
    
    }
    
}
