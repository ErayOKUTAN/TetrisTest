using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace _GameData.Scripts
{
    public class BlockGenerator : MonoBehaviour
    {
        public List<BlocksParent> blockTemplates;
        public List<Transform> blockInsPositions;
        
        public List<ColorData> colorDatas;

        private int _emptyedSlotCount;
        public void Awake()
        {
            foreach (var position in blockInsPositions)
            {
                CreateBlock(position);
            }
        }

        private void CreateBlock(Transform position)
        {
            var blockTemplate = Instantiate(blockTemplates[Random.Range(0, blockTemplates.Count)],position);
            
            blockTemplate.Init(position,this);
            blockTemplate.transform.localScale = Vector3.one; 
            List<bool> selectedColorIndex = new List<bool>();
            for (int j = 0; j < colorDatas.Count; j++)
            {
                selectedColorIndex.Add(false);
            }
            int i = 0;
            while (i < blockTemplate.blocks.Count)
            {
                ColorData colorData = colorDatas[Random.Range(0, colorDatas.Count)];
                var index = colorDatas.IndexOf(colorData);
                if (!selectedColorIndex[index])
                {
                    selectedColorIndex[index] = true;
                    blockTemplate.blocks[i].Init(colorData);
                    i++;
                }
            }
        }

        public void SlotEmpty()
        {
            _emptyedSlotCount++;
            if (_emptyedSlotCount == 3)
            {
                foreach (var position in blockInsPositions)
                {
                    CreateBlock(position);
                    _emptyedSlotCount = 0;
                }
            }
        }

    
    }

    [Serializable]
    public struct ColorData
    {
        public ColorsType colorsType;
        public Material material;
    }
}
