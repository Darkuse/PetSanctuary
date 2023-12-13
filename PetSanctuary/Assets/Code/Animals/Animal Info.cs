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
    public string dataName;
    public RarityTier rarity;
    public float difficultyWeight;
    public float detectionRadius;
    public float walkingRadius;
    public float walkingSpeed;
    public float runningSpeed;
    public float runningTime;
    public int friendship=0;
    public int health=10;
    public int hunger=20;
}
