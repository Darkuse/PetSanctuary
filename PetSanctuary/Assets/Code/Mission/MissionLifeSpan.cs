using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionLifeSpan : MonoBehaviour
{
    private string missionName;
    private string missionType;
    private float missionDifficulty;
    private string missionDescription;
    private int missionCompleteTime;
    private float missionLifeSpan;
    private int missionReward;
    private GameObject missionAnimal;

    public TextMeshProUGUI missionNameText;
    public TextMeshProUGUI missionTimeLeftText;
    public TextMeshProUGUI missionDifficultyText;
    public Image animalAvatar;


    public void SetMissionInfo(string name, string type, float difficulty, string description, GameObject animal)
    {
        missionName = name;
        missionType = type;
        missionDifficulty = difficulty;
        missionDescription = description;
        missionAnimal = animal;
    }

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(PickedMission);
        animalAvatar.sprite = Resources.Load<Sprite>("Animals/" + missionAnimal.GetComponent<Animal>().animalInfo.dataName + "Head");
        // Mission Name
        missionNameText.text = string.Format(missionName, missionAnimal.GetComponent<Animal>().animalInfo.name);
        // Mission LifeSpan
        missionLifeSpan = (int)(Random.Range(1, 6) / missionDifficulty);
        missionTimeLeftText.text = string.Format("Time Left: {0}m", missionLifeSpan);
        // Mission Difficulty

        string difficultyCategory;

        if (missionDifficulty <= 0.3)
        {
            difficultyCategory = "Easy";
        }
        else if (missionDifficulty <= 0.6)
        {
            difficultyCategory = "Medium";
        }
        else
        {
            difficultyCategory = "Hard";
        }

        missionDifficultyText.text = string.Format("Difficulty: {0}", difficultyCategory);

        StartCoroutine(TimerCoroutine());
    }

    private IEnumerator TimerCoroutine()
    {
        while (missionLifeSpan > 0)
        {
            yield return new WaitForSeconds(60);
            missionLifeSpan--;
            missionTimeLeftText.text = string.Format("Time Left: {0}m", missionLifeSpan);
        }
        Debug.Log("Timer finished!");
        Destroy(gameObject);
    }


    [SerializeField]
    DataCarrier data;
    public void PickedMission()
    {
        GameObject missionDescriptionPanel = GameObject.Find("MainCanvas").transform.Find("PanelMissionDescription").gameObject;
        missionDescriptionPanel.SetActive(true);
        MissionPanel misisonPanel = missionDescriptionPanel.GetComponent<MissionPanel>();
        // Mission Name
        misisonPanel.missionNameText.text = string.Format(missionName, missionAnimal.GetComponent<Animal>().animalInfo.name);
        // Mission Description
        misisonPanel.missionDescriptionText.text = string.Format(missionDescription, missionAnimal.GetComponent<Animal>().animalInfo.name);
        // Complete Time
        missionCompleteTime = (int)(5 / missionDifficulty);
        misisonPanel.completionTimeText.text = string.Format("Mission completion time: {0} minutes", missionCompleteTime);
        // Reward
        missionReward = (int)(missionAnimal.GetComponent<Animal>().animalInfo.difficultyWeight * 2 * 10) + 10;
        misisonPanel.rewardText.text = string.Format("Reward: {0} coins", missionReward);
        data = GameObject.Find("DontDestroyOnLoad").GetComponent<DataCarrier>();
        data.SetMissionData(string.Format(
            missionName, missionAnimal.GetComponent<Animal>().animalInfo.name),
            string.Format(missionDescription, missionAnimal.GetComponent<Animal>().animalInfo.name),
            missionCompleteTime,
            missionAnimal.GetComponent<Animal>().animalInfo.dataName,
            (int)(missionAnimal.GetComponent<Animal>().animalInfo.difficultyWeight*2*10)+10);
    }
}
