using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SidePanel : MonoBehaviour
{
    public List<TextMeshProUGUI> resourceDisplay = new List<TextMeshProUGUI>();
    public TMP_InputField playerName;
    public GameObject playerNamePanel;

    public void ToStartMenu()
    {
        SceneManager.LoadScene("Start Menu");
        
    }

    private void Start()
    {
        if (PlayerPrefs.GetString("PlayerName", null) == null|| PlayerPrefs.GetString("PlayerName", null)=="")
        {
            playerNamePanel.SetActive(true);
        }
        PlayerInventory.Instance.resourceDisplay = resourceDisplay;
        PlayerInventory.Instance.AddResource("Wood", 0);
        PlayerInventory.Instance.AddResource("Stone", 0);
        PlayerInventory.Instance.AddResource("Gold", 0);
        PlayerInventory.Instance.AddResource("Food", 0);
        PlayerInventory.Instance.LoadResourcesToText();
    }

    public void ApplyPlayerName()
    {
        if (!string.IsNullOrWhiteSpace(playerName.text))
        {
            PlayerPrefs.SetString("PlayerName", playerName.text);
            PlayerPrefs.Save();
            playerNamePanel.SetActive(false);
        }
    }

    public void PanelInteraction(RectTransform panel, ref bool isOnTop, float inScenePosition, float overScenePosition)
    {
        StartCoroutine(MovePanel(inScenePosition, overScenePosition, panel, isOnTop));
        isOnTop = !isOnTop;
    }

    private float duration = 0.5f;
    private IEnumerator MovePanel(float inScenePosition, float overScenePosition, RectTransform panel, bool isOnTop)
    {
        float time = 0;
        Vector2 startPosition = panel.anchoredPosition;
        Vector2 endPosition = !isOnTop ? new Vector2(startPosition.x, inScenePosition) : new Vector2(startPosition.x, overScenePosition);

        while (time < duration)
        {
            time += Time.deltaTime;
            panel.anchoredPosition = Vector2.Lerp(startPosition, endPosition, time / duration);
            yield return null;
        }
    }

    [SerializeField]
    private RectTransform inventoryPanel;
    private bool inOnTop = false;
    public void InventoryInteraction()
    {
        PanelInteraction(inventoryPanel, ref inOnTop, 0, 50);
    }

    [SerializeField]
    private RectTransform missionPanel;
    private bool miOnTop = false;
    public void MissionInteraction()
    {
        PanelInteraction(missionPanel, ref miOnTop, -50, 450);
    }

    public void GetBackFromMission()
    {
        SceneManager.LoadScene("Base");
    }

    public void AddResource(string resourceName)
    {
        PlayerInventory.Instance.AddResource(resourceName, 10);
    }

    public void RandomSeed(Toggle toggle)
    {
        if (toggle.isOn)
        {
            PlayerPrefs.SetString("RandomSeed", "true");
        }
        else
        {
            PlayerPrefs.SetString("RandomSeed", "false");
        }
    }
}
