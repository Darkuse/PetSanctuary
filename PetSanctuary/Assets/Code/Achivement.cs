using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Achievement : MonoBehaviour
{
    public GameObject achievementHolderGrid;
    public GameObject achievementBoxPrefab;
    [SerializeField]
    private List<AchievementInfo> achievementInfos = new List<AchievementInfo>();
    public GameObject AchivDescirptionBox;
    public GameObject popup;

    private void Start()
    {
        achievementInfos = LoadAchievements();
        SaveAchievements();
        UpdateUI();
    }

    public void SaveAchievements()
    {
        string json = JsonUtility.ToJson(new AchievementSerialazableList(achievementInfos));
        PlayerPrefs.SetString("AchievementInfos", json);
    }

    public List<AchievementInfo> LoadAchievements()
    {
        string json = PlayerPrefs.GetString("AchievementInfos", "");

        if (!string.IsNullOrEmpty(json))
        {
            AchievementSerialazableList loadedData = JsonUtility.FromJson<AchievementSerialazableList>(json);
            return loadedData.AchievementInfos;
        }
        else
        {
            return achievementInfos;
        }
    }

    public void UpdateSave(string titleName)
    {
        foreach (var item in achievementInfos)
        {
            if (item.Title == titleName)
            {
                item.achieved = true;
                break;
            }
        }
        SaveAchievements();
        UpdateUI();
    }

    private void UpdateUI()
    {
        // Deletes everyCghild
        foreach (Transform child in achievementHolderGrid.transform)
        {
            Destroy(child.gameObject);
        }
        // Populates
        foreach (var item in achievementInfos)
        {
            GameObject newAchievementBox = Instantiate(achievementBoxPrefab, achievementHolderGrid.transform);
            AchievementBox achievementBox = newAchievementBox.AddComponent<AchievementBox>();
            achievementBox.Initialize(item.Title, item.description, item.achieved, AchivDescirptionBox);
        }
    }

    public void CheckTame()
    {
        // Variables to track progress
        int numberOfAnimalsTamed = PlayerPrefs.GetInt("numberOfAnimalsTamed", 0);

        // Taming Achievements
        switch (numberOfAnimalsTamed)
        {
            case 1:
                UpdateSave("First Friend");
                PopupField("First Friend");
                break;
            case 5:
                UpdateSave("Animal Whisperer");
                PopupField("Animal Whisperer");
                break;
            case 10:
                UpdateSave("Zoo Keeper");
                PopupField("Zoo Keeper");
                break;
            case 20:
                UpdateSave("Wildlife Champion");
                PopupField("Wildlife Champion");
                break;
        }
    }

    public void CheckPet()
    {
        int timesAnimalsPetted = PlayerPrefs.GetInt("timesAnimalsPetted", 0);
        // Animal Care Achievements
        switch (timesAnimalsPetted)
        {
            case 1:
                UpdateSave("Furry Friend");
                PopupField("Furry Friend");
                break;
            case 5:
                UpdateSave("Companion Caretaker");
                PopupField("Companion Caretaker");
                break;
            case 10:
                UpdateSave("Animal Lover");
                PopupField("Animal Lover");
                break;
        }
    }
    public void CheckFarm()
    {
        int timesCollectedFromFarm = PlayerPrefs.GetInt("timesCollectedFromFarm", 0);

        // Farming Achievements
        switch (timesCollectedFromFarm)
        {
            case 1:
                UpdateSave("First Harvest");
                PopupField("First Harvest");
                break;
            case 5:
                UpdateSave("Green Thumb");
                PopupField("Green Thumb");
                break;
        }
    }
    public void CheckGold()
    {
        int goldCoinsEarned = PlayerPrefs.GetInt("goldCoinsEarned", 0);
        // Gold Achievement
        if (goldCoinsEarned >= 200)
        {
            UpdateSave("Golden Beginnings");
            PopupField("Golden Beginnings");
        }
    }

    private void PopupField(string title)
    {
        popup.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = title;
        StartCoroutine(SlidePopupAndBack(popup, 200, 500, 2f));

    }

    private IEnumerator SlidePopupAndBack(GameObject popup, float targetY, float originalY, float duration)
    {
        // Slide to target position
        yield return StartCoroutine(SlidePopup(popup, targetY, duration));

        // Wait for 2 seconds
        yield return new WaitForSeconds(2f);

        // Slide back to original position
        yield return StartCoroutine(SlidePopup(popup, originalY, duration));
    }

    private IEnumerator SlidePopup(GameObject popup, float targetY, float duration)
    {
        Vector3 startPosition = popup.transform.localPosition;
        Vector3 endPosition = new Vector3(startPosition.x, targetY, startPosition.z);

        float time = 0;
        while (time < duration)
        {
            popup.transform.localPosition = Vector3.Lerp(startPosition, endPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        popup.transform.localPosition = endPosition;
    }


}
[Serializable]
public class AchievementInfo
{
    public string Title;
    public string description;
    public bool achieved;
}

[Serializable]
public class AchievementSerialazableList
{
    public List<AchievementInfo> AchievementInfos = new List<AchievementInfo>();

    public AchievementSerialazableList(List<AchievementInfo> achiv)
    {
        AchievementInfos = new List<AchievementInfo>();
        foreach (var item in achiv)
        {
            AchievementInfos.Add(item);
        }
    }
}

public class AchievementBox : MonoBehaviour
{
    private Image avatar;
    private GameObject completed;
    private string title;
    private string description;
    private bool achieved;
    private GameObject descriptionBox;

    private Button click;

    public void Initialize(string title, string description, bool achieved, GameObject descriptionBox)
    {
        this.title = title;
        this.description = description;
        this.achieved = achieved;
        this.descriptionBox = descriptionBox;

    }

    private void Start()
    {
        avatar = gameObject.transform.GetChild(0).GetComponent<Image>();
        completed = gameObject.transform.GetChild(2).gameObject;
        avatar.sprite = Resources.Load<Sprite>("Achievement/" + title);
        click = GetComponent<Button>();
        completed.SetActive(!achieved);
        click.onClick.AddListener(ButtonPress);
    }

    private void ButtonPress()
    {
        // title
        descriptionBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = title;
        // Desctiption
        descriptionBox.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = description;
        // Completion status
        if (achieved)
        {
            descriptionBox.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Completed!";
        }
        else
        {
            descriptionBox.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Not completed.";
        }
    }
}
