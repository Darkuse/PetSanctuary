using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionPanel : MonoBehaviour
{
    public TextMeshProUGUI missionNameText;
    public TextMeshProUGUI missionDescriptionText;
    public TextMeshProUGUI completionTimeText;
    public TextMeshProUGUI rewardText;
    


    public void CloseMissionPanel()
    {
        gameObject.SetActive(false);
    }

    public void AcceptMission()
    {
        StartCoroutine(LoadAsync("Exploration"));
    }

    public GameObject loadingScreenObj;
    public Slider loadingBar;
    IEnumerator LoadAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        loadingScreenObj.SetActive(true);

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / .9f);
            Debug.Log(asyncLoad.progress);
            loadingBar.value = progress;

            yield return null;
        }

        loadingScreenObj.SetActive(false);
    }
}
