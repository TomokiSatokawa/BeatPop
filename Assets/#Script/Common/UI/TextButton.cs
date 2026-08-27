using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI
{
    public class TextButton : Button
    {
        [SerializeField] private TextMeshProUGUI _text;
        public TextMeshProUGUI Text => _text;
    }
}