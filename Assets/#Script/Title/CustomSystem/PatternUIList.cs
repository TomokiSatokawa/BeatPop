using Common.UI;
using UnityEngine;
using Title.SongSelect;

namespace Title.Custom
{
    public class PatternUIList : ScrollViewBase
    {
        [SerializeField] private PatternUIControl _prefab;
        [SerializeField] private CustomPatternLoader _patternLoader;
        [SerializeField] private CustomSound _sound;

        private PatternUIControl _currentSelect;
        private PatternUIControl _usePattern;
        public async void ShowList()
        {
            if (!SongInfoControl.I.CurrentData.HasValue) return;
            int songID = SongInfoControl.I.CurrentData.Value.SongData.SongID;
            DeleteChild();
            var patternData = await CustomDataLoader.I.GetCustomPattern(songID);
            if (patternData == null)
            {
                Debug.LogError($"SongID{songID} Ç™ñ≥å¯Ç‹ÇΩÇÕ PatternÇ™ë∂ç›ÇµÇ‹ÇπÇÒ");
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
            CustomDataLoader.I.AddPattern(SongInfoControl.I.CurrentData.Value.SongData.SongID, newPattern);
        }

        private void AddPatternUI(PatternJsonData pattern)
        {
            var patternUI = InstantiateContent(_prefab);

            patternUI.SetData(pattern, SelectPattern);
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

            _sound.SetCustom(patternUI.patternData.SoundPattern);
        }

        public async void SetPattern()
        {
            if (_usePattern == _currentSelect) return;
            _usePattern?.ShowSetPattern(false);
            _usePattern.patternData.IsSelect = false;
            await CustomDataLoader.I.SavePattern(_usePattern.patternData);

            _usePattern = _currentSelect;
            _usePattern.patternData.IsSelect = true;
            await CustomDataLoader.I.SavePattern(_usePattern.patternData);
            _usePattern.ShowSetPattern(true);
        }

        public async void SavePattern()
        {
            if (_currentSelect == null) return;
            _currentSelect.patternData.SoundPattern = _sound.GetCustom();
            await CustomDataLoader.I.SavePattern(_currentSelect.patternData);
        }
    }
}