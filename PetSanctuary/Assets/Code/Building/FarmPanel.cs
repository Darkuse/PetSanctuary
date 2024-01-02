using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class FarmPanel : MonoBehaviour
{
    public Tilemap buildingGrid;
    public Tile emptyBed;
    public Tile growingBed;
    private FarmInfo farmInfo;
    public WarningPanel warning;

    public GameObject harvestSelectPanel;
    public GameObject harvestInfoPanel;

    public Button collectFood;
    public TextMeshProUGUI foodYieldText;
    public TextMeshProUGUI foodTimeText;

    private Coroutine growingCoroutine;

    public void NothingIsGrowing(Vector3Int position)
    {
        buildingGrid.SetTile(position, emptyBed);
    }

    public void SomethingIsGrowing(Vector3Int position)
    {
        buildingGrid.SetTile(position, growingBed);
    }

    public void UpdateInfoPanel(FarmInfo farmInfo, bool growing, string harvest, int time)
    {
        if (growingCoroutine!= null)
        {
            StopCoroutine(growingCoroutine);
        }
        this.farmInfo = farmInfo;
        if (growing)
        {
            harvestSelectPanel.SetActive(false);
            harvestInfoPanel.SetActive(true);
            collectFood.interactable = false;
            foodYieldText.text = "Food Yield: " + farmInfo.GetFoodCount();
            growingCoroutine = StartCoroutine(HarvestTime(DateTime.Parse(harvest), TimeSpan.FromMinutes(time)));
        }
        else
        {
            harvestSelectPanel.SetActive(true);
            harvestInfoPanel.SetActive(false);
        }
    }

    public void AddFood(HarvestCost harvest)
    {
        bool enoughFood = PlayerInventory.Instance.IfEnoughResources("Gold", harvest.cost);
        if (enoughFood)
        {
            PlayerInventory.Instance.RemoveResource("Gold", harvest.cost);
            farmInfo.PlantWasPlanted(harvest.yield,harvest.growthTime);
            buildingGrid.SetTile(farmInfo.GetBuildPosition(), growingBed) ;
        }
        else
        {
            warning.NotEnoughGold();
        }
    }

    private IEnumerator HarvestTime(DateTime harvest, TimeSpan time)
    {
        DateTime harvestTime = harvest + time;

        while (true)
        {
            TimeSpan timeUntilHarvest = harvestTime - DateTime.Now;

            if (timeUntilHarvest.TotalSeconds > 0)
            {
                foodTimeText.text = "Time Until Harvest: " + timeUntilHarvest.ToString(@"dd\:hh\:mm\:ss");
            }
            else
            {
                collectFood.interactable = true;
                foodTimeText.text = "Harvest is ready!";
                break;
            }
            yield return new WaitForSeconds(1);
        }
    }

    public void CollectFood()
    {
        farmInfo.PlantCollected();
        NothingIsGrowing(farmInfo.GetBuildPosition());
    }
    public void DeleteFarm()
    {
        farmInfo.DeleteBuild();
    }
}
