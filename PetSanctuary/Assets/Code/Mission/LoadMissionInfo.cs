using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMissionInfo : MonoBehaviour
{
    public TextMeshProUGUI missionName;
    public TextMeshProUGUI missionDescription;
    public DataCarrier data;
    public TextMeshProUGUI timer;

    [SerializeField]
    private float timeRemaining;

    private void Start()
    {
        data = GameObject.Find("DontDestroyOnLoad").GetComponent<DataCarrier>();
        missionName.text = data.missionName;
        missionDescription.text = data.missionDescription;
        timeRemaining = data.missionCompleteTime*60;
    }

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerDisplay(timeRemaining);
        }
    }

    void UpdateTimerDisplay(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            SceneManager.LoadScene("Base");
        }

        int minutes = (int)(timeToDisplay / 60);
        int seconds = (int)(timeToDisplay % 60);

        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
