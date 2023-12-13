using UnityEngine;
using UnityEngine.UI;

public class InteractionButton : MonoBehaviour
{
    public Sprite pickaxeSprite;
    public Sprite axeSprite;
    public Sprite handSprite;
    public Sprite fishingRodSprite;
    public Sprite defaultSprite;

    public Image actionButtonImage;

    public void ChangeImage(string tool)
    {
        switch (tool)
        {
            case "Hand":
                actionButtonImage.sprite = handSprite;
                break;
            case "Pickaxe":
                actionButtonImage.sprite = pickaxeSprite;
                break;
            case "Axe":
                actionButtonImage.sprite = axeSprite;
                break;
            case "Fishing_Pole":
                actionButtonImage.sprite = fishingRodSprite;
                break;
            default:
                actionButtonImage.sprite = defaultSprite;
                break;
        }
    }
}
