using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGeneration : MonoBehaviour
{
    [Header("Grass - player walking zone")]
    public Tilemap tilemap;
    public RuleTile tile;

    public int width = 10;
    public int height = 10;

    [Header("Forest - non-walkable zone")]
    public Tilemap tilemapForest;
    public RuleTile forestTile;

    void Start()
    {
        GenerateTileMap();
        GenerateForestBorder();
    }

    void GenerateTileMap()
    {
        for (int x = -width; x < width; x++)
        {
            for (int y = -height; y < height; y++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }

    void GenerateForestBorder()
    {
        int borderWidth = 6; // Width of the border in tiles

        // Draw the border around the first tilemap
        for (int x = -(width/2 + borderWidth); x < width/2 + borderWidth; x++)
        {
            for (int y = -(height/2 + borderWidth); y < height/2 + borderWidth; y++)
            {
                // Check if the position is outside the bounds of the grass tilemap
                if (x < -width/2+1 || x >= width/2-1 || y < -height/2+1 || y >= height/2-1)
                {
                    tilemapForest.SetTile(new Vector3Int(x, y, 0), forestTile);
                }
            }
        }
    }
}