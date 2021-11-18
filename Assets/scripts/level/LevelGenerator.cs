using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField,
     Tooltip("This denotes how much empty space should exist outside the level boundaries in each dimension")]
    private Vector3Int worldBuffer = new Vector3Int(10, 30, 10);

    [SerializeField]
    private GameObject tilePrefab;

    private const string SNAKE_LEVEL_FOLDER = "levels";
    private const string SNAKE_LEVEL_FILENAME = "snake_level_";
    private const string SNAKE_LEVEL_FILETYPE = ".txt";

    private int levelIndex = 0;
    private string levelFilePath;

    private static LevelGenerator instance;
    public static LevelGenerator Instance => instance;

    // Start is called before the first frame update
    void Awake()
    {
        levelFilePath = Application.streamingAssetsPath;
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        LoadLevel(0);
    }

    public void LoadLevel(int i)
    {
        levelIndex = i;
        string levelFile = SNAKE_LEVEL_FILENAME + levelIndex + SNAKE_LEVEL_FILETYPE;
        string pathToLevelFile = Path.Combine(levelFilePath, SNAKE_LEVEL_FOLDER);

        var levelData = LevelLoader.ParseLevelData(levelFile, pathToLevelFile);

        ConstructLevel(levelData.Data, levelData.BoundingBox);
    }

    public void LoadNextLevel()
    {
        levelIndex++;
        LoadLevel(levelIndex);
    }

    private void ConstructLevel(char[][] levelData, Vector3Int boundingBox)
    {
        Vector3Int worldSize = new Vector3Int(
            boundingBox.x + worldBuffer.x * 2,
            boundingBox.y + worldBuffer.y * 2,
            boundingBox.z + worldBuffer.z * 2
        );

        bool[,,] grid = new bool[worldSize.x, worldSize.y, worldSize.z];

        for (int z = 0; z < levelData.GetLength(0); z++) // used to be 0 -> worldsize
        {
            for (int x = 0; x < levelData[z].Length; x++) // etc
            {
                int pillarHeight = int.Parse(levelData[z][x].ToString());
                for (int y = 0; y <= pillarHeight; y++)
                {
                    Vector3Int levelCell = new Vector3Int(x, y, z);
                    Vector3Int gridSpaceCell = worldBuffer + levelCell;

                    grid[gridSpaceCell.x, gridSpaceCell.y, gridSpaceCell.z] = true;
                    var tile = Instantiate(tilePrefab, transform);
                    tile.transform.position = gridSpaceCell;
                    tile.name = $"Tile [{gridSpaceCell.x}, {gridSpaceCell.y}, {gridSpaceCell.z}]";
                }
            }
        }

        GridController.Instance.InitializeGrid(worldSize.x, worldSize.y, worldSize.z);
    }


    // Update is called once per frame
    void Update()
    {
    }
}