using System;

[Serializable]
public class AnimalInfo
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
    public int friendship = 0;
    public int health = 10;
    public int hunger = 20;

    public AnimalInfo CloneAnimalInfo(AnimalInfo source)
    {
        return new AnimalInfo
        {
            name = source.name,
            dataName = source.dataName,
            rarity = source.rarity,
            difficultyWeight = source.difficultyWeight,
            detectionRadius = source.detectionRadius,
            walkingRadius = source.walkingRadius,
            walkingSpeed = source.walkingSpeed,
            runningSpeed = source.runningSpeed,
            runningTime = source.runningTime,
            friendship = source.friendship,
            health = source.health,
            hunger = source.hunger
        };
    }
}
