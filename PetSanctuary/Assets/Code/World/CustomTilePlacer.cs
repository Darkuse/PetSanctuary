using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CustomTilePlacer : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile tile4; // The tile you want to have a 90% chance to appear
    public Tile[] otherTiles; // Array of the other tiles

    public int width = 100; // Width of the tilemap area to fill
    public int height = 100; // Height of the tilemap area to fill

    void Start()
    {
        GenerateTiles();
    }

    void GenerateTiles()
    {
        for (int x = -width; x < width; x++)
        {
            for (int y = -height; y < height; y++)
            {
                // Random value to determine if tile 4 should be placed
                if (Random.value < 0.9f) // 90% chance
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), tile4);
                }
                else // 10% chance to place other tiles
                {
                    int randomIndex = Random.Range(0, otherTiles.Length);
                    tilemap.SetTile(new Vector3Int(x, y, 0), otherTiles[randomIndex]);
                }
            }
        }
    }
}
