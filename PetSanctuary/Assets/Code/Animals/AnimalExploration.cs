using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AnimalExploration : MonoBehaviour, IInteractable
{
    private PlayerController player;
    public ParticleSystem heartParticles;
    public string dataName;
    public GameObject brokenHeart;
    public Image fillBar;
    public TextMeshProUGUI timer;
    private int timesPet = 0;
    WarningPanel warningPanel;

    private bool isPetCoroutineRunning = false;

    private void Start()
    {
        fillBar.transform.parent.gameObject.SetActive(false);
        warningPanel = GameObject.Find("Scripts").GetComponent<WarningPanel>();
    }

    public void InteractLogic()
    {
        if (!isPetCoroutineRunning)
        {
            if (PlayerInventory.Instance.IfEnoughResources("Food", 1))
            {
                fillBar.transform.parent.gameObject.SetActive(true);
                heartParticles.Play();
                StartCoroutine(TimerCoroutine(5));
                PlayerInventory.Instance.RemoveResource("Food", 1);
            }
            else
            {
                warningPanel.NotEnoughFoodExploration();
            }
        }
        if (timesPet >= 2)
        {
            fillBar.transform.parent.gameObject.SetActive(false);
            AddAnimalToPlayerList();
        }
    }

    bool interacted = false;
    void AddAnimalToPlayerList()
    {
        if (!interacted)
        {
            interacted = true;
            brokenHeart.SetActive(false);
            PlayerPrefs.SetString("AnimalToPlayerList", dataName);

            PlayerInventory.Instance.AddResource("Gold", GameObject.Find("DontDestroyOnLoad").GetComponent<DataCarrier>().missionReward);

            int temp = PlayerPrefs.GetInt("numberOfAnimalsTamed", 0);
            temp++;
            PlayerPrefs.SetInt("numberOfAnimalsTamed", temp);

            GameObject.Find("Achievements").GetComponent<Achievement>().CheckTame();
            Invoke("LoadScene", 2f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject.Find("ButtonAction").GetComponent<InteractionButton>().ChangeImage("Hand");
            player = collision.GetComponent<PlayerController>();
            player.SetIInstance(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.clearIInstance();
            GameObject.Find("ButtonAction").GetComponent<InteractionButton>().ChangeImage(null);
        }
    }

    private void LoadScene()
    {
        StartCoroutine(LoadAsync("Base"));
    }

    IEnumerator LoadAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    private IEnumerator TimerCoroutine(int durationInSeconds)
    {
        isPetCoroutineRunning = true;
        int secondsRemaining = durationInSeconds;
        timesPet++;
        fillBar.fillAmount = (float)timesPet / 2;

        while (secondsRemaining > 0)
        {
            yield return new WaitForSeconds(1);
            secondsRemaining--;
            timer.text = string.Format("{0:00}:{1:00}", secondsRemaining / 60, secondsRemaining);
        }
        isPetCoroutineRunning = false;
    }
}
