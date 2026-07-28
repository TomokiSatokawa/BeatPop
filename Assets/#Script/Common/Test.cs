using Result.UI;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private TimingSliderView _timingSliderView;
    public void OnClick()
    {
        _timingSliderView.OnAnimation(Random.Range(0, 100), Random.Range(0, 100));
    }
}