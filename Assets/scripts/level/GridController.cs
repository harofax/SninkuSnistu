using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField]
    private MapTile tilePrefab;
    
    [SerializeField]
    private Vector2Int gridDimensions;
    public Vector2Int GridDimensions => gridDimensions;
    
    private MapTile[,] grid;

    private float gridUnit;
    public float GridUnit => gridUnit;

    private static GridController instance;

    public static GridController Instance
    {
        get { return instance;
        }
    }

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
        
        gridUnit = tilePrefab.transform.localScale.x;
        InitializeGrid();
    }

    public Vector3 GetRandomPosition(float yLevel)
    {
        Vector2Int gridSpan = GridDimensions / 2;

        int x = Random.Range(-gridSpan.x, gridSpan.x);
        int z = Random.Range(-gridSpan.y, gridSpan.y);

        // print("x: " + x + ", z: " + z);
        
        Vector3 randomPosition = new Vector3(x, yLevel, z);

        return randomPosition;
    }

    private void InitializeGrid()
    {
        Vector3 origin = transform.position + new Vector3(gridUnit/2f, 0, gridUnit/2f);

        Vector2Int gridSpan = gridDimensions / 2;
        
        // multiply by 0.5 instead of dividing by 2, according to Krister that is better.
        Vector3 startPos = new Vector3(origin.x - gridSpan.x, origin.y, origin.z - gridSpan.y);
        
        grid = new MapTile[gridDimensions.x, gridDimensions.y];

        for (int y = 0; y < gridDimensions.y; y++)
        {
            for (int x = 0; x < gridDimensions.x; x++)
            {
                MapTile newTile = Instantiate(tilePrefab, transform);
                Vector3 offset = new Vector3(x * gridUnit, 0, y * gridUnit);
                newTile.transform.position = startPos + offset;
                
                newTile.name = $"Tile {x}, {y}";
                
                grid[x, y] = newTile;
            }
        }
    }
}
