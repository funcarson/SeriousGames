// TouchJoystick.cs
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public RectTransform handle;
    public float handleRange = 100f;
    private Vector2 input = Vector2.zero;

    public Vector2 Direction => input;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform, eventData.position, eventData.pressEventCamera, out pos);
        input = Vector2.ClampMagnitude(pos, handleRange) / handleRange;
        handle.anchoredPosition = input * handleRange;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }
}
