using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridController : MonoBehaviour
{
    [SerializeField]
    private GameObject worldPrefab;

    [SerializeField]
    private Vector3Int gridPreview;

    [SerializeField, Range(1, 6)]
    private int gridUnit = 2;
    
    private Vector3Int gridDimensions;
    public Vector3Int GridDimensions => gridDimensions;
    
    private bool[,,] grid;
    
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
    }
    
    public Vector3Int ConvertToGridPos(Vector3 pos)
    {
        // int x = Mathf.CeilToInt(pos.x / gridUnit) * gridUnit;
        // int y = Mathf.CeilToInt(pos.y / gridUnit) * gridUnit;
        // int z = Mathf.CeilToInt(pos.z / gridUnit) * gridUnit;
        //
        // return new Vector3Int(x, y, z);
        return Vector3Int.RoundToInt(pos / gridUnit);
    }

    public Vector3 ConvertToWorldSpace(Vector3Int gridPos)
    {
        return gridPos * gridUnit;
    }

    public Vector3 GetRandomPosition(float yLevel)
    {
        int x = Random.Range(0, gridDimensions.x) * gridUnit;
        int z = Random.Range(0, gridDimensions.z) * gridUnit;
    
        Vector3 randomPosition = new Vector3(x, yLevel, z);
    
        return randomPosition;
    }

    internal void InitializeGrid(int xSize, int ySize, int zSize)
    {
        gridDimensions = new Vector3Int(xSize, ySize, zSize);
        
        grid = new bool[gridDimensions.x, gridDimensions.y, gridDimensions.z];

        // for (int x = 0; x < gridDimensions.x; x++)
        // {
        //     for (int y = 0; y < gridDimensions.y; y++)
        //     {
        //         for (int z = 0; z < gridDimensions.z; z++)
        //         {
        //             grid[x, y, z] = false;
        //         }
        //     }
        // }
    }

    private bool InBounds(Vector3Int gridCell)
    {
        return (gridCell.x >= 0 && gridCell.x < gridDimensions.x) && 
               (gridCell.y >= 0 && gridCell.y < gridDimensions.y) && 
               (gridCell.z >= 0 && gridCell.z < gridDimensions.z);
    }
}
