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

    public static GridController Instance => instance;

    private void Awake()
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
        int x = Random.Range(0, gridDimensions.x * gridUnit);
        int z = Random.Range(0, gridDimensions.y * gridUnit);

        x = Mathf.CeilToInt(x / gridUnit) * gridUnit;
        z = Mathf.CeilToInt(z / gridUnit) * gridUnit;

        Vector3 randomPosition = new Vector3(x, yLevel, z);

        return randomPosition;
    }

    private void InitializeGrid()
    {
        Vector3 origin = transform.position; // + new Vector3(gridUnit/2f, 0, gridUnit/2f);

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
