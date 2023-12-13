using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // Singleton instance
    public static PlayerInventory Instance { get; private set; }

    private Dictionary<string, int> resources = new Dictionary<string, int>();
    [SerializeField]
    public List<TextMeshProUGUI> resourceDisplay = new List<TextMeshProUGUI>();

    private void Start()
    {
        resources.Add("Gold", 0);
        resources.Add("Wood", 0);
        resources.Add("Stone", 0);
        LoadResources();
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddResource(string resourceName, int resourceCount)
    {
        Debug.Log(string.Format("{0} have been added with {1}", resourceName, resourceCount));
        if (resources.ContainsKey(resourceName))
        {
            resources[resourceName] += resourceCount;
        }
        else
        {
            resources[resourceName] = resourceCount;
        }
        UpdateResourceText(resourceName);
    }

    public void RemoveResource(string resourceName, int resourceCount)
    {
        if (resources.ContainsKey(resourceName))
        {
            resources[resourceName] -= resourceCount;
            // Prevents negative values
            if (resources[resourceName] < 0)
            {
                Debug.Log("Not enough resources. Setting resource count to 0.");
                resources[resourceName] = 0;
            }
        }
        else
        {
            Debug.LogWarning("Trying to remove a resource that doesn't exist in the inventory.");
        }
        UpdateResourceText(resourceName);
    }

    private void UpdateResourceText(string resourceName)
    {
        foreach (TextMeshProUGUI txt in resourceDisplay)
        {
            if (txt.gameObject.name == "TextCount" + resourceName)
            {
                txt.text = resources[resourceName].ToString("D5");
            }
        }
    }

    public void LoadResourcesToText()
    {
        foreach (var resource in resources)
        {
            UpdateResourceText(resource.Key);
        }
    }

    public bool IfEnoughResources(string resourceName, int cost)
    {
        if (resources[resourceName]>=cost)
        {
            return true;
        }
        return false;
    }

    private void SaveResources()
    {
        ResourceList resourceList = new ResourceList(resources);
        string json = JsonUtility.ToJson(resourceList);
        PlayerPrefs.SetString("Resources", json);
        PlayerPrefs.Save();
    }

    private void LoadResources()
    {
        if (PlayerPrefs.HasKey("Resources"))
        {
            string json = PlayerPrefs.GetString("Resources");
            ResourceList resourceList = JsonUtility.FromJson<ResourceList>(json);
            resources.Clear();

            foreach (ResourceData data in resourceList.resources)
            {
                resources[data.key] = data.value;
            }
        }
    }

    private void OnApplicationQuit()
    {
        SaveResources();
    }
}

[System.Serializable]
public class ResourceData
{
    public string key;
    public int value;

    public ResourceData(string key, int value)
    {
        this.key = key;
        this.value = value;
    }
}

// a construct to help serialize a dictionary into JSON
[System.Serializable]
public class ResourceList
{
    public List<ResourceData> resources;

    public ResourceList(Dictionary<string, int> dictionary)
    {
        resources = new List<ResourceData>();
        foreach (var item in dictionary)
        {
            resources.Add(new ResourceData(item.Key, item.Value));
        }
    }
}
