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

    // Start is called before the first frame update
    void Awake()
    {
        // multiply by 0.5 instead of dividing by 2, according to Krister that is better.
        Vector3 startPos = new Vector3(-gridDimensions.x * 0.5f, 0, -gridDimensions.y * 0.5f);
        
        grid = new MapTile[gridDimensions.x, gridDimensions.y];

        // Will always be a whole number, and will not change.
        // Could make it a const field but this is more clear and less magic-number:y
        int tileSize = (int) tilePrefab.transform.localScale.x;
        
        for (int y = 0; y < gridDimensions.y; y++)
        {
            for (int x = 0; x < gridDimensions.x; x++)
            {
                MapTile newTile = Instantiate(tilePrefab, transform);
                Vector3 offset = new Vector3(x * tileSize, 0, y * tileSize);
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
