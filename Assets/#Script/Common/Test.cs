using InGame.UI;
using Result.UI;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private ItemAddView _itemAddView;
    [SerializeField] private ItemAddView _itemAddView2;
    public void OnClick()
    {
        _itemAddView.Play(100, 200);
        _itemAddView2.Play(100, 200);
    }
}