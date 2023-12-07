using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalInfo : MonoBehaviour
{
    public enum RarityTier
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    public string name;
    public RarityTier rarity;
    public float difficultyWeight;
    public float detectionRadius;
    public float walkingRadius;
    public float walkingSpeed;
    public float runningSpeed;
    public float runningTime;
}
