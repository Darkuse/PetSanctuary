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
    private DataCarrier data;

    public GameObject treeSpawn;
    public GameObject stoneSpawn;


    void Start()
    {
        AddResources();
    }

    void AddResources()
    {
        Random.InitState(seed);

        // Populate Trees
        int treeCount = Random.Range(10, 20);
        for (int i = 0; i < treeCount; i++)
        {
            Vector2 position = new Vector2(Random.Range(-worldWidth, worldWidth), Random.Range(-worldHeight, worldHeight));
            Instantiate(trees[Random.Range(0, trees.Count)], position, Quaternion.identity).transform.SetParent(treeSpawn.transform);

        }

        // Populate Stones
        int stoneCount = Random.Range(5, 15);
        for (int i = 0; i < stoneCount; i++)
        {
            Vector2 position = new Vector2(Random.Range(-worldWidth, worldWidth), Random.Range(-worldHeight, worldHeight));
            Instantiate(stones[Random.Range(0, stones.Count)], position, Quaternion.identity).transform.SetParent(stoneSpawn.transform);
        }
        data = GameObject.Find("DontDestroyOnLoad").GetComponent<DataCarrier>();
        Vector2 positiona = new Vector2(Random.Range(-worldWidth, worldWidth), Random.Range(-worldHeight, worldHeight));
        Instantiate(data.missionAnimal, positiona, Quaternion.identity);
    }
}