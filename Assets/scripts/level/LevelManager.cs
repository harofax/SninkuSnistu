using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField,
     Tooltip("This denotes how much empty space should exist outside the level boundaries in each dimension")]
    private Vector3Int worldBuffer = new Vector3Int(10, 30, 10);

    [SerializeField]
    private GameObject tilePrefab;

    private List<GameObject> spawnedTiles = new List<GameObject>(1000);

    private const string SNAKE_LEVEL_FOLDER = "levels";
    private const string SNAKE_LEVEL_FILENAME = "snake_level_";
    private const string SNAKE_LEVEL_FILETYPE = ".txt";

    private int levelIndex = 0;
    private int numOfLevels;
    private string levelFilePath;
    
    private const int WinSceneIndex = 3;

    private static LevelManager instance;
    private int tileSize;

    public static LevelManager Instance => instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        tileSize = GridController.Instance.GridUnit;
        tilePrefab.transform.localScale = new Vector3(tileSize, tileSize, tileSize);

        levelFilePath = Path.Combine(Application.streamingAssetsPath, SNAKE_LEVEL_FOLDER);
        numOfLevels = LevelLoader.CountLevels(levelFilePath, SNAKE_LEVEL_FILENAME, SNAKE_LEVEL_FILETYPE);
    }

    public void ClearLevel()
    {
        foreach (var spawnedTile in spawnedTiles)
        {
            Destroy(spawnedTile);
        }
    }

    private void Start()
    {
    }

    public void LoadLevel(int i)
    {
        levelIndex = i;
        if (i >= numOfLevels)
        {
            SceneManager.LoadScene(WinSceneIndex);
            return;
        }

        string levelFile = SNAKE_LEVEL_FILENAME + levelIndex + SNAKE_LEVEL_FILETYPE;

        var levelData = LevelLoader.ParseLevelData(levelFile, levelFilePath);
        
        ConstructLevel(levelData.Data, levelData.BoundingBox);
    }

    

    public void LoadNextLevel()
    {
        levelIndex++;
        LoadLevel(levelIndex);
    }

    private void ConstructLevel(char[][] levelData, Vector3Int boundingBox)
    {
        Vector3 origin = transform.position;

        Vector3Int worldSize = boundingBox + (worldBuffer * 2);

        //bool[,,] grid = new bool[worldSize.x, worldSize.y, worldSize.z];
        HashSet<Vector3Int> occupiedTiles = new HashSet<Vector3Int>();

        for (int z = 0; z < levelData.GetLength(0); z++) // used to be 0 -> worldsize
        {
            for (int x = 0; x < levelData[z].Length; x++) // etc
            {
                int pillarHeight = int.Parse(levelData[z][x].ToString());
                for (int y = 0; y < pillarHeight; y++)
                {
                    Vector3Int levelCell = new Vector3Int(x, y, z);
                    Vector3Int gridSpaceCell = worldBuffer + levelCell;

                    //grid[gridSpaceCell.x, gridSpaceCell.y, gridSpaceCell.z] = true;
                    occupiedTiles.Add(gridSpaceCell);
                    var tile = Instantiate(tilePrefab, transform);
                    tile.transform.position = origin + gridSpaceCell * tileSize;
                    tile.name = $"Tile [{gridSpaceCell.x}, {gridSpaceCell.y}, {gridSpaceCell.z}]";
                    spawnedTiles.Add(tile);
                }
            }
        }

        GridController.Instance.InitializeGrid(occupiedTiles, worldSize.x, worldSize.y, worldSize.z);
    }
}