using DG.Tweening;
using InGame.Node;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace InGame.UI
{
    /// <summary>
    /// ”»’èUI‚ÌView
    /// </summary>
    public class JudgeUIView : MonoBehaviour
    {
        [Header("Animation")]
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private SerializableDictionary<JudgementType, Sprite> _judgeSprite;
        [SerializeField] private float _popScale = 1.2f;
        [SerializeField] private float _popDuration = 0.08f;
        [SerializeField] private float _returnDuration = 0.12f;
        [SerializeField] private float _fadeDelay = 0.25f;
        [SerializeField] private float _fadeDuration = 0.25f;
        [SerializeField] private float _defaultSize = 1f;

        private Sequence _sequence;
        private void Awake()
        {
            _sequence = DOTween.Sequence()
                .Append(_sprite.transform.DOScale(_popScale, _popDuration).SetEase(Ease.OutBack))
                .Append(_sprite.transform.DOScale(_defaultSize, _returnDuration).SetEase(Ease.OutQuad))
                .Join(_sprite.DOFade(0f, _fadeDuration).SetDelay(_fadeDelay))
                .SetAutoKill(false)
                .Pause();
        }

        private void OnEnable()
        {
            _sprite.transform.localScale = Vector3.one * _defaultSize;
            _sprite.DOFade(1, 0);
        }

        public void PlayAnimation(JudgementType type)
        {
            if(!_judgeSprite.TryGetValue(type,out var sprite))
            {

                Debug.LogError($"[JudgeUIView] JudgementType JudgeImage is not found typ{type}]");
                return;
            }
            _sprite.sprite = sprite;
            _sequence.Restart();
        }

        private void OnDestroy()
        {
            _sequence?.Kill();
        }
    }
}