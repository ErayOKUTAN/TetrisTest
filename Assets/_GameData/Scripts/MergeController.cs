using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _GameData.Scripts
{
    public class MergeController : MonoBehaviour
    {
        [Serializable]
        public struct MergeData
        {
            public List<Block> blocks;
        }

        public static Action<MergeData> OnMergeBlockData;
        public static Action OnTryToFindMergedData;

        private bool _isMergeDelayStarted;

        private List<MergeData> _datas;
        private void OnEnable()
        {
            OnMergeBlockData += OnMergeBlockDataHandler;
        }

        public void Awake()
        {
            _datas = new List<MergeData>();
        }

        private void OnMergeBlockDataHandler(MergeData mergeData)
        {
            _datas.Add(mergeData);
            if (!_isMergeDelayStarted)
            {
                _isMergeDelayStarted = true;
                StartCoroutine(MergeDelay());    
            }
            
        }

        public void StartMerge()
        {
            var mostLengthData = _datas[0];
            for (int i = 1; i < _datas.Count; i++)
            {
                if (mostLengthData.blocks.Count < _datas[i].blocks.Count)
                {
                    mostLengthData = _datas[i];
                }
            }

            StartCoroutine(MergeAnim(mostLengthData));

        } 

        IEnumerator MergeDelay()
        {
            yield return new WaitForSeconds(0.1f);
            StartMerge();
        }

        IEnumerator MergeAnim(MergeData data)
        {
            float alpha = 0f;

            var moveBlocks = new Block[data.blocks.Count - 1];
            var moveBlocksPos = new Vector3[data.blocks.Count - 1];
            for (int i = 1; i < data.blocks.Count; i++)
            {
                moveBlocks[i - 1] = data.blocks[i];
                moveBlocksPos[i - 1] = data.blocks[i].transform.position;
            }
            
            var twoBlockBetweenMoveTime = 0.1f;
            var mostClosedDistance = Vector3.Distance(data.blocks[0].transform.position, moveBlocks[0].transform.position);
            if (moveBlocks.Length > 1)
            {
                for (int i = 1; i < moveBlocks.Length; i++)
                {
                    var distance = Vector3.Distance(data.blocks[0].transform.position, moveBlocks[i].transform.position);
                    if (mostClosedDistance < distance)
                    {
                        mostClosedDistance = distance;
                    }
                }
            }

            var time = mostClosedDistance * twoBlockBetweenMoveTime;

            
            var distances = new List<float>();

            foreach (var blocksPos in moveBlocksPos)
            {
                distances.Add(Vector3.Distance(data.blocks[0].transform.position, blocksPos));
                print(Vector3.Distance(data.blocks[0].transform.position, blocksPos));
            }
            
            alpha = 0f;
            
            while (alpha < 1.0f)
            {
                alpha = Mathf.Min(1.0f, alpha + Time.deltaTime / .35f);
                print(alpha);
                for (int i = 0; i < data.blocks[0].activeHooks.Count; i++)
                {
                    var targetXScale = Mathf.Lerp(0, distances[i], alpha);
                    var scale = data.blocks[0].activeHooks[i].localScale;
                    scale.x = targetXScale;
                    data.blocks[0].activeHooks[i].localScale = scale;
                }
                yield return new WaitForEndOfFrame();
            }

            alpha = 0f;
            
            while (alpha < 1.0f)
            {
                alpha = Mathf.Min(1.0f, alpha + Time.deltaTime / time);
                
                for (int i = 0; i < data.blocks[0].activeHooks.Count; i++)
                {
                    var targetXScale = Mathf.Lerp(distances[i], 0, alpha);
                    var scale = data.blocks[0].activeHooks[i].localScale;
                    scale.x = targetXScale;
                    data.blocks[0].activeHooks[i].localScale = scale;
                }
                
                for (int i = 0; i < moveBlocks.Length; i++)
                {
                    var moveLerp = Vector3.Lerp(moveBlocksPos[i],data.blocks[0].transform.position  , alpha);
                    moveBlocks[i].transform.position = moveLerp;
                }
                yield return new WaitForEndOfFrame();
            }

            ProgressController.OnScore.Invoke(10,data.blocks[0].transform.position);
            
            foreach (var block in data.blocks)
            {
                block.Destroy();
            }

            yield return new WaitForSeconds(0.1f);
            _datas.Clear();
            _datas = new List<MergeData>();
            _isMergeDelayStarted = false;
            OnTryToFindMergedData.Invoke();
           
        }
        

        private void OnDisable()
        {
            OnMergeBlockData -= OnMergeBlockDataHandler;
        }
    }
    
    
    
    
}


    