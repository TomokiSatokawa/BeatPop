using TMPro;
using UnityEngine;

namespace Title.Custom
{
    /// <summary>
    /// カスタムパネルのパターンNameを更新する
    /// </summary>
    public class CustomPanelUIControl : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private PatternUIList _patternUIList;
        public void UpdateTitle()
        {
            _titleText.text = _patternUIList.CurrentSelectData.PatternName;
        }
    }
}