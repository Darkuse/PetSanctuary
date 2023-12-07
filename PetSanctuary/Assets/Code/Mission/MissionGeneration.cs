using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class MissionGeneration : MonoBehaviour
{
    [SerializeField]
    private Transform missionContent;
    [SerializeField]
    private GameObject missionPrefab;
    [SerializeField]
    private List<GameObject> animals = new List<GameObject>();
    private MissionsContainer missionsContainer;

    void Start()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("MissionDescription");
        if (jsonFile != null)
        {
            string jsonContent = jsonFile.text;
            missionsContainer = JsonUtility.FromJson<MissionsContainer>(jsonContent);
        }
        else
        {
            Debug.LogError("JSON file not found");
        }

        for (int i = 0; i < 4; i++)
        {
            int random = Random.Range(0, animals.Count);
            PopulateWithMission(random);
        }
    }

    private void PopulateWithMission(int animalIndex)
    {
        GameObject pickedAnimal = animals[animalIndex];

        GameObject missionInstance = Instantiate(missionPrefab, missionContent);

        MissionInfo mission = missionsContainer.missionDescirption[Random.Range(0, missionsContainer.missionDescirption.Count)];

        missionInstance.AddComponent<MissionLifeSpan>().SetMissionInfo(mission.missionName,mission.missionType, pickedAnimal.GetComponent<AnimalInfo>().difficultyWeight, mission.description,pickedAnimal);
    }
}
