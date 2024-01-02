using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

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
    public int gatherTime;

    private PlayerController player;
    public TextMeshProUGUI interactionText;

    public GameObject smokeParticle;


    public void InteractLogic()
    {
        
        StartCoroutine(GatherTimer(gatherTime, player));
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

    private IEnumerator GatherTimer(int totalSec, PlayerController playersBar)
    {
            playersBar.SetBar(true);
        for (int sec = totalSec; sec >= 0; sec--)
        {
            playersBar.GatherBar(sec, totalSec);
            yield return new WaitForSeconds(1);
        }
        Instantiate(smokeParticle, transform.position, Quaternion.identity);
        gatherResource();
        playersBar.SetBar(false);
    }
}
