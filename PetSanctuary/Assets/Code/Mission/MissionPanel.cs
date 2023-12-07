using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene("Exploration");
    }
}
