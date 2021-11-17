using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField, Tooltip("This denotes how much empty space should exist outside the level boundaries in each dimension")]
    private Vector3Int worldBuffer = new Vector3Int(10, 30, 10);

    [SerializeField]
    private GameObject tilePrefab;
    
    private const string SNAKE_LEVEL_FILENAME = "/levels/snake_level_";
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

    public void LoadLevel(int i)
    {
        string levelFile = SNAKE_LEVEL_FILENAME + levelIndex + SNAKE_LEVEL_FILETYPE;
         
        var levelData = LevelLoader.TextFileToCharArray(levelFile, levelFilePath);
        
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

        for (int x = 0; x < worldSize.x; x++)
        {
            for (int z = 0; z < worldSize.z; z++)
            {
                for (int y = 0; y < worldSize.y; y++)
                {
                    Vector3Int currentCell = new Vector3Int(x, y, z);
                    Vector3Int validPos = currentCell - boundingBox;
                    // Check if point is outside the bounding box, if so it should be empty (false)
                    if (validPos.x < 0 || validPos.x > boundingBox.x ||
                        validPos.y < 0 || validPos.y > boundingBox.y ||
                        validPos.z < 0 || validPos.z > boundingBox.z)
                    {
                        grid[x, y, z] = false;
                    }
                    else
                    {
                        // We already checked for invalid characters when parsing the file, so
                        // it is safe to use Parse here without catching an exception.
                        int pillarHeight = int.Parse(levelData[x][z].ToString());
                        
                        if (y - pillarHeight < 0)
                        {
                            grid[x, y, z] = true;
                        }
                    }
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
