using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyButtonControl : MonoBehaviour
{
    [SerializeField] private Button _mainButton;
    [SerializeField] private Image _outLineImage;
    [SerializeField] private Image _fillImage;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Color _invalidColor;

    public Button MainButton => _mainButton;

    private Color _mainColor;
    private Color _selectedColor;
    private Color _unselectedColor;

    public void SetColor(Color mainColor,Color selectColor,Color unselectColor)
    {
        _mainColor = mainColor;
        _selectedColor = selectColor;
        _unselectedColor = unselectColor;

        _outLineImage.color = mainColor;
        _fillImage.color = mainColor;

        IsSelect(false);
    }

    public void SetLevel(int level)
    {
        _levelText.text = level.ToString();
    }

    public void IsSelect(bool b)
    {
        _fillImage.gameObject.SetActive(b);
        _levelText.color  = b ? _selectedColor : _unselectedColor;
        _nameText.color = b ? _mainColor : _unselectedColor;
    }

    public void SetInteractable(bool b)
    {
        _levelText.gameObject.SetActive(b);
        _mainButton.interactable = b;
    }
}
