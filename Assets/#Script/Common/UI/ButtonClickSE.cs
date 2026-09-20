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
        [SerializeField] private bool _isMainButton;

        private SESoundType _type;

        private void Start()
        {
            GetButtonComponent();
            _type = _isMainButton ? SESoundType.ButtonClickMain : SESoundType.ButtonClickSub;

            _button?.onClick.AddListener(PlaySE);
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
        public void PlaySE()
        {
            SoundManager.SE.PlaySE(_type);
        }
    }
}