using System;
using UnityEngine;

namespace _GameData.Scripts
{
    public class InputManager : MonoBehaviour
    {
        private BlocksParent _holdingBlocks;
        private bool _isHold;

        private Camera _mainCam;

        public LayerMask blocksLayer;
        public LayerMask dragPanelLayer;


        public static Action<BlocksParent> OnHoldingBlock;
        public static Action<BlocksParent> OnDropBlock;
        public void Awake()
        {
            _mainCam = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hitInfo,blocksLayer))
                {
                    if(hitInfo.transform.TryGetComponent(out BlocksParent blocksParent))
                    {
                        _isHold = true;
                        _holdingBlocks = blocksParent;
                        _holdingBlocks.transform.localScale = Vector3.one;
                        OnHoldingBlock.Invoke(_holdingBlocks);
                    }
                  
                }
            }
            else if(Input.GetMouseButton(0))
            {
                if (_isHold)
                {
                    Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out var hit , 100, dragPanelLayer))
                    {
                        var holdingBlocksTransform = _holdingBlocks.transform;
                        var pos = hit.point;
                        pos.z -= 1.5f;
                        holdingBlocksTransform.position = Vector3.Lerp(holdingBlocksTransform.position, pos, Time.deltaTime * 12);
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (_isHold)
                {
                    OnDropBlock.Invoke(_holdingBlocks);
                    _holdingBlocks.Drop();
                    _isHold = false;
                }
            }
            
        }
    }
}
