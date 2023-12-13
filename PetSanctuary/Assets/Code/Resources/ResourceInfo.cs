using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceInfo : MonoBehaviour, IInteractable
{
    public enum GatheringTool
    {
        Hand,
        Pickaxe,
        Axe,
        Fishing_Pole
    }

    public string resourceName;
    [SerializeField]
    private string resourceDataName;
    public string description;
    public int resourceCount;
    public GatheringTool gatheringTools;

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
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject.Find("ButtonAction").GetComponent<InteractionButton>().ChangeImage(gatheringTools.ToString());
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
}
