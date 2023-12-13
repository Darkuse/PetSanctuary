using UnityEngine;

public class CageInfo : MonoBehaviour, IInteractable
{
    private PlayerController player;
    private GameObject cagePanel;

    public void InteractLogic()
    {
        cagePanel.SetActive(true);
    }

    public void SetCagePanel(GameObject panel)
    {
        cagePanel = panel;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<PlayerController>();
            player.SetIInstance(this);
            GameObject.Find("ButtonAction").GetComponent<InteractionButton>().ChangeImage("Hand");

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (player != null)
            {
                player.clearIInstance();
                GameObject.Find("ButtonAction").GetComponent<InteractionButton>().ChangeImage(null);
            }
        }
    }
}
