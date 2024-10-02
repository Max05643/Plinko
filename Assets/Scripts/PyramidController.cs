using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PyramidController : MonoBehaviour
{
    [System.Serializable]
    public class PyramidSettings
    {
        public int height;
        public Vector2 spacing = Vector2.one;
        public float blockSize = 1;
        public int startFromRow = 2;
        public GameObject blockPrefab;
    }


    [SerializeField]
    PyramidSettings settings;

    [SerializeField]
    List<List<GameObject>> blocks = new List<List<GameObject>>();

    public int Height => settings.height;
    public float BlockSize => settings.blockSize;

    [ContextMenu("Generate Pyramid")]
    void GeneratePyramid()
    {
        blocks = new List<List<GameObject>>();

        for (int i = 0; i < settings.startFromRow; i++)
        {
            blocks.Add(new List<GameObject>());
        }

        for (int row = settings.startFromRow; row <= settings.height; row++)
        {
            blocks.Add(new List<GameObject>());
            for (int column = 0; column <= row; column++)
            {
                var block = Instantiate(settings.blockPrefab, transform);
                block.transform.localPosition = PositionForBlock(row, column);
                blocks[row].Add(block);
            }
        }
    }


    public Vector2 PositionForBlock(int row, int column)
    {
        return new Vector2(-row * (settings.spacing.x * 0.5f) + column * settings.spacing.x, -row * settings.spacing.y);
    }

    public Vector2 PositionAboveBlock(int row, int column)
    {
        return PositionForBlock(row, column) + Vector2.up * settings.blockSize;
    }


}
