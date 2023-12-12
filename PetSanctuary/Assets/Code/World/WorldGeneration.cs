using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGeneration : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile tile;

    public int width = 100;
    public int height = 100;

    void Start()
    {
        GenerateTileMap();
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
}