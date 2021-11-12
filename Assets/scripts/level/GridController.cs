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

    private int gridUnit = 2;
    public int GridUnit => gridUnit;

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

        //Vector3 tileSize = tilePrefab.transform.localScale;
        tilePrefab.transform.localScale = new Vector3(gridUnit, gridUnit, gridUnit);
        
        InitializeGrid();
    }

    public Vector3 GetRandomPosition(float yLevel)
    {
        Vector2Int gridSpan = GridDimensions / 2;

        // int x = Random.Range(-gridSpan.x, gridSpan.x + 1);
        // int z = Random.Range(-gridSpan.y, gridSpan.y + 1);

        int x = Random.Range(0, gridDimensions.x);
        int z = Random.Range(0, gridDimensions.y);

        x = Mathf.CeilToInt(x / gridUnit) * gridUnit;
        z = Mathf.CeilToInt(z / gridUnit) * gridUnit;

        Vector3 randomPosition = new Vector3(x, yLevel, z);

        return randomPosition;
    }

    private void InitializeGrid()
    {
        Vector3 origin = transform.position; // + new Vector3(gridUnit/2f, 0, gridUnit/2f);

        Vector2Int gridSpan = gridDimensions / 2;
        
        Vector3 startPos = new Vector3(origin.x - gridSpan.x, origin.y, origin.z - gridSpan.y);

        grid = new MapTile[gridDimensions.x, gridDimensions.y];

        for (int y = 0; y < gridDimensions.y; y++)
        {
            for (int x = 0; x < gridDimensions.x; x++)
            {
                MapTile newTile = Instantiate(tilePrefab, transform);
                
                Vector3 offset = new Vector3(x * gridUnit, 0, y * gridUnit);

                newTile.transform.position = offset;
                newTile.name = $"Tile [{origin.x + offset.x}, {origin.z + offset.z}]";
                
                grid[x, y] = newTile;
            }
        }
    }
}
