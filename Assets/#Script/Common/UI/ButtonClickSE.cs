using Sound;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI
{
    /// <summary>
    /// ボタンを押された時にサウンドを鳴らす
    /// </summary>
    public class ButtonClickSE : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private void Start()
        {
            GetButtonComponent();
            _button?.onClick.AddListener(() => SoundManager.SE.PlaySE(SESoundType.ButtonClick));
        }

        public void Reset()
        {
            GetButtonComponent();
        }

        private void GetButtonComponent()
        {
            if (_button == null)
            {
                _button = GetComponent<Button>();
            }
        }
    }
}