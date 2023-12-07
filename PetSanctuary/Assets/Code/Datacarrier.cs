using UnityEngine;

public class DataCarrier : MonoBehaviour
{
    public string missionName;
    public string missionDescription;
    public int missionCompleteTime;
    public GameObject missionAnimal;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetMissionData(string name, string description, int completeTime, GameObject animal)
    {
        missionName = name;
        missionDescription = description;
        missionCompleteTime = completeTime;
        missionAnimal = animal;
    }
}
