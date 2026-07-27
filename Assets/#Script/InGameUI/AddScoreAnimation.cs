using DG.Tweening;
using TMPro;
using UnityEngine;

namespace InGame.UI
{
    /// <summary>
    /// スコア加算アニメーション
    /// </summary>
    public class AddScoreAnimation : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        [Header("Move")]
        [SerializeField] private float _moveX = 120f;

        [Header("Duration")]
        [SerializeField] private float _duration = 0.8f;

        private Sequence _sequence;

        private void Awake()
        {
            _sequence = DOTween.Sequence()
                .SetAutoKill(false)
                .Pause();

            // 初期表示
            _sequence.Append(_text.DOFade(1f, 0.1f));

            // 横スライド
            _sequence.Join(transform.DOLocalMoveX(_moveX, _duration)
                .SetEase(Ease.OutCubic));

            // 終盤でフェードアウト
            _sequence.Join(_text.DOFade(0f, _duration * 0.4f)
                .SetDelay(_duration * 0.6f));

            _sequence.AppendCallback(() =>
            {
                gameObject.SetActive(false);
            });
        }

        public void Play(int score)
        {
            SetScore(score);

            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;

            _text.alpha = 0f;

            gameObject.SetActive(true);

            _sequence.Restart();
        }

        private void SetScore(int score)
        {
            _text.text = $"+{score}";
        }

        private void OnDestroy()
        {
            _sequence?.Kill();
        }
    }
}