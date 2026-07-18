using Common.UI;
using UnityEngine;
using Title.SongSelect;
using UnityEngine.Events;

namespace Title.Custom
{
    public class PatternUIList : ScrollViewBase
    {
        [SerializeField] private PatternUIControl _prefab;
        [SerializeField] private CustomPatternLoader _patternLoader;
        [SerializeField] private CustomSound _sound;
        [SerializeField] private UnityEvent _onPatternSelect;

        private PatternUIControl _currentSelect;
        private PatternUIControl _usePattern;

        public PatternJsonData CurrentSelectData => _currentSelect.PatternData;
        public PatternJsonData UsePattern => _usePattern.PatternData;
        public async void ShowList()
        {
            if (!SongInfoControl.I.CurrentData.HasValue) return;
            int songID = SongInfoControl.I.CurrentData.Value.SongData.SongID;
            DeleteChild();
            var patternData = await CustomDataLoader.I.GetCustomPattern();
            if (patternData == null)
            {
                Debug.LogError($"SongID{songID} が無効または Patternが存在しません");
                return;
            }

            foreach (var pattern in patternData)
            {
                AddPatternUI(pattern);
            }
        }
        public void CreatePattern()
        {
            var newPattern = _patternLoader.GetDefaultPattern();
            AddPatternUI(newPattern);
            CustomDataLoader.I.AddPattern( newPattern);
        }

        private void AddPatternUI(PatternJsonData pattern)
        {
            var patternUI = InstantiateContent(_prefab);

            patternUI.SetData(pattern,uiData => {
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
            await CustomDataLoader.I.SavePattern(_currentSelect.PatternData);
        }
    }
}