using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceInfo : MonoBehaviour, IInteractable
{
    public string resourceName;
    [SerializeField]
    private string resourceDataName;
    public string description;
    public int resourceCount;
    public List<string> gatheringTools = new List<string> { "Hand", "Pickaxe", "Axe", "Fishing Pole" };

    private PlayerController player;
    public TextMeshProUGUI interactionText;


    public void InteractLogic()
    {
        gatherResource();
    }

    void gatherResource()
    {
        Debug.Log(string.Format("{0} have {1} resources",resourceName,resourceCount));
        PlayerInventory.Instance.AddResource(resourceDataName, resourceCount);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<PlayerController>();
            player.SetIInstance(this);
            interactionText.text = resourceName;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.clearIInstance();
            interactionText.text = "";
        }
    }
}
