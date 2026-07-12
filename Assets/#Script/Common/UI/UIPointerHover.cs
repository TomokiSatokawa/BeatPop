using UnityEngine;
using UnityEngine.EventSystems;

public class UIPointerHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool IsPointerOver { get; private set; }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsPointerOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsPointerOver = false;
    }
}