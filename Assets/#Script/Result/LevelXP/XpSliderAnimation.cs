using System;
using DG.Tweening;
using R3;
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
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private TextMeshProUGUI _sliderValueText;
        [SerializeField] private TextMeshProUGUI _levelUpValueText;
        [SerializeField] private Ease _singleEase = Ease.InOutCubic;
        [SerializeField] private Ease _startEase = Ease.InOutCubic;
        [SerializeField] private Ease _fillEase = Ease.Linear;
        [SerializeField] private Ease _endEase = Ease.OutCubic;

        private Sequence _sequence;
        private int _currentLevel;

        private void Awake()
        {
            _slider.fillAmount = PlayerDataLoader.Info.XP / (float)_levelData.GetLevelUpXp(PlayerDataLoader.Info.Level);
        }
        public void Play(IReadOnlyPlayerInfo startInfo,IReadOnlyPlayerInfo endInfo, Action<int> levelUpAction)
        {
            int startLevel = startInfo.Level;
            _currentLevel = startLevel;
            int startXP = startInfo.XP;
            int endLevel = endInfo.Level;
            int endXP = endInfo.XP;
            _sequence?.Kill();

            _slider.fillAmount = startXP / (float)_levelData.GetLevelUpXp(_currentLevel);

            _sequence = DOTween.Sequence();

            // レベルアップなし
            if (startLevel == endLevel)
            {
                _sequence.Append(DOMoveValue(endXP / (float)_levelData.GetLevelUpXp(_currentLevel), _duration, _singleEase));

                _sequence.Play();
                return;
            }

            // 最初のレベルの残りを埋める
            _sequence.Append(DOMoveValue(1f, _duration, _startEase));

            _sequence.AppendCallback(() =>
            {
                levelUpAction?.Invoke(startLevel + 1);
                _slider.fillAmount = 0f;
            });

            // 中間レベル
            for (int level = startLevel + 1; level < endLevel; level++)
            {
                _sequence.Append(DOMoveValue(1f, _duration, _fillEase));

                int callbackLevel = level + 1;

                _sequence.AppendCallback(() =>
                {
                    _currentLevel++;
                    levelUpAction?.Invoke(callbackLevel);
                    _slider.fillAmount = 0f;
                });
            }

            // 最終レベル
            _sequence.Append(DOMoveValue(endXP / (float)_levelData.GetLevelUpXp(endLevel), _duration, _endEase));

            _sequence.Play();
        }

        private Tween DOMoveValue(float value, float duration, Ease ease)
        {
            return DOVirtual.Float(_slider.fillAmount, value, duration, x =>
            {
                _slider.fillAmount = x;
                UpdateText(x);
            }).SetEase(ease);
        }
        private void UpdateText(float value)
        {
            int _LevelUpXp = _levelData.GetLevelUpXp(_currentLevel);
            int currentXP = Mathf.RoundToInt(_LevelUpXp * value);
            int remainXP = _LevelUpXp - currentXP;

            _sliderValueText.text = $"{currentXP:D5}/{_LevelUpXp:D5}";

            _levelUpValueText.text = $"次のレベルまで{remainXP:D5}";
        }
    }
}