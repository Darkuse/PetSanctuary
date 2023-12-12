using System;
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


    [Header("Buildings")]
    public Transform buildingSpace;
    private List<SerializableBuildInfo> buildingCenterPosition = new List<SerializableBuildInfo>();



    private void Start()
    {
        buildingCenterPosition = LoadBuildingList();
        foreach (var item in buildingCenterPosition)
        {
            width = item.buildingWidth;
            height = item.buildingHeight;
            ChangeStartEndIterationSize(0);
            HighLight(item.GetVector3Int(), redTile);
            BuildFence(item.GetVector3Int());
            SpawnObjectAtCenter(item.GetVector3Int(), width, height);
        }
    }

    private void ChangeStartEndIterationSize(int extraSize)
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
            ChangeStartEndIterationSize(1);
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

        ChangeStartEndIterationSize(0);

        BuildFence(previuosCenter);

        buildingCenterPosition.Add(new SerializableBuildInfo(previuosCenter, width, height));

        SaveBuildingList();

        HighLight(previuosCenter, redTile);

        SpawnObjectAtCenter(previuosCenter, width, height);

    }

    private void BuildFence(Vector3Int position)
    {
        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                Vector3Int tilePosition = new Vector3Int(position.x + x, position.y + y, position.z);
                cageGrid.SetTile(tilePosition, fenceTile);
            }
        }
    }

    public void SpawnObjectAtCenter(Vector3 centerPosition, int width, int height)
    {
        // Creates new gameObject
        GameObject spawnedObject = new GameObject("CageInfo");
        // Sets to parent
        spawnedObject.transform.SetParent(buildingSpace, false);
        // If cage width are not even, center is adjusted
        if (width % 2 != 0) centerPosition += new Vector3(0.5f, 0, 0);
        // If cage height are not even, center is adjusted
        if (height % 2 != 0) centerPosition += new Vector3(0, 0.5f, 0);
        spawnedObject.transform.position = centerPosition;

        // Ads to object Collider and sets width and height
        BoxCollider boxCollider = spawnedObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(width, height, 1); 
    }

    private void SaveBuildingList()
    {
        SerializableVector3IntList serializableList = new SerializableVector3IntList();

        foreach (var build in buildingCenterPosition)
        {
            serializableList.list.Add(build);
        }

        string json = JsonUtility.ToJson(serializableList);
        PlayerPrefs.SetString("BuildList",json);
        //File.WriteAllText(Application.dataPath + "/BuildingList.txt", json);
    }

    public List<SerializableBuildInfo> LoadBuildingList()
    {
        //string path = Application.dataPath + "/BuildingList.txt";

        string json = PlayerPrefs.GetString("BuildList");
        if (json!=null)
        {
            SerializableVector3IntList serializableList = JsonUtility.FromJson<SerializableVector3IntList>(json);

            List<SerializableBuildInfo> buildingInfo = new List<SerializableBuildInfo>();
            foreach (var serializableVector in serializableList.list)
            {
                Vector3Int vector = new Vector3Int(serializableVector.x, serializableVector.y, serializableVector.z);
                SerializableBuildInfo buildInfo = new SerializableBuildInfo(vector, serializableVector.buildingWidth, serializableVector.buildingHeight);
                buildingInfo.Add(buildInfo);
            }

            return buildingInfo;
        }
        return new List<SerializableBuildInfo>();
    }

    public void NewTileMap()
    {
        BoundsInt bounds = builderGrid.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                TileBase tile = builderGrid.GetTile(new Vector3Int(x, y, 0));
                if (tile == redTile)
                {
                    builderGrid.SetTile(new Vector3Int(x, y), greenTile);
                }
            }
        }
        File.WriteAllText(Application.dataPath + "/BuildingList.txt", null);
    }
}

[Serializable]
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

[Serializable]
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

[Serializable]
public class SerializableBuildInfo
{
    public int x, y, z, buildingWidth, buildingHeight;
    AnimalInfo animal;


    public SerializableBuildInfo(Vector3Int vector, int width, int height)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
        buildingWidth = width;
        buildingHeight = height;
    }

    public Vector3Int GetVector3Int()
    {
        return new Vector3Int(x, y, z);
    }
}

[Serializable]
public class SerializableVector3IntList
{
    public List<SerializableBuildInfo> list = new List<SerializableBuildInfo>();
}

