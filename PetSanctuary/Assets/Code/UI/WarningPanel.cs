using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WarningPanel : MonoBehaviour
{
    public GameObject warningPanel;
    public Button aceptButton;
    public TextMeshProUGUI releaseText;
    // Not enough resources
    public TextMeshProUGUI notenoughText;

    public CagePanel cagePanel;

    // Opens warning panel for release pet for reward
    public void CallReleaseButton(int goldReward)
    {
        FalseEverthing();
        releaseText.text = string.Format("Are you sure you want to release for {0} gold ?", goldReward);
        releaseText.gameObject.SetActive(true);
        aceptButton.gameObject.SetActive(true);
        aceptButton.onClick.AddListener(() => ReleaseButtonAccept());
    }

    public void ReleaseButtonAccept()
    {
        cagePanel.SellAnimal();
        warningPanel.SetActive(false);
    }

    // Opens warning panel for not enough resource notice
    public void NotEnoughResource()
    {
        FalseEverthing();
        notenoughText.gameObject.SetActive(true);
        notenoughText.text = "Not Enough Resources";
    }

    private void FalseEverthing()
    {
        warningPanel.SetActive(true);
        notenoughText.gameObject.SetActive(false);
        releaseText.gameObject.SetActive(false);
        aceptButton.gameObject.SetActive(false);
    }

    public void NotEnoughFood()
    {
        FalseEverthing();
        notenoughText.gameObject.SetActive(true);
        notenoughText.text = "Not Enough Food";
    }

    public void NotEnoughGold()
    {
        FalseEverthing();
        notenoughText.gameObject.SetActive(true);
        notenoughText.text = "Not Enough Gold";
    }

}
