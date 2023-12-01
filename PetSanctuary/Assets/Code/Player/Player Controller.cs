using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IInteractable interactableObject;

    public void Interact()
    {
        interactableObject.InteractLogic();
    }

    public void SetIInstance(IInteractable interactable)
    {
        interactableObject = interactable;
    }
    public void clearIInstance()
    {
        interactableObject = null;
    }
}
