using DG.Tweening;
using Sound;
using UnityEngine;

namespace Result.UI
{
    /// <summary>
    /// リザルトの称号View
    /// </summary>
    public class ResultBadgeView : MonoBehaviour
    {
        [SerializeField] private GameObject _allPerfect;
        [SerializeField] private GameObject _fullCombo;

        [Header("Animation")]
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private float _startScale = 1.3f;
        [SerializeField] private float _waitTime = 1.5f;

        private Sequence _sequence;
        private void Start()
        {
            _allPerfect.SetActive(false);
            _fullCombo.SetActive(false);
        }

        public void ShowBadge(ResultType resultType)
        {
            _allPerfect.SetActive(false);
            _fullCombo.SetActive(false);

            switch (resultType)
            {
                case ResultType.AllPerfect:
                    PlayAnimation(_allPerfect);
                    break;

                case ResultType.FullCombo:
                    PlayAnimation(_fullCombo);
                    break;
                default:
                    PlayAnimation(_fullCombo);
                    break;
            }
        }

        private void PlayAnimation(GameObject badgeObject)
        {
            badgeObject.SetActive(true);

            var canvasGroup = badgeObject.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = badgeObject.AddComponent<CanvasGroup>();
            }

            canvasGroup.alpha = 0f;

            Transform trans = badgeObject.transform;
            trans.localScale = Vector3.one * 0.3f;
            trans.localRotation = Quaternion.Euler(0, 0, -8f);

            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            _sequence.AppendInterval(_waitTime);
            _sequence.AppendCallback(() => SoundManager.SE.PlaySE(SESoundType.BadgeView));
            // フェードイン
            _sequence.Append(canvasGroup.DOFade(1f, 0.15f));

            // ドン！と飛び出す
            _sequence.Join(
                trans.DOScale(1.15f, 0.22f)
                    .SetEase(Ease.OutExpo));

            // 少し戻る
            _sequence.Append(
                trans.DOScale(0.95f, 0.10f)
                    .SetEase(Ease.InOutQuad));

            // ちょうどいいサイズへ
            _sequence.Append(
                trans.DOScale(1f, 0.12f)
                    .SetEase(Ease.OutBack));

            // 少しだけ回転を戻す
            _sequence.Join(
                trans.DORotate(Vector3.zero, 0.18f)
                    .SetEase(Ease.OutBack));

            // 最後にパンチ
            _sequence.Append(
                trans.DOPunchScale(
                    Vector3.one * 0.12f,
                    0.35f,
                    8,
                    0.6f));
        }
    }
}