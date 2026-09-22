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
        private const float IgnoreTime = 1f;
        private float _sceneStartTime;

        private void Awake()
        {
            GetButtonComponent();
            _type = _isMainButton ? SESoundType.ButtonClickMain : SESoundType.ButtonClickSub;

            _button?.onClick.AddListener(PlaySE);
            _sceneStartTime = Time.time;
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
            //初期値のSE防止
            if (Time.time - _sceneStartTime < IgnoreTime) return;

            SoundManager.SE.PlaySE(_type);
        }
    }
}