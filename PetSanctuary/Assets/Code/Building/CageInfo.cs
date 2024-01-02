using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CageInfo : MonoBehaviour, IInteractable
{
    private PlayerController player;
    private GameObject cagePanel;
    public string lastTimePetted;
    public string lastTimeFed;

    public AnimalInfo animalInfo;
    private SerializableCageInfo buildInfo;
    private CagePanel cp;
    private GridBuilding gridBuild;
    public int foodCount;
    public GameObject spawnedAnimal;
    private WarningPanel warningPanel;

    private void Start()
    {
        warningPanel = GameObject.Find("UIScripts").GetComponent<WarningPanel>();
        if (buildInfo.dataName == "" || buildInfo.dataName == null)
        {
            animalInfo = null;
            lastTimeFed = "";
            lastTimePetted = "";
        }
        else
        {
            animalInfo = GameObject.Find("Animals").GetComponent<AnimalList>().FindAnimal(buildInfo.dataName);
            lastTimeFed = buildInfo.lastTimeFed;
            lastTimePetted = buildInfo.lastTimePetted;
            animalInfo.name = buildInfo.name;
            animalInfo.hunger = HungerPointsNow(LastTime(lastTimeFed));
            if (HungerPointsNow(LastTime(lastTimeFed)) == 0)
            {
                animalInfo.health = Mathf.Clamp(buildInfo.health - (int)LastTime(lastTimeFed).TotalDays * 5, 0, 100);
                animalInfo.friendship = Mathf.Clamp(buildInfo.friendship - (int)LastTime(lastTimeFed).TotalDays * 10, 0, 100);
            }
            else
                animalInfo.health = buildInfo.health;
            SpawnAnimalInCage();
        }
    }

    // Deletes hunger points
    private int HungerPointsNow(TimeSpan now)
    {
        return Mathf.Clamp(buildInfo.hunger - (int)now.TotalDays * 10, 0, 100);
    }

    public void InteractLogic()
    {
        cagePanel.SetActive(true);
        cp = GameObject.Find("UIScripts").GetComponent<CagePanel>();
        cp.UpdateInfoPanel(this);
    }

    public void SetCagePanel(GameObject panel, SerializableCageInfo buildInfo, GridBuilding gridbuild)
    {
        cagePanel = panel;
        this.buildInfo = buildInfo;
        gridBuild = gridbuild;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            player = collider.GetComponent<PlayerController>();
            player.SetIInstance(this);
            GameObject.Find("ButtonAction").GetComponent<InteractionButton>().ChangeImage("Hand");
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        try
        {
            if (collider.CompareTag("Player"))
            {
                if (player != null)
                {
                    player.clearIInstance();
                    GameObject.Find("ButtonAction").GetComponent<InteractionButton>().ChangeImage(null);
                }
            }
        }
        catch { }
    }

    public void AnimalWasPetted()
    {
        TimeSpan timeSinceLastPetted = LastTime(lastTimePetted);

        // Check if time has passed
        if ((30 * animalInfo.difficultyWeight + 10) * 60 - timeSinceLastPetted.TotalSeconds <= 0 || timeSinceLastPetted.TotalSeconds == 0)
        {
            lastTimePetted = DateTime.Now.ToString("o");
            Debug.Log("Just petted Time: " + lastTimePetted);
            animalInfo.friendship = Mathf.Clamp(animalInfo.friendship + 5, 0, 100);
            animalInfo.health = Mathf.Clamp(animalInfo.health + 3, 0, 100);
            UpdateBuilingInfo();
        }
    }

    public TimeSpan LastTime(string timeLast)
    {
        if (timeLast == null || timeLast == "")
        {
            return TimeSpan.Zero;
        }
        DateTime lastTime = DateTime.Parse(timeLast, null, System.Globalization.DateTimeStyles.RoundtripKind);

        return DateTime.Now - lastTime;
    }

    public void AnimalWasFed()
    {
        TimeSpan timeSinceLastFed = LastTime(lastTimeFed);

        bool enoughFood = PlayerInventory.Instance.IfEnoughResources("Food", foodCount);
        if (enoughFood)
        {
            // Check if time has passed
            if ((30 * animalInfo.difficultyWeight + 10) * 60 - timeSinceLastFed.TotalSeconds <= 0 || timeSinceLastFed.TotalSeconds == 0)
            {
                lastTimeFed = DateTime.Now.ToString("o");
                animalInfo.friendship = Mathf.Clamp(animalInfo.friendship + 10, 0, 100);
                animalInfo.hunger = Mathf.Clamp(animalInfo.hunger + 10, 0, 100);
                animalInfo.health = Mathf.Clamp(animalInfo.health + 5, 0, 100);
                PlayerInventory.Instance.RemoveResource("Food", foodCount);
                UpdateBuilingInfo();
            }
        }
        else
        {
            warningPanel.NotEnoughFood();
        }
    }

    public void UpdateBuilingInfo()
    {
        buildInfo.name = animalInfo.name;
        buildInfo.dataName = animalInfo.dataName;
        buildInfo.friendship = animalInfo.friendship;
        buildInfo.hunger = animalInfo.hunger;
        buildInfo.health = animalInfo.health;
        buildInfo.lastTimeFed = lastTimeFed;
        buildInfo.lastTimePetted = lastTimePetted;
        gridBuild.UpdateCageInfo(buildInfo);
    }

    public void SpawnAnimalInCage()
    {
        foodCount = (int)Mathf.Round(10 + 8 * animalInfo.difficultyWeight);
        // Spawns animal to cage
        spawnedAnimal = Instantiate(GameObject.Find("Animals").GetComponent<AnimalList>().PickAnimalToSpawn(animalInfo.dataName), new Vector2(), Quaternion.identity);
        spawnedAnimal.transform.SetParent(transform, false);
    }

    public void DeleteCage()
    {
        gridBuild.DeleteCage(buildInfo.GetVector3Int(), buildInfo.buildingWidth, buildInfo.buildingHeight);
        cagePanel.SetActive(false);
        Destroy(gameObject);
        SceneManager.LoadScene("Base");
    }

}
