using InGame.UI;
using Result.UI;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private  ClearAnimationSwitcher _clearAnimationSwitcher;
    [SerializeField] private LevelAnimation _SlevelAnimation;
    [SerializeField] private XpSliderAnimation _xpSliderAnimation;
    public void OnClick()
    {
        //_xpSliderAnimation.Play(1, 0, 5, 50, _SlevelAnimation.Play);
    }
}