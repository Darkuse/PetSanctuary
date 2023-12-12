using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelInteraction:MonoBehaviour
{
    // Closes or opens Panel
    public void InteractWithPanel(GameObject panel)
    {
        panel.SetActive(!panel.activeSelf);
    }
}
