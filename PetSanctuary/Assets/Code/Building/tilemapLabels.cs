using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class tilemapLabels : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject textMeshPrefab;

    void Start()
    {
        LabelTiles();
    }

    private void LabelTiles()
    {
        for (int n = tilemap.cellBounds.xMin; n < tilemap.cellBounds.xMax; n++)
        {
            for (int p = tilemap.cellBounds.yMin; p < tilemap.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = new Vector3Int(n, p, (int)tilemap.transform.position.z);
                Vector3 place = tilemap.CellToWorld(localPlace);
                if (tilemap.HasTile(localPlace))
                {
                    // Instantiate the TextMesh Prefab at the tile's position
                    GameObject textMeshObj = Instantiate(textMeshPrefab, place, Quaternion.identity, transform);
                    TextMeshPro textMesh = textMeshObj.GetComponent<TextMeshPro>();

                    // Set the text to the tile's local position
                    textMesh.text = localPlace.x + "," + localPlace.y;
                }
            }
        }
    }

}
