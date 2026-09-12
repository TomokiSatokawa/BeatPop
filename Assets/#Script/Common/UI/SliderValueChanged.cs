using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Common.UI
{
    /// <summary>
    /// スライダーの値変更が終了した時
    /// </summary>
    public class SliderValueChanged : MonoBehaviour, IPointerUpHandler
    {
        [SerializeField] private Slider _slider;

        public UnityEvent<float> OnValueChanged;
        public void OnPointerUp(PointerEventData eventData)
        {
            OnValueChanged?.Invoke(_slider.value);
        }
    }
}