using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private int _startCount = 3;
    [SerializeField] private float _duration = 0.8f;

    private Tween _currentTween;

    /// <summary>
    /// コールバック版
    /// </summary>
    public void Play(Action onComplete = null)
    {
        PlayAsync().ContinueWith(() =>
        {
            onComplete?.Invoke();
        }).Forget();
    }

    /// <summary>
    /// await版
    /// </summary>
    public async UniTask PlayAsync()
    {
        gameObject.SetActive(true);

        for (int i = _startCount; i >= 1; i--)
        {
            _text.text = i.ToString();

            _text.transform.localScale = Vector3.zero;
            _text.color = new Color(1, 1, 1, 0);

            _currentTween?.Kill();

            _currentTween = DOTween.Sequence()
                .Append(
                    _text.transform
                        .DOScale(1f, _duration * 0.5f)
                        .SetEase(Ease.OutBack)
                )
                .Join(_text.DOFade(1f, _duration * 0.2f))
                .AppendInterval(_duration * 0.3f)
                .Append(_text.DOFade(0f, _duration * 0.2f));

            await _currentTween.AsyncWaitForCompletion();
        }

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _currentTween?.Kill();
    }
}