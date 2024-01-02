using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceCost : MonoBehaviour
{
    public enum BuildType
    {
        Cage,
        Farm
    }

    public int woodCost;
    public int stoneCost;
    public int goldCost;
    public BuildType buildType;

    public int cageWidth;
    public int cageHeight;

    public TextMeshProUGUI stoneText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI woodText;

    private void Start()
    {
        stoneText.text = stoneCost.ToString("D3");
        goldText.text = goldCost.ToString("D3");
        woodText.text = woodCost.ToString("D3");
    }
}
