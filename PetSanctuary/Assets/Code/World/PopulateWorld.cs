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
    [Range(10, 100)]
    public int treeCount;
    [Range(10, 100)]
    public int stoneCount;


    void Start()
    {
        AddResources();
    }

    void AddResources()
    {
        if (PlayerPrefs.GetString("RandomSeed") == "true")
        {
            seed = (int)System.DateTime.Now.Ticks;
        }
        Random.InitState(seed);

        // Populate Trees
        for (int i = 0; i < treeCount; i++)
        {
            Vector2 position = new Vector2Int(Random.Range(-worldWidth + 1, worldWidth - 1), Random.Range(-worldHeight + 1, worldHeight - 1));
            Instantiate(trees[Random.Range(0, trees.Count)], position, Quaternion.identity).transform.SetParent(treeSpawn.transform);

        }

        // Populate Stones
        for (int i = 0; i < stoneCount; i++)
        {
            Vector2 position = new Vector2Int(Random.Range(-worldWidth + 1, worldWidth - 1), Random.Range(-worldHeight + 1, worldHeight - 1));
            Instantiate(stones[Random.Range(0, stones.Count)], position, Quaternion.identity).transform.SetParent(stoneSpawn.transform);
        }

        // Spawns Animal to the world
        data = GameObject.Find("DontDestroyOnLoad").GetComponent<DataCarrier>();
        Vector2 positiona = new Vector2Int(Random.Range(-worldWidth, worldWidth), Random.Range(-worldHeight, worldHeight));
        foreach (var item in animalListSpawn)
        {
            if (item.GetComponent<AnimalExploration>().dataName == data.missionAnimalDataName)
            {
                Instantiate(item, positiona, Quaternion.identity);
                break;
            }
        }
    }
}