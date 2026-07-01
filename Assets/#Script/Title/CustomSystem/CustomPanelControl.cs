using Title.Custom;
using TMPro;
using UnityEngine;

public class CustomPanelControl : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private PatternUIList _patternUIList;
    public void UpdateTitle()
    {
        _titleText.text = _patternUIList.CurrentSelectData.PatternName;
    }
}
