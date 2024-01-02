using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HarvestCost : MonoBehaviour
{
    public int yield;
    public TextMeshProUGUI yieldText;
    public int growthTime;
    public TextMeshProUGUI growthText;
    public int cost;
    public TextMeshProUGUI costText;
    [SerializeField]
    private FarmPanel farm;

    void Start()
    {
        yieldText.text =yield.ToString()+ " food";
        TimeSpan growthTimeSpan = TimeSpan.FromMinutes(growthTime);
        growthText.text = $"{growthTimeSpan.Hours}h {growthTimeSpan.Minutes}m";
        costText.text = cost.ToString() + "g";

        Button button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(() => farm.AddFood(this));
    }


}
