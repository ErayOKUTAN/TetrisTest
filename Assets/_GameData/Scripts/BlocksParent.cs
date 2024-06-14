using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _GameData.Scripts
{
    public class BlocksParent : MonoBehaviour
    {
        public Transform createdPos;
        public List<Block> blocks;

        public LayerMask gridUnitLayer;
        private BlockGenerator _blockGenerator;
        public void Init(Transform slotPos,BlockGenerator blockGenerator)
        {
            _blockGenerator = blockGenerator;
            createdPos = slotPos;
            blocks = new List<Block>(GetComponentsInChildren<Block>());
        }
        
        public void ReturnToSlot()
        {
            transform.localScale = Vector3.one; 
            transform.SetParent(createdPos);
            transform.localPosition = Vector3.zero;
        }

        public void Drop()
        {
            var cantDropBlock = false;
            foreach (var block in blocks)
            {
                if (!block.TryToDrop(gridUnitLayer))
                {
                    cantDropBlock = true;
                    break;
                }
            }

            if (cantDropBlock)
            {
                ReturnToSlot();
            }
            else
            {
                foreach (var block in blocks)
                {
                    block.Drop(gridUnitLayer);
                }

                StartCoroutine(Delay());


            }
        }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(.1f);
            foreach (var block in blocks)
            {
                block.TryToFindMergeData();
            }
            _blockGenerator.SlotEmpty();
            Destroy(gameObject);
        }
    }
}
