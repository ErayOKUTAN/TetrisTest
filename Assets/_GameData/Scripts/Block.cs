using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace _GameData.Scripts
{
    public class Block : MonoBehaviour
    {
        [SerializeField]private ColorsType colorsType;

        public ColorsType ColorsType => colorsType;
        private GridUnit _currentGridUnit;

        public GridUnit CurrentGridUnit => _currentGridUnit;

        public SkinnedMeshRenderer meshRenderer;

        public LayerMask currentLayer;
        public Action OnBlockDestroy;

        [SerializeField] private List<Transform> hooks;
        [SerializeField] private List<MeshRenderer> hooksMeshRenderers;


        private bool _isOnSlot;

        public bool IsOnSlot => _isOnSlot;

        public List<Transform> activeHooks;
        public void Init(ColorData data)
        {
            _isOnSlot = true;
            meshRenderer.material = data.material;
            colorsType = data.colorsType;
            foreach (var meshRenderer in hooksMeshRenderers)
            {
                meshRenderer.material = data.material;
            }
            MergeController.OnTryToFindMergedData += OnTryToFindMergedDataHandler;
        }

        private void OnTryToFindMergedDataHandler()
        {
            if (_currentGridUnit != null)
            {
                TryToFindMergeData();    
            }
        }
        

        public bool TryToDrop(LayerMask gridLayer)
        {
            if (Physics.Raycast(transform.position, Vector3.forward, out var hitInfo, 100, gridLayer)&& hitInfo.transform.TryGetComponent(out GridUnit gridUnit))
            {
                return !gridUnit.isFull;
            }
            else
            {
                return false;
            }
        }

        public void Drop(LayerMask gridLayer)
        {
            if (Physics.Raycast(transform.position, Vector3.forward, out var hitInfo, 100, gridLayer)&& hitInfo.transform.TryGetComponent(out GridUnit gridUnit))
            {
                _isOnSlot = false;
                transform.SetParent(null);
                transform.localPosition = gridUnit.transform.position;
                gridUnit.SetGridUnit(this);
                _currentGridUnit = gridUnit;
            }
        }

        public void TryToFindMergeData()
        {
            var mergedData = new MergeController.MergeData
            {
                blocks = new List<Block>()
            };
            mergedData.blocks.Add(this);

            
            if (Physics.Raycast(transform.position,  Vector3.right, out var hitInfoRight, 10,currentLayer) && hitInfoRight.transform.gameObject != gameObject)
            {
             
                if (hitInfoRight.transform.TryGetComponent(out Block block) && block.colorsType == colorsType && !block._isOnSlot)
                {
                    // Debug.Log("Right " + colorsType +" " + block.colorsType,gameObject);
                    // Debug.DrawRay(transform.position, transform.right * hitInfoRight.distance, Color.yellow,10f);
                        activeHooks.Add(hooks[0]);
                        mergedData.blocks.Add(block);
                    
                    
                }
            }
			
            if (Physics.Raycast(transform.position, Vector3.left, out var hitInfoLeft, 10,  currentLayer) && hitInfoLeft.transform.gameObject != gameObject )
            {
               
                if (hitInfoLeft.transform.TryGetComponent(out Block block) && block.colorsType == colorsType&& !block._isOnSlot)
                {
                    // Debug.Log("Left " + colorsType +" " + block.colorsType,gameObject);
                    // Debug.DrawRay(transform.position, -transform.right * hitInfoLeft.distance, Color.yellow,10f);
                   
                        
                        activeHooks.Add(hooks[1]);
                        mergedData.blocks.Add(block);
                    
                    

                }
            }
			
            if (Physics.Raycast(transform.position, Vector3.up, out var hitInfoUp, 10,  currentLayer) && hitInfoUp.transform.gameObject != gameObject )
            {
                
              
                if (hitInfoUp.transform.TryGetComponent(out Block block) && block.colorsType == colorsType&& !block._isOnSlot)
                {
                    // Debug.Log("Up " + colorsType +" " + block.colorsType,gameObject);
                    // Debug.DrawRay(transform.position, transform.up * hitInfoUp.distance, Color.yellow,10f);
                   
                        activeHooks.Add(hooks[2]);
                        mergedData.blocks.Add(block);
                    

                   
                }
            }
			
            if (Physics.Raycast(transform.position,  Vector3.down, out var hitInfoDown, 10,  currentLayer) && hitInfoDown.transform.gameObject != gameObject )
            {
             
                if (hitInfoDown.transform.TryGetComponent(out Block block) && block.colorsType == colorsType&& !block._isOnSlot)
                {
                    // Debug.Log("Down " + colorsType +" " + block.colorsType,gameObject);
                    // Debug.DrawRay(transform.position, -transform.up * hitInfoDown.distance, Color.yellow,10f);
                   
                        activeHooks.Add(hooks[3]);
                        mergedData.blocks.Add(block);
                    
                  
                }
            }

        
            if (mergedData.blocks.Count > 1)
            {
                MergeController.OnMergeBlockData.Invoke(mergedData);    
            }
        }
        


        public void Destroy()
        {
            MergeController.OnTryToFindMergedData -= OnTryToFindMergedDataHandler;
            _currentGridUnit.EmptySlot();
            OnBlockDestroy.Invoke();
            Destroy(gameObject);
        }
    }
}
