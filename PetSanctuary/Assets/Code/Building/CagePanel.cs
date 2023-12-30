using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CagePanel : MonoBehaviour
{
    [Header("Animal info")]
    public CageInfo cageInfo;

    [Header("Animal info panel")]
    // Panel which displays cage info about animal
    public GameObject animalPanelInfo;
    // Image for renaming Animal name
    public GameObject changeImage;
    // Renames animal after clicking
    public GameObject saveImage;

    // Animal Avatar
    public Image animalAvatarImage;

    // Main info about animal, his care and reward for caring after release
    public TextMeshProUGUI animalDataName;
    public TextMeshProUGUI animalRarity;
    public TextMeshProUGUI animalFoodRequirementText;
    public TextMeshProUGUI animalRewardForRealeasingText;

    // Status bar
    public Image healthBar;
    public Image friendshipBar;
    public Image hungerBar;
    // Animal player given name display
    public TMP_InputField nameInputField;
    // Button Text
    public TextMeshProUGUI buttonPlayText;
    public TextMeshProUGUI buttonFedText;
    public TextMeshProUGUI buttonReleaseText;

    [Header("Animal select grid")]
    public GameObject selectAnimalGrid;
    public GameObject animalBroderPrefab;
    public Transform gridContent;
    public AnimalList animalList;

    private Coroutine feedCoroutine;
    private Coroutine petCoroutine;

    [Header("Warning Panel")]
    public WarningPanel warningPanel;

    // On button press, let's you change animal name, and on confirmation saves Animal name to database
    public void ChangeAnimalName()
    {
        // Changes image for button to display, what are player doing with name,
        // first one to make input interactable, second one to confirm and save name changes
        changeImage.SetActive(!changeImage.activeSelf);
        saveImage.SetActive(!saveImage.activeSelf);

        if (saveImage.activeSelf)
        {
            nameInputField.interactable = true;
            nameInputField.Select();
        }
        else
        {
            nameInputField.interactable = false;
            cageInfo.animalInfo.name = nameInputField.text;
            cageInfo.UpdateBuilingInfo();
        }
    }

    // When Cage panels open, it is updated with animal infomration
    public void UpdateInfoPanel(CageInfo cageInfo)
    {

        this.cageInfo = cageInfo;

        // If in cage where a no animal, a panel to choose animal will pop-out
        if (cageInfo.animalInfo == null || cageInfo.animalInfo.dataName == null)
        {
            if (feedCoroutine != null)
                StopCoroutine(feedCoroutine);
            if (petCoroutine != null)
                StopCoroutine(petCoroutine);
            selectAnimalGrid.SetActive(true);
            animalPanelInfo.SetActive(false);
            PopulateGrid();
        }
        else
        {
            selectAnimalGrid.SetActive(false);
            animalPanelInfo.SetActive(true);

            // Load info about animal
            animalAvatarImage.sprite = Resources.Load<Sprite>("Animals/" + cageInfo.animalInfo.dataName + "Head");
            animalDataName.text = string.Format("Species: {0}", cageInfo.animalInfo.dataName);
            animalRarity.text = string.Format("Rarity: {0}", cageInfo.animalInfo.rarity.ToString());
            healthBar.fillAmount = cageInfo.animalInfo.health / 100f;
            hungerBar.fillAmount = cageInfo.animalInfo.hunger / 100f;
            friendshipBar.fillAmount = cageInfo.animalInfo.friendship / 100f;
            nameInputField.text = cageInfo.animalInfo.name;
            if (feedCoroutine != null)
                StopCoroutine(feedCoroutine);
            if (petCoroutine != null)
                StopCoroutine(petCoroutine);
            petCoroutine = StartCoroutine(WaitForPetTime());
            feedCoroutine = StartCoroutine(WaitForFedTime());
            animalFoodRequirementText.text = String.Format("Feed Requirement: {0} per feed",cageInfo.foodCount);
            ReleaseAnimal(false);
        }
    }

    // Populates animal selector with player animal list
    private void PopulateGrid()
    {
        // Deletes all childrens 
        for (int i = gridContent.childCount - 1; i >= 0; i--)
        {
            Destroy(gridContent.GetChild(i).gameObject);
        }
        // Check players Animal list
        foreach (var item in animalList.playerAnimalList)
        {
            // Populates grid with player animals
            GameObject newItem = Instantiate(animalBroderPrefab, gridContent);
            newItem.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Animals/" + item.dataName + "Head");
            Button button = newItem.GetComponent<Button>();
            button.onClick.AddListener(() => AddAnimalToCage(item.CloneAnimalInfo(item)));
        }
    }

    // Player selected animal from grid - player animal captured list
    public void AddAnimalToCage(AnimalInfo animal)
    {
        cageInfo.animalInfo = animal;
        cageInfo.UpdateBuilingInfo();
        cageInfo.SpawnAnimalInCage();
        UpdateInfoPanel(cageInfo);
        animalList.RemoveAnimalFromPlayerList(animal.dataName);
    }

    // This code will be used to change button text about the reward, what player will get for caring animal.
    // Bool checks if this function was called with button or just to update text
    public void ReleaseAnimal(bool buttonPressed)
    {
        if (buttonPressed)
        {
            warningPanel.CallReleaseButton(CountAnimalWorth());
        }
        animalRewardForRealeasingText.text = String.Format("Release Reward: {0} Gold Coins", CountAnimalWorth());
    }

    public void SellAnimal()
    {
        PlayerInventory.Instance.AddResource("Gold", CountAnimalWorth());
        cageInfo.animalInfo = new AnimalInfo();
        cageInfo.lastTimeFed = "";
        cageInfo.lastTimePetted = "";
        // Deletes animal texture from scene
        Destroy(cageInfo.spawnedAnimal);
        cageInfo.UpdateBuilingInfo();
        UpdateInfoPanel(cageInfo);
    }

    private int CountAnimalWorth()
    {
        return (int)Mathf.Round(cageInfo.animalInfo.friendship * cageInfo.animalInfo.difficultyWeight + cageInfo.animalInfo.health * cageInfo.animalInfo.difficultyWeight);
    }

    // Animal Play button
    public void PetAnimal()
    {
        cageInfo.AnimalWasPetted();
        if (petCoroutine != null)
            StopCoroutine(petCoroutine);
        petCoroutine = StartCoroutine(WaitForPetTime());
        UpdateInfoPanel(cageInfo);
        ReleaseAnimal(false);
    }

    private IEnumerator WaitForPetTime()
    {
        int leftTime = (int) cageInfo.LastTime(cageInfo.lastTimePetted).TotalSeconds;

        if (cageInfo.lastTimePetted != "")
        {
            int secondsRemaining = (int)(30 * cageInfo.animalInfo.difficultyWeight + 10) * 60 - leftTime;
            while (secondsRemaining > 0)
            {
                buttonPlayText.text = "To play wait for: " + TimeSpan.FromSeconds(secondsRemaining).ToString(@"hh\:mm\:ss");
                yield return new WaitForSeconds(1f);
                secondsRemaining--;
            }
        }
        buttonPlayText.text = "Play";
    }


    // Animal Feed button
    public void FedAnimal()
    {
        cageInfo.AnimalWasFed();
        if (feedCoroutine != null)
            StopCoroutine(feedCoroutine);
        feedCoroutine = StartCoroutine(WaitForFedTime());
        UpdateInfoPanel(cageInfo);
        ReleaseAnimal(false);
    }

    private IEnumerator WaitForFedTime()
    {
        int leftTime = (int)cageInfo.LastTime(cageInfo.lastTimeFed).TotalSeconds;

        if (cageInfo.lastTimeFed != "")
        {
            int secondsRemaining = (int)(60 * cageInfo.animalInfo.difficultyWeight + 10) * 60 - leftTime;

            while (secondsRemaining > 0)
            {
                buttonFedText.text = "To feed wait for: " + TimeSpan.FromSeconds(secondsRemaining).ToString(@"hh\:mm\:ss");
                yield return new WaitForSeconds(1f);
                secondsRemaining--;
            }
        }
        buttonFedText.text = "Feed";
    }


}
