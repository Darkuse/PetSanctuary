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
}
