using System;
using DG.Tweening;
using InGame.Score;
using Title.PlayerData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Result.UI
{
    public class XpSliderAnimation : MonoBehaviour
    {
        [SerializeField] private LevelData _levelData;
        [SerializeField] private Image _slider;
        [SerializeField] private float _waitDuration = 1f;
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private TextMeshProUGUI _sliderValueText;
        [SerializeField] private TextMeshProUGUI _addXpValueText;
        [SerializeField] private TextMeshProUGUI _levelUpValueText;
        [SerializeField] private Ease _singleEase = Ease.InOutCubic;
        [SerializeField] private Ease _startEase = Ease.InOutCubic;
        [SerializeField] private Ease _fillEase = Ease.Linear;
        [SerializeField] private Ease _endEase = Ease.OutCubic;

        private Sequence _sequence;
        private int _currentLevel;

        private void Awake()
        {
            _currentLevel = PlayerDataLoader.Info.Level;

            float maxXP = _levelData.GetLevelUpXp(_currentLevel);
            float fillAmount = PlayerDataLoader.Info.XP / maxXP;

            _slider.fillAmount = fillAmount;
            UpdateText(fillAmount);
        }

        public void Play(IReadOnlyPlayerInfo startInfo, IReadOnlyPlayerInfo endInfo, Action<int> levelUpAction)
        {
            _addXpValueText.text = $"+{ScoreDataManager.ScoreData.GetXP()}XP";

            _sequence?.Kill();

            int startLevel = startInfo.Level;
            int endLevel = endInfo.Level;

            _currentLevel = startLevel;

            float startFill =
                startInfo.XP /
                (float)_levelData.GetLevelUpXp(startLevel);

            _slider.fillAmount = startFill;
            UpdateText(startFill);

            _sequence = DOTween.Sequence();
            _sequence.AppendInterval(_waitDuration);

            // レベルアップなし
            if (startLevel == endLevel)
            {
                float endFill = endInfo.XP / (float)_levelData.GetLevelUpXp(endLevel);
                _sequence.Append(DOMoveValue(_slider.fillAmount, endFill, _duration, _singleEase));

                _sequence.Play();
                return;
            }

            // 最初のレベルの残りを埋める
            _sequence.Append(DOMoveValue(_slider.fillAmount, 1f, _duration, _startEase));

            // 最初のレベルアップ
            _sequence.AppendCallback(GetLevelUpEvent(levelUpAction));

            // 中間レベル
            for (int level = startLevel + 1; level < endLevel; level++)
            {
                _sequence.Append(DOMoveValue(0, 1f, _duration, _fillEase));
                _sequence.AppendCallback(GetLevelUpEvent(levelUpAction));
            }

            // 最終レベル
            float endXP = endInfo.XP / (float)_levelData.GetLevelUpXp(endLevel);

            _sequence.Append(DOMoveValue(0, endXP, _duration, _endEase));

            _sequence.Play();
        }

        private TweenCallback GetLevelUpEvent(Action<int> levelUpAction)
        {
            return () =>
            {
                _currentLevel++;

                _slider.fillAmount = 0f;
                UpdateText(0f);

                levelUpAction?.Invoke(_currentLevel);
            };
        }

        private Tween DOMoveValue(float from, float value, float duration, Ease ease)
        {
            return DOVirtual.Float(from, value, duration,
                x =>
                {
                    _slider.fillAmount = x;
                    UpdateText(x);
                })
                .SetEase(ease);
        }

        private void UpdateText(float value)
        {
            int levelUpXP = _levelData.GetLevelUpXp(_currentLevel);

            int currentXP = Mathf.RoundToInt(levelUpXP * value);
            int remainXP = Mathf.Max(0, levelUpXP - currentXP);

            _sliderValueText.text =
                $"{currentXP:D5}/{levelUpXP:D5}";

            _levelUpValueText.text =
                $"次のレベルまで{remainXP:D5}";
        }

        private void OnDestroy()
        {
            _sequence?.Kill();
        }
    }
}