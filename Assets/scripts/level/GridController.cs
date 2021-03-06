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
    
    //private bool[,,] grid;
    private HashSet<Vector3Int> occupiedCells = new HashSet<Vector3Int>();
    public HashSet<Vector3Int> OccupiedCells => occupiedCells;

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

        while (IsFreeGridPos(x, y, z) == false)
        {
            if (y == gridDimensions.y) return GetRandomPosition();
            y++;
        }
    
        Vector3Int randomPosition = new Vector3Int(x, y, z);
        
        return randomPosition;
    }

    private bool IsFreeGridPos(int x, int y, int z)
    {
        
        Vector3Int potentialPos = WrapGridPos(new Vector3Int(x, y, z));
        Vector3Int underPos = potentialPos + Vector3Int.down;

        bool emptySpace = !occupiedCells.Contains(potentialPos);
        bool standingGround = occupiedCells.Contains(underPos);
        

        bool accessNorth = occupiedCells.Contains(WrapGridPos(underPos + Vector3Int.forward));
        bool accessSouth = occupiedCells.Contains(WrapGridPos(underPos + Vector3Int.back));
        bool accessWest = occupiedCells.Contains(WrapGridPos(underPos + Vector3Int.left));
        bool accessEast = occupiedCells.Contains(WrapGridPos(underPos + Vector3Int.right));
        
        int BoolToInt(bool value)
        {
            return value ? 1 : 0;
        }
        
        int amountOfAccess = 
            BoolToInt(accessNorth) +
            BoolToInt(accessSouth) +
            BoolToInt(accessWest) +
            BoolToInt(accessEast);

        return emptySpace && standingGround && (amountOfAccess >= 2);
        
    }

    internal void InitializeGrid(HashSet<Vector3Int> occupied, int xSize, int ySize, int zSize)
    {
        gridDimensions = new Vector3Int(xSize, ySize, zSize);
        occupiedCells = new HashSet<Vector3Int>();
        occupiedCells = occupied;
    }

    /// <summary>
    /// Wraps the given Vec3Int to a point inside the grid.
    /// </summary>
    /// <param name="gridCellPos"></param>
    /// <returns></returns>
    public Vector3Int WrapGridPos(Vector3Int gridCellPos)
    {
        int x = Freya.Mathfs.Mod(gridCellPos.x, gridDimensions.x); // (gridCellPos.x % gridDimensions.x + gridDimensions.x + 1) % gridDimensions.x;
        int y = Freya.Mathfs.Mod(gridCellPos.y, gridDimensions.y); //(gridCellPos.y % gridDimensions.y + gridDimensions.y + 1) % gridDimensions.y;
        int z = Freya.Mathfs.Mod(gridCellPos.z, gridDimensions.z); //(gridCellPos.z % gridDimensions.z + gridDimensions.z + 1) % gridDimensions.z;
        
        return new Vector3Int(x, y, z);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Color prev = Gizmos.color;
        Gizmos.color = Color.yellow;

        foreach (var tilePos in occupiedCells) 
        {
            Gizmos.DrawCube(tilePos * gridUnit, Vector3.one);
        }
        
        // for (int x = 0; x < gridDimensions.x; x++)
        // {
        //     for (int y = 0; y < gridDimensions.y; y++)
        //     {
        //         for (int z = 0; z < gridDimensions.z; z++)
        //         {
        //             if (grid[x, y, z]) Gizmos.DrawCube(new Vector3(x,y,z) * gridUnit, new Vector3(1, 1, 1));
        //         }
        //     }
        // }

        Gizmos.color = prev;
    }
#endif 
    
}
