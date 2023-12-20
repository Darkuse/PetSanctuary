using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public GameObject loadingScreenObj;
    public Slider loadingBar;

    public void StartGame()
    {
        StartCoroutine(LoadAsync("Base"));
    }

    public void StartNewGame()
    {
        PlayerPrefs.DeleteAll();
        StartCoroutine(LoadAsync("Base"));
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

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
