using UnityEngine;

public class DataCarrier : MonoBehaviour
{
    public string missionName;
    public string missionDescription;
    public int missionCompleteTime;
    public string missionAnimalDataName;
    public int missionReward;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetMissionData(string name, string description, int completeTime, string animal,int reward)
    {
        missionName = name;
        missionDescription = description;
        missionCompleteTime = completeTime;
        missionAnimalDataName = animal;
        missionReward = reward;
    }
}
