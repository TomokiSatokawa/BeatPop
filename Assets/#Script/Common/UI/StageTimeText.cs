using InGame;
using TMPro;
using UnityEngine;

namespace Common.UI
{
    /// <summary>
    /// StageTimeを表示する
    /// デバッグ用
    /// </summary>
    public class StageTimeText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        public void Update()
        {
            if (StageTimeController.I == null) return;
            _text.text = StageTimeController.StageTime.ToString("N2");
        }
    }
}