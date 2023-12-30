using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimalExploration : MonoBehaviour, IInteractable
{
    private PlayerController player;
    public ParticleSystem heartParticles;
    public string dataName;
    public GameObject brokenHeart;

    public void InteractLogic()
    {
        //Instantiate(heartParticles, transform.position, Quaternion.identity);
        AddAnimalToPlayerList();
    }

    bool interacted = false;
    void AddAnimalToPlayerList()
    {
        heartParticles.Play();
        if (!interacted)
        {
            interacted = true;
            brokenHeart.SetActive(false);
            PlayerPrefs.SetString("AnimalToPlayerList", dataName);

            PlayerInventory.Instance.AddResource("Gold", GameObject.Find("DontDestroyOnLoad").GetComponent<DataCarrier>().missionReward);

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
}
