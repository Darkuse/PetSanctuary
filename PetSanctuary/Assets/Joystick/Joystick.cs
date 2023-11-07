using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform thumbstick;
    private bool isDragging = false;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out position))
        {
            float radius = background.rect.width / 2.0f;
            Vector2 delta = position - background.anchoredPosition;
            float distance = delta.magnitude;
            if (distance > radius)
            {
                delta = delta.normalized * radius;
                position = background.anchoredPosition + delta;
            }
            thumbstick.anchoredPosition = position;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false; 
        thumbstick.anchoredPosition = Vector2.zero;
    }

    public Vector2 GetDirection()
    {
        if (isDragging)
        {
            Vector2 delta = thumbstick.anchoredPosition;
            float distance = delta.magnitude;
            if (distance > 0)
            {
                return delta / distance;
            }
        }
        return Vector2.zero;
    }
}
