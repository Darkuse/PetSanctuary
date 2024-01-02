using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GridBuilding : MonoBehaviour
{
    [Header("Tilemaps")]
    public Tilemap builderGrid;
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
    private List<SerializableCageInfo> cageInfoList = new List<SerializableCageInfo>();
    private List<SerializableFarmInfo> farmInfoList = new List<SerializableFarmInfo>();

    private void Start()
    {
        LoadBuildingList();
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

    [Header("Warning")]
    public WarningPanel warningPanel;
    private ResourceCost resourceCost;
    private string buildType;
    public void ToBuildMenu(ResourceCost cost)
    {
        try
        {
            bool canAfford = true;
            canAfford &= PlayerInventory.Instance.IfEnoughResources("Wood", cost.woodCost);
            canAfford &= PlayerInventory.Instance.IfEnoughResources("Stone", cost.stoneCost);
            canAfford &= PlayerInventory.Instance.IfEnoughResources("Gold", cost.goldCost);
            width = cost.cageWidth;
            height = cost.cageHeight;

            if (canAfford)
                StartBuilding();
            else
                warningPanel.NotEnoughResource();
            resourceCost = cost;
            buildType = cost.buildType.ToString();
        }
        catch
        {
            warningPanel.NotEnoughResource();
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

        // Fore iteration
        ChangeStartEndIterationSize(0);

        if (buildType == "Cage")
        {
            // Builds Fence Tiles
            BuildFence(previuosCenter, fenceTile);


            // Adds Fence position and infomration in list
            SerializableCageInfo buildInf = new SerializableCageInfo(previuosCenter, width, height);
            cageInfoList.Add(buildInf);
            // Spawns Fence object with information about him
            SpawnObjectAtCenter(previuosCenter, buildInf);
        }
        else if(buildType == "Farm")
        {
            farmInfoList.Add(new SerializableFarmInfo(previuosCenter));
        }

        // Save Build Fence in Playerprefs
        SaveBuildingList();

        // Paints tile to red
        HighLight(previuosCenter, redTile);


        // Removes resources
        PlayerInventory.Instance.RemoveResource("Wood", resourceCost.woodCost);
        PlayerInventory.Instance.RemoveResource("Stone", resourceCost.stoneCost);
        PlayerInventory.Instance.RemoveResource("Gold", resourceCost.goldCost);

        player.transform.position = new Vector2(0, 0);

        // Temporary fix for bug
        SceneManager.LoadScene("Base");

        // Disables enables ui elements
        StartBuilding();
    }

    private void BuildFence(Vector3Int position, RuleTile tile)
    {
        Debug.LogWarning("Entered Fence Building");
        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                Vector3Int tilePosition = new Vector3Int(position.x + x, position.y + y, position.z);
                cageGrid.SetTile(tilePosition, tile);
            }
        }
    }

    // Spawns object, adds class CageInfo to object to store information about cage
    public GameObject cagePanel;
    public GameObject farmPanel;
    public void SpawnObjectAtCenter(Vector3 centerPosition, SerializableCageInfo buildInf)
    {
        // Creates new gameObject
        GameObject spawnedObject = new GameObject("CageInfo");
        // Sets to parent
        spawnedObject.transform.SetParent(buildingSpace, false);
        // If cage width are not even, center is adjusted
        if (buildInf.buildingWidth % 2 != 0) centerPosition += new Vector3(0.5f, 0, 0);
        // If cage height are not even, center is adjusted
        if (buildInf.buildingHeight % 2 != 0) centerPosition += new Vector3(0, 0.5f, 0);
        spawnedObject.transform.position = centerPosition;

        // Adds to object Collider and sets width and height, interact with player Trigger
        BoxCollider2D boxCollider = spawnedObject.AddComponent<BoxCollider2D>();
        boxCollider.size = new Vector3(buildInf.buildingWidth + 0.3f, buildInf.buildingHeight + 0.3f, 1);
        boxCollider.isTrigger = true;

        // Obstacle for player
        BoxCollider2D boxColliderBorder = spawnedObject.AddComponent<BoxCollider2D>();
        boxColliderBorder.size = new Vector3(buildInf.buildingWidth-0.6f, buildInf.buildingHeight - 0.5f, 1);
        // Sends info to new Object
        spawnedObject.AddComponent<CageInfo>().SetCagePanel(cagePanel, buildInf, this);
    }

    public void SpawnFarmObject(Vector3 centerPosition, SerializableFarmInfo buildInf)
    {
        GameObject spawnedObject = new GameObject("FarmInfo");

        spawnedObject.transform.SetParent(buildingSpace, false);

        centerPosition += new Vector3(0.5f, 0.5f, 0);
        spawnedObject.transform.position = centerPosition;

        // Adds to object Collider and sets width and height, interact with player
        BoxCollider2D boxCollider = spawnedObject.AddComponent<BoxCollider2D>();
        boxCollider.size = new Vector3(1 + 0.3f, 1 + 0.3f, 1);
        boxCollider.isTrigger = true;

        // Obstacle for player
        BoxCollider2D boxColliderBorder = spawnedObject.AddComponent<BoxCollider2D>();
        boxColliderBorder.size = new Vector3(1,1, 1);
        // Sends info to new Object
        spawnedObject.AddComponent<FarmInfo>().SetFarmPanel(farmPanel, buildInf, this);
    }

    private void SaveBuildingList()
    {
        SerializableBuildingsInformation serializableList = new SerializableBuildingsInformation();

        foreach (var build in cageInfoList)
        {
            serializableList.cageList.Add(build);
        }
        foreach (var build in farmInfoList)
        {
            serializableList.farmList.Add(build);
        }

        string json = JsonUtility.ToJson(serializableList);
        PlayerPrefs.SetString("BuildList", json);
        PlayerPrefs.Save();
#if UNITY_EDITOR
        File.WriteAllText(Application.dataPath + "/Buildings.txt", json);
#endif
    }

    public void LoadBuildingList()
    {
        string json = PlayerPrefs.GetString("BuildList", null);
        if (json != null)
        {
            SerializableBuildingsInformation serializableList = JsonUtility.FromJson<SerializableBuildingsInformation>(json);

            foreach (var cageBuild in serializableList.cageList)
            {
                width = cageBuild.buildingWidth;
                height = cageBuild.buildingHeight;
                ChangeStartEndIterationSize(0);
                HighLight(cageBuild.GetVector3Int(), redTile);
                BuildFence(cageBuild.GetVector3Int(), fenceTile);
                SpawnObjectAtCenter(cageBuild.GetVector3Int(), cageBuild);
                cageInfoList.Add(cageBuild);
            }
            foreach (var farmBuild in serializableList.farmList)
            {
                width = 1;
                height = 1;
                ChangeStartEndIterationSize(0);
                SpawnFarmObject(farmBuild.GetVector3Int(), farmBuild);
                HighLight(farmBuild.GetVector3Int(), redTile);
                farmInfoList.Add(farmBuild);
            }
        }
    }

    // Updates Cage builds information
    public void UpdateCageInfo(SerializableCageInfo cageInfoBuild)
    {
        for (int i = 0; i < cageInfoList.Count; i++)
        {
            if (cageInfoList[i].GetVector3Int() == cageInfoBuild.GetVector3Int())
            {
                cageInfoList[i] = cageInfoBuild;
                break;
            }
        }
        SaveBuildingList();
    }
    // Updates Farm builds information
    public void UpdateFarmInfo(SerializableFarmInfo farmInfoBuild)
    {
        for (int i = 0; i < farmInfoList.Count; i++)
        {
            if (farmInfoList[i].GetVector3Int() == farmInfoBuild.GetVector3Int())
            {
                farmInfoList[i] = farmInfoBuild;
                break;
            }
        }
        SaveBuildingList();
    }

    public void DeleteCage(Vector3Int currentbuildingPos,int width,int height)
    {
        this.width = width;
        this.height = height;
        ChangeStartEndIterationSize(0);
        for (int i = cageInfoList.Count - 1; i >= 0; i--)
        {
            if (cageInfoList[i].GetVector3Int() == currentbuildingPos)
            {
                cageInfoList.RemoveAt(i);
                break;
            }
        }
        BuildFence(currentbuildingPos, null);
        SaveBuildingList();
    }
    public void DeleteFarm(Vector3Int currentbuildingPos)
    {
        width = 1;
        height = 1;
        ChangeStartEndIterationSize(0);
        for (int i = farmInfoList.Count - 1; i >= 0; i--)
        {
            if (farmInfoList[i].GetVector3Int() == currentbuildingPos)
            {
                farmInfoList.RemoveAt(i);
                break;
            }
        }
        BuildFence(currentbuildingPos, null);
        SaveBuildingList();
    }
}

[Serializable]
public class SerializableCageInfo
{
    public int x, y, z, buildingWidth, buildingHeight;
    public int friendship;
    public string lastTimePetted;
    public int health;
    public int hunger;
    public string lastTimeFed;
    public string dataName;
    public string name;


    public SerializableCageInfo(Vector3Int vector, int width, int height)
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
public class SerializableFarmInfo
{
    public int x, y, z, foodHarvest, growTime;
    public string growStartTime;
    public bool growing;

    public SerializableFarmInfo(Vector3Int vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    public Vector3Int GetVector3Int()
    {
        return new Vector3Int(x, y, z);
    }
}

[Serializable]
public class SerializableBuildingsInformation
{
    public List<SerializableCageInfo> cageList = new List<SerializableCageInfo>();
    public List<SerializableFarmInfo> farmList = new List<SerializableFarmInfo>();
}

