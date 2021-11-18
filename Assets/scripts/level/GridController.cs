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

    public Vector3Int GetRandomPosition()
    {
        int x = Random.Range(0, gridDimensions.x);
        int z = Random.Range(0, gridDimensions.z);
        int y = 0;

        while (CheckAccessible(x, y, z) == false)
        {
            if (y == gridDimensions.y) return GetRandomPosition();
            y++;
        }
    
        Vector3Int randomPosition = new Vector3Int(x, y, z);
        
        return randomPosition;
    }

    private bool CheckAccessible(int x, int y, int z)
    {   
        int BoolToInt(bool value)
        {
            return value ? 1 : 0;
        }
        
        Vector3Int tilePos = new Vector3Int(x, y, z) + Vector3Int.down;
        
        bool tile = GetGridCell(tilePos);

        bool accessNorth = GetGridCell(tilePos + Vector3Int.forward); // grid[x, y - 1, z + 1]
        bool accessSouth = GetGridCell(tilePos + Vector3Int.back); //grid[x, y - 1, z - 1];
        bool accessWest = GetGridCell(tilePos + Vector3Int.left); //grid[x - 1, y - 1, z];
        bool accessEast = GetGridCell(tilePos + Vector3Int.right); //grid[x + 1, y - 1, z + 1];

        int accessPoints =
            BoolToInt(accessNorth) +
            BoolToInt(accessSouth) +
            BoolToInt(accessWest) +
            BoolToInt(accessEast);

        return (tile && (accessPoints >= 2));
    }
    
    

    internal void InitializeGrid(bool[,,] gridData, int xSize, int ySize, int zSize)
    {
        gridDimensions = new Vector3Int(xSize, ySize, zSize);
        grid = gridData;
    }

    private bool GetGridCell(Vector3Int gridCellPos)
    {
        int x = Freya.Mathfs.Mod(gridCellPos.x, gridDimensions.x); // (gridCellPos.x % gridDimensions.x + gridDimensions.x + 1) % gridDimensions.x;
        int y = Freya.Mathfs.Mod(gridCellPos.y, gridDimensions.y); //(gridCellPos.y % gridDimensions.y + gridDimensions.y + 1) % gridDimensions.y;
        int z = Freya.Mathfs.Mod(gridCellPos.z, gridDimensions.z); //(gridCellPos.z % gridDimensions.z + gridDimensions.z + 1) % gridDimensions.z;
        
        return grid[x, y, z];
    }

    private bool InBounds(Vector3Int gridCell)
    {
        return (gridCell.x >= 0 && gridCell.x < gridDimensions.x) && 
               (gridCell.y >= 0 && gridCell.y < gridDimensions.y) && 
               (gridCell.z >= 0 && gridCell.z < gridDimensions.z);
    }
 
// #if UNITY_EDITOR
//     private void OnDrawGizmos()
//     {
//         Color prev = Gizmos.color;
//         Gizmos.color = Color.yellow;
//         for (int x = 0; x < gridDimensions.x; x++)
//         {
//             for (int y = 0; y < gridDimensions.y; y++)
//             {
//                 for (int z = 0; z < gridDimensions.z; z++)
//                 {
//                     if (grid[x, y, z]) Gizmos.DrawCube(new Vector3(x,y,z) * gridUnit, new Vector3(1, 1, 1));
//                 }
//             }
//         }
//
//         Gizmos.color = prev;
//     }
// #endif 
    
}
