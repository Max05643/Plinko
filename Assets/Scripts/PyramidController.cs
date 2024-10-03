using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using Zenject;

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
        public GameObject finishBlockPrefab;
    }

    [Inject]
    SoundController soundController;

    [Inject]
    UIController uiController;


    [SerializeField]
    PyramidSettings settings;

    List<List<GameObject>> blocks;

    List<GameObject> finishBlocks;

    public int Height => settings.height;
    public float BlockSize => settings.blockSize;

    void Start()
    {
        GeneratePyramid();
    }

    void GeneratePyramid()
    {
        GenerateBlocks();
        GenerateFinishBlocks();
        EditorUtility.SetDirty(gameObject);
    }

    void GenerateFinishBlocks()
    {
        finishBlocks = new List<GameObject>();
        for (int i = 0; i <= settings.height + 1; i++)
        {
            var block = Instantiate(settings.finishBlockPrefab, transform);
            block.transform.localPosition = PositionForBlock(settings.height + 1, i);
            finishBlocks.Add(block);
        }
    }

    void GenerateBlocks()
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


    public void RegisterBlockReached(int row, int column)
    {
        PlayInteractAnimation(row, column);
        soundController.PlayClip(2);
    }
    public void RegisterFinishBlockReached(int column)
    {
        PlayFinishAnimation(column);
        soundController.PlayClip(3);
        uiController.AddScore();
    }

    void PlayInteractAnimation(int row, int column)
    {
        blocks[row][column].GetComponent<SpriteRenderer>().DOKill();
        blocks[row][column].GetComponent<SpriteRenderer>().DOColor(Color.green, 0.2f).SetLoops(2, LoopType.Yoyo);
    }

    void PlayFinishAnimation(int column)
    {
        finishBlocks[column].transform.DOKill();
        finishBlocks[column].transform.DOScale(new Vector3(0.375f, 1.25f, 1), 0.2f).SetLoops(2, LoopType.Yoyo);

        finishBlocks[column].GetComponent<SpriteRenderer>().DOKill();
        finishBlocks[column].GetComponent<SpriteRenderer>().DOColor(Color.green, 0.2f).SetLoops(2, LoopType.Yoyo);
    }


}
