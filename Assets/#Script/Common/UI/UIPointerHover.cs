using R3;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPointerHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ReactiveProperty<bool> _isPointerOver = new();
    public ReadOnlyReactiveProperty<bool> IsPointerOver => _isPointerOver;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isPointerOver.Value = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isPointerOver.Value = false;
    }
}