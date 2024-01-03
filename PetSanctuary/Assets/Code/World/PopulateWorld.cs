using System.Collections.Generic;
using UnityEngine;

public class PopulateWorld : MonoBehaviour
{
    public int worldWidth = 10;
    public int worldHeight = 10;
    public int seed;

    [SerializeField]
    private List<GameObject> trees = new List<GameObject>();
    [SerializeField]
    private List<GameObject> stones = new List<GameObject>();

    public List<GameObject> animalListSpawn;
    private DataCarrier data;

    public GameObject treeSpawn;
    public GameObject stoneSpawn;
    [Range(10, 500)]
    public int treeCount;
    [Range(10, 500)]
    public int stoneCount;


    private HashSet<Vector2Int> occupiedPositions = new HashSet<Vector2Int>();

    void Start()
    {
        InitializeSeed();
        PopulateTrees();
        PopulateStones();
        SpawnMissionAnimal();
    }

    void InitializeSeed()
    {
        if (PlayerPrefs.GetString("RandomSeed") == "true")
        {
            seed = (int)System.DateTime.Now.Ticks;
        }
        Random.InitState(seed);
    }

    void PopulateTrees()
    {
        PopulateResource(treeCount, trees, treeSpawn.transform);
    }

    void PopulateStones()
    {
        PopulateResource(stoneCount, stones, stoneSpawn.transform);
    }

    void PopulateResource(int count, List<GameObject> resourceList, Transform parent)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2Int position2D;
            do
            {
                position2D = new Vector2Int(Random.Range(-worldWidth + 1, worldWidth - 1), Random.Range(-worldHeight + 1, worldHeight - 1));
            } while (occupiedPositions.Contains(position2D));

            occupiedPositions.Add(position2D);
            Vector3 position = new Vector3(position2D.x, position2D.y, 0); // Convert to Vector3
            GameObject resource = Instantiate(resourceList[Random.Range(0, resourceList.Count)], position, Quaternion.identity);
            resource.transform.SetParent(parent);
        }
    }


    void SpawnMissionAnimal()
    {
        data = GameObject.Find("DontDestroyOnLoad").GetComponent<DataCarrier>();
        Vector2Int position2D;
        do
        {
            position2D = new Vector2Int(Random.Range(-worldWidth, worldWidth), Random.Range(-worldHeight, worldHeight));
        } while (occupiedPositions.Contains(position2D));

        Vector3 position = new Vector3(position2D.x, position2D.y, 0); // Convert to Vector3

        foreach (var item in animalListSpawn)
        {
            if (item.GetComponent<AnimalExploration>().dataName == data.missionAnimalDataName)
            {
                Instantiate(item, position, Quaternion.identity);
                break;
            }
        }

    }
}