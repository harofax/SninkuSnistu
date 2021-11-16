using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridController : MonoBehaviour
{
    [SerializeField]
    private MapTile tilePrefab;
    
    [SerializeField]
    private Vector2Int gridDimensions;

    [SerializeField, Range(0, 10)]
    private int outerRingRadius;

    [SerializeField, Range(-5, 5)]
    private int outerRingHeight;
    
    [SerializeField, Range(1, 6)]
    private int gridUnit = 2;
    
    public Vector2Int GridDimensions => gridDimensions;
    
    private MapTile[,] grid;
    
    private readonly HashSet<Vector3Int> tilePositions = new HashSet<Vector3Int>();

    public HashSet<Vector3Int> TilePositions => tilePositions;

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

        if (outerRingRadius > gridDimensions.x || outerRingRadius > gridDimensions.y)
        {
            throw new ArgumentOutOfRangeException(paramName: "outerRingRadius", "Radius cannot be larger than grid dimensions");
        }

        tilePrefab.transform.localScale = new Vector3(gridUnit, gridUnit, gridUnit);
        InitializeGrid();
    }
    
    public Vector3Int ConvertToGridPos(Vector3 pos)
    {
        return Vector3Int.RoundToInt(pos / gridUnit);
    }

    public Vector3 GetRandomPosition(float yLevel)
    {
        int x = Random.Range(outerRingRadius, (gridDimensions.x - outerRingRadius)) * gridUnit;
        int z = Random.Range(outerRingRadius, (gridDimensions.y - outerRingRadius)) * gridUnit;

        x = Mathf.CeilToInt(x / gridUnit) * gridUnit;
        z = Mathf.CeilToInt(z / gridUnit) * gridUnit;

        Vector3 randomPosition = new Vector3(x, yLevel, z);

        return randomPosition;
    }

    public MapTile GetTile(int x, int y)
    {
        if (!InBounds(x, y))
        {
            throw new ArgumentOutOfRangeException();
        }

        return grid[x, y];
    }

    private void InitializeGrid()
    {
        grid = new MapTile[gridDimensions.x, gridDimensions.y];
        
        for (int z = 0; z < gridDimensions.y; z++)
        {
            for (int x = 0; x < gridDimensions.x; x++)
            {
                int yOffset = z < outerRingRadius || 
                              z > gridDimensions.y - outerRingRadius ||
                              x < outerRingRadius || 
                              x > gridDimensions.x - outerRingRadius 
                    ? outerRingHeight 
                    : 0;
                
                MapTile newTile = Instantiate(tilePrefab, transform);
                
                Vector3 offset = new Vector3(x, yOffset, z) * gridUnit;

                newTile.transform.position = offset;
                newTile.name = $"Tile [{x}, {z}]";
                
                grid[x, z] = newTile;
                tilePositions.Add(new Vector3Int(x, yOffset, z));
            }
        }
    }

    private bool InBounds(int x, int y)
    {
        return (x >= 0 && x < gridDimensions.x) && (y >= 0 && y < gridDimensions.y);
    }
}
