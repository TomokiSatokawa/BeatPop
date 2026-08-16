using Common.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Title.Custom
{
    /// <summary>
    /// パターンリスト
    /// </summary>
    public class PatternUIList : ScrollViewBase
    {
        [SerializeField] private PatternUIControl _prefab;
        [SerializeField] private CustomPatternLoader _patternLoader;
        [SerializeField] private CustomSound _sound;
        [SerializeField] private CustomChart _chart;
        [SerializeField] private CustomColor _color;
        [SerializeField] private UnityEvent _onPatternSelect;

        private PatternUIControl _currentSelect;
        private PatternUIControl _usePattern;

        public PatternJsonData CurrentSelectData => _currentSelect.PatternData;
        public PatternJsonData UsePattern => _usePattern.PatternData;

        public async void ShowList()
        {
            DeleteChildren();
            var patternDataList = await CustomDataLoader.I.GetAllCustomPattern();
            if (patternDataList == null)
            {
                Debug.LogError($"[PatternUIList] パターンリストがありません。");
                return;
            }

            foreach (var pattern in patternDataList)
            {
                AddPatternUI(pattern);
            }
        }

        public void CreatePattern()
        {
            var newPattern = _patternLoader.GetDefaultPattern();
            AddPatternUI(newPattern);
            CustomDataLoader.I.AddPattern(newPattern).Forget();
        }

        private void AddPatternUI(PatternJsonData pattern)
        {
            var patternUI = InstantiateContent(_prefab);

            patternUI.SetData(pattern, uiData =>
            {
                SelectPattern(uiData);
                _onPatternSelect?.Invoke();
            });
            if (pattern.IsSelect)
            {
                SelectPattern(patternUI);
                _usePattern = patternUI;
            }
            patternUI.ShowSetPattern(pattern.IsSelect);
        }

        public void SelectPattern(PatternUIControl patternUI)
        {
            if (_currentSelect != null)
            {
                _currentSelect.OnDeselect();
                SavePattern();
            }
            patternUI.OnSelect();
            _currentSelect = patternUI;

            //カスタムの値を変更
            _sound.SetCustom(patternUI.PatternData.SoundPattern);
            _chart.SetCustom(patternUI.PatternData.ChartPattern);
            _color.SetCustom(patternUI.PatternData.ColorPattern);
        }

        public async void SetPattern()
        {
            if (_usePattern == _currentSelect) return;
            _usePattern?.ShowSetPattern(false);
            _usePattern.PatternData.IsSelect = false;
            await CustomDataLoader.I.SavePattern(_usePattern.PatternData);

            _usePattern = _currentSelect;
            _usePattern.PatternData.IsSelect = true;
            await CustomDataLoader.I.SavePattern(_usePattern.PatternData);
            _usePattern.ShowSetPattern(true);
        }

        public async void SavePattern()
        {
            if (_currentSelect == null) return;
            _currentSelect.PatternData.SoundPattern = _sound.GetCustom();
            _currentSelect.PatternData.ChartPattern = _chart.GetCustom();
            _currentSelect.PatternData.ColorPattern = _color.GetCustom();
            await CustomDataLoader.I.SavePattern(_currentSelect.PatternData);
        }
    }
}