using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimalList : MonoBehaviour
{
    // Prefabs of existing in game animals
    public List<Animal> existingAnimalList = new List<Animal>();
    // List for storing animals info
    public List<AnimalInfo> animalList = new List<AnimalInfo>();
    // Player animal list
    public List<AnimalInfo> playerAnimalList = new List<AnimalInfo>();

    private void Start()
    {
        foreach (var item in existingAnimalList)
        {
            animalList.Add(item.animalInfo);
        }
        LoadAnimalList();

        string animalToPlayer = PlayerPrefs.GetString("AnimalToPlayerList", null);
        AddAnimalToPlayerList(animalToPlayer);
        PlayerPrefs.DeleteKey("AnimalToPlayerList");

    }

    public void LoadAnimalList()
    {
        string json = PlayerPrefs.GetString("AnimalPlayerList");
        //Debug.Log("Loading Animal list");
        Debug.Log(json);
        if (!string.IsNullOrEmpty(json))
        {
            StringListWrapper loadedData = JsonUtility.FromJson<StringListWrapper>(json);
            foreach (var dataName in loadedData.list)
            {
                foreach (var existingAnimal in animalList)
                {
                    if (dataName == existingAnimal.dataName)
                    {
                        playerAnimalList.Add(existingAnimal);
                        break;
                    }
                }
            }
        }
    }

    public void SaveAnimalList()
    {
        string json;
        StringListWrapper listWrapper = new StringListWrapper();
        foreach (var item in playerAnimalList)
        {
            listWrapper.list.Add(item.dataName);
        }
        json = JsonUtility.ToJson(listWrapper);
        PlayerPrefs.SetString("AnimalPlayerList", json);
       // Debug.Log("Saving Animal list");
        Debug.Log(json);
        PlayerPrefs.Save();
    }

    public AnimalInfo FindAnimal(string dataName)
    {
        foreach (var item in animalList)
        {
            if (dataName == item.dataName)
            {
                return item.CloneAnimalInfo(item);
            }
        }
        return null;
    }

    public void RemoveAnimalFromPlayerList(string dataName)
    {
        for (int i = 0; i < playerAnimalList.Count; i++)
        {
            if (playerAnimalList[i].dataName == dataName)
            {
                playerAnimalList.RemoveAt(i);
                break;
            }
        }
        SaveAnimalList();
    }

    public GameObject PickAnimalToSpawn(string dataName)
    {
        foreach (var item in existingAnimalList)
        {
            if (dataName == item.animalInfo.dataName)
            {
                return item.gameObject;
            }
        }
        return null;
    }

    public void AddAnimalToPlayerList(string dataName)
    {
        foreach (var existingAnimal in animalList)
        {
            if (dataName == existingAnimal.dataName)
            {
                playerAnimalList.Add(existingAnimal);
                break;
            }
        }
        SaveAnimalList();
    }
}
[Serializable]
public class StringListWrapper
{
    public List<string> list = new List<string>();
}