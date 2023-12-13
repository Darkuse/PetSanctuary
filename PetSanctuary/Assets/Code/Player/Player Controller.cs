using UnityEngine;

// The PlayerController class is responsible for managing interactions between the player and interactable objects in the game.
public class PlayerController : MonoBehaviour
{
    private IInteractable interactableObject;

    public void Interact()
    {
        interactableObject.InteractLogic();
    }
    // Sets the current interactable object to the provided interactable instance.
    public void SetIInstance(IInteractable interactable)
    {
        interactableObject = interactable;
    }
    public void clearIInstance()
    {
        interactableObject = null;
    }
}
