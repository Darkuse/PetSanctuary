using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GridBuilding : MonoBehaviour
{
    [Header("Tilemaps")]
    public Tilemap builderGrid;
    public Tilemap restartBuilderGrid;
    public Tilemap cageGrid;

    [Header("UI Elements")]
    public Camera camera;
    public GameObject player;
    public GameObject mainCanvas;
    public GameObject buildPanel;
    public GameObject buildCanvas;
    public Button buildButton;

    private bool building = false;

    [Header("Zone Tiles")]
    // Allowed building zone
    public Tile greenTile;
    // Building zone
    public Tile yellowTile;
    // Already build zone
    public Tile redTile;
    // World boundries
    public Tile grayTile;
    [Header("Fence Tiles")]
    public RuleTile fenceTile;

    [Header("Painting Size")]
    public int width;
    public int height;

    private int startX = -3;
    private int endX = 3;
    private int startY = -3;
    private int endY = 3;

    // Extra size so player can walk around cages
    private int extraSize = 1;



    private void Start()
    {
        LoadTilemap();
    }

    public void StartBuilding()
    {
        builderGrid.gameObject.SetActive(!builderGrid.gameObject.activeSelf);
        buildPanel.SetActive(!buildPanel.gameObject.activeSelf);
        camera.GetComponent<Camerafollow>().enabled = !camera.GetComponent<Camerafollow>().enabled;
        camera.GetComponent<DragCameraMoving>().enabled = !camera.GetComponent<DragCameraMoving>().enabled;
        player.SetActive(!player.activeSelf);
        mainCanvas.SetActive(!mainCanvas.activeSelf);
        buildCanvas.SetActive(!buildCanvas.activeSelf);
        building = !building;
        camera.transform.position = new Vector3(0, 0, -20);
        if (building)
        {
            startX = -(width / 2) - extraSize;
            endX = (width / 2) + extraSize;
            startY = -(height / 2) - extraSize;
            endY = (height / 2) + extraSize;

            if (width % 2 == 0)
            {
                endX -= 1;
            }
            if (height % 2 == 0)
            {
                endY -= 1;
            }
        }
        else
        {
            HighLight(previuosCenter, greenTile);
        }
    }

    Vector3Int previuosCenter = new Vector3Int(50, 50, 50);
    private void Update()
    {
        if (building)
        {
            Vector3 worldPoint = camera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, camera.nearClipPlane));
            Vector3Int currentCenter = builderGrid.WorldToCell(worldPoint);

            if (currentCenter != previuosCenter)
            {
                HighLight(previuosCenter, greenTile);
                HighLight(currentCenter, yellowTile);
                previuosCenter = currentCenter;
            }
        }
    }

    private bool canBuild = true;

    private void HighLight(Vector3Int current, Tile tile)
    {
        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                Vector3Int tilePosition = new Vector3Int(current.x + x, current.y + y, current.z);
                // || builderGrid.GetTile(tilePosition) == null
                if (builderGrid.GetTile(tilePosition) == redTile || builderGrid.GetTile(tilePosition) == grayTile)
                {
                    if (tile == yellowTile)
                    {
                        canBuild = false;
                    }
                }
                else
                {
                    builderGrid.SetTile(tilePosition, tile);
                }
            }
        }
        builderGrid.RefreshAllTiles();
        if (!canBuild && tile == yellowTile)
        {
            buildButton.interactable = false;
            canBuild = true;
        }
        else if (tile == yellowTile)
        {
            buildButton.interactable = true;
        }
    }

    public void Build()
    {
        StartBuilding();

        startX += extraSize;
        endX -= extraSize;
        startY += extraSize;
        endY -= extraSize;


        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                Vector3Int tilePosition = new Vector3Int(previuosCenter.x + x, previuosCenter.y + y, previuosCenter.z);
                cageGrid.SetTile(tilePosition, fenceTile);

            }
        }

        HighLight(previuosCenter, redTile);
        SaveTilemap();
    }

    public void SaveTilemap()
    {
        List<TileData> tileDataList = new List<TileData>();
        BoundsInt bounds = builderGrid.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                TileBase tile = builderGrid.GetTile(new Vector3Int(x, y, 0));
                if (tile != null && tile == redTile)
                {
                    tileDataList.Add(new TileData(x, y, tile.name));
                }
            }
        }

        string json = JsonUtility.ToJson(new Serialization<TileData>(tileDataList));
        PlayerPrefs.SetString("SavedTilemap", json);
        PlayerPrefs.Save();

        File.WriteAllText(Application.dataPath + "/TileMapBuild.json", json);

    }

    public void NewTileMap()
    {
        builderGrid = restartBuilderGrid;
    }


    public void LoadTilemap()
    {
        if (PlayerPrefs.HasKey("SavedTilemap"))
        {
            string json = PlayerPrefs.GetString("SavedTilemap");
            Serialization<TileData> data = JsonUtility.FromJson<Serialization<TileData>>(json);
            List<TileData> tileDataList = data.ToList();

            foreach (TileData tileData in tileDataList)
            {
                Vector3Int tilePosition = new Vector3Int(tileData.x, tileData.y, 0);
                TileBase tileToSet = null;

                switch (tileData.tileType)
                {
                    case "GreenTiles_0":
                        tileToSet = greenTile;
                        break;
                    case "YellowTiles_0":
                        tileToSet = yellowTile;
                        break;
                    case "RedTiles_0":
                        tileToSet = redTile;
                        break;
                }

                if (tileToSet == redTile)
                {
                    cageGrid.SetTile(tilePosition, fenceTile);
                }

                builderGrid.SetTile(tilePosition, tileToSet);
            }
        }
    }
}

[System.Serializable]
public class TileData
{
    public int x, y;
    public string tileType;

    public TileData(int x, int y, string tileType)
    {
        this.x = x;
        this.y = y;
        this.tileType = tileType;
    }
}

[System.Serializable]
public class Serialization<T>
{
    [SerializeField] private List<T> items;

    public Serialization(List<T> items)
    {
        this.items = items;
    }

    public List<T> ToList()
    {
        return items;
    }
}