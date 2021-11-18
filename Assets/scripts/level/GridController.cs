using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridController : MonoBehaviour
{
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
        
        x = Mathf.CeilToInt(x / gridUnit) * gridUnit;
        z = Mathf.CeilToInt(z / gridUnit) * gridUnit;
    
        Vector3 randomPosition = new Vector3(x, yLevel, z);
    
        
        return randomPosition;
    }

    internal void InitializeGrid(int xSize, int ySize, int zSize)
    {
        gridDimensions = new Vector3Int(xSize, ySize, zSize);
        
        grid = new bool[gridDimensions.x, gridDimensions.y, gridDimensions.z];
    }

    private bool InBounds(Vector3Int gridCell)
    {
        return (gridCell.x >= 0 && gridCell.x < gridDimensions.x) && 
               (gridCell.y >= 0 && gridCell.y < gridDimensions.y) && 
               (gridCell.z >= 0 && gridCell.z < gridDimensions.z);
    }
}
