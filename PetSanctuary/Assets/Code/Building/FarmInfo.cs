using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FarmInfo : MonoBehaviour, IInteractable
{
    private SerializableFarmInfo buildInfo;
    private PlayerController player;
    private GameObject farmUIPanel;
    private GridBuilding gridBuild;

    private void Start()
    {
        if (buildInfo.growing)
            GameObject.Find("UIScripts").GetComponent<FarmPanel>().SomethingIsGrowing(buildInfo.GetVector3Int());
        else
            GameObject.Find("UIScripts").GetComponent<FarmPanel>().NothingIsGrowing(buildInfo.GetVector3Int());
    }

    public void SetFarmPanel(GameObject panel, SerializableFarmInfo buildInfo, GridBuilding gridbuild)
    {
        farmUIPanel = panel;
        gridBuild = gridbuild;
        this.buildInfo = buildInfo;
    }

    public void InteractLogic()
    {
        farmUIPanel.SetActive(true);
        GameObject.Find("UIScripts").GetComponent<FarmPanel>().UpdateInfoPanel(this, buildInfo.growing, buildInfo.growStartTime, buildInfo.growTime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            player = collider.GetComponent<PlayerController>();
            player.SetIInstance(this);
            GameObject.Find("ButtonAction").GetComponent<InteractionButton>().ChangeImage("Hand");
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        try
        {
            if (collider.CompareTag("Player"))
            {
                if (player != null)
                {
                    player.clearIInstance();
                    GameObject.Find("ButtonAction").GetComponent<InteractionButton>().ChangeImage(null);
                }
            }
        }
        catch { }
    }

    public void UpdateBuildInfo()
    {
        gridBuild.UpdateFarmInfo(buildInfo);
    }

    public void DeleteBuild()
    {
        gridBuild.DeleteFarm(buildInfo.GetVector3Int());
        farmUIPanel.SetActive(false);
        Destroy(gameObject);
        SceneManager.LoadScene("Base");
    }

    public void PlantWasPlanted(int yield, int growTime)
    {
        buildInfo.growStartTime = DateTime.Now.ToString("o");
        buildInfo.growing = true;
        buildInfo.foodHarvest = yield;
        buildInfo.growTime = growTime;
        UpdateBuildInfo();

        GameObject.Find("UIScripts").GetComponent<FarmPanel>().UpdateInfoPanel(this, buildInfo.growing, buildInfo.growStartTime, buildInfo.growTime);
    }

    public void PlantCollected()
    {
        buildInfo.growStartTime = null;
        buildInfo.growing = false;
        farmUIPanel.SetActive(false);
        PlayerInventory.Instance.AddResource("Food", buildInfo.foodHarvest);
        UpdateBuildInfo();
    }

    public int GetFoodCount()
    {
        return buildInfo.foodHarvest;
    }
    public int GetGrowTime()
    {
        return buildInfo.growTime;
    }
    public Vector3Int GetBuildPosition()
    {
        return buildInfo.GetVector3Int();
    }
}
