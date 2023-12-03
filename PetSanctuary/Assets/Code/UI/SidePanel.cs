using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SidePanel : MonoBehaviour
{


    public void ToStartMenu()
    {
        SceneManager.LoadScene("Start Menu");
    }

    [SerializeField]
    private RectTransform inventoryPanel;
    public void InventoryInteraction()
    {
        StartCoroutine(MoveInventoryPanel());
    }

    private bool isAtTop = false;
    private float duration = 0.5f;
    IEnumerator MoveInventoryPanel()
    {
        float time = 0;
        Vector2 startPosition = inventoryPanel.anchoredPosition;
        Vector2 endPosition = isAtTop ? new Vector2(startPosition.x, 0) : new Vector2(startPosition.x, 50);

        while (time < duration)
        {
            time += Time.deltaTime;
            inventoryPanel.anchoredPosition = Vector2.Lerp(startPosition, endPosition, time / duration);
            yield return null;
        }

        inventoryPanel.anchoredPosition = endPosition;
        isAtTop = !isAtTop;
    }
}
