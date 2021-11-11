using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField]
    private MapTile tilePrefab;
    
    [SerializeField]
    private Vector2Int gridDimensions;
    
    private MapTile[,] grid;

    private float gridUnit;

    private static GridController instance;

    public static GridController Instance
    {
        get { return instance;
        }
    }

    public float GridUnit => gridUnit;

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

    private void InitializeGrid()
    {
        Vector3 origin = transform.position;
        
        // multiply by 0.5 instead of dividing by 2, according to Krister that is better.
        Vector3 startPos = new Vector3(origin.x - gridDimensions.x * 0.5f, origin.y, origin.z - gridDimensions.y * 0.5f);
        
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
