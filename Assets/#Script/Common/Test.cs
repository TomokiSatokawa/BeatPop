using InGame.UI;
using Result.UI;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private  ClearAnimationSwitcher _clearAnimationSwitcher;
    public void OnClick()
    {
        _clearAnimationSwitcher.Play(ResultType.Clear);
    }
}