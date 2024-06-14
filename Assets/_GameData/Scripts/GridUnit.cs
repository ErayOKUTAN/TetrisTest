using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _GameData.Scripts
{
    public class GridUnit : MonoBehaviour
    {
        public bool isFull;

        private Block _currentBlock;
        

        private int _indexX;

        public int IndexX => _indexX;

        public int IndexY => _indexY;

        private int _indexY;

        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private List<GameObject> topBorders;
        [SerializeField] private List<GameObject> leftBorders;
        [SerializeField] private List<GameObject> rightBorders;
        [SerializeField] private List<GameObject> bottomBorders;
        private Material _defaultMat;
        
        
        private bool _isThereTop;
        private bool _isThereBottom;
        private bool _isThereLeft;
        private bool _isThereRight;
        public void CheckNeighbours()
        {
            _isThereTop = SendRay(Vector3.up, topBorders);
            _isThereBottom = SendRay(Vector3.down, bottomBorders);
            _isThereLeft = SendRay(Vector3.left, leftBorders);
            _isThereRight = SendRay(Vector3.right, rightBorders);
        }

        private bool SendRay(Vector3 dir, List<GameObject> borderList)
        {
            Ray ray = new Ray(transform.position + Vector3.forward, dir);
            if (Physics.Raycast(transform.position + Vector3.forward, dir, out var hitInfo, 0.75f))
            {
                Debug.DrawRay(ray.origin, ray.direction * 0.75f, Color.magenta, 1);
                if (hitInfo.transform.TryGetComponent(out GridUnit gridUnit))
                {
                    borderList.ForEach(i => i.SetActive(false));
                    return true;
                }
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * 0.75f, Color.black, 1);
            }

            return false;
        }
        
        
        public void Init(int indexX,int indexY)
        {
            _indexX = indexX;
            _indexY = indexY;
            _defaultMat = _meshRenderer.material;

        }
        public void SetGridUnit(Block block)
        {
            _currentBlock = block;
            isFull = true;
        }

        public void EmptySlot()
        {
            _currentBlock = null;
            isFull = false;
        }

        public void SetMaterial(Material material)
        {
            _meshRenderer.material = material;
        }

        public void ResetMaterial()
        {
            _meshRenderer.material = _defaultMat;
        }
    }
}
