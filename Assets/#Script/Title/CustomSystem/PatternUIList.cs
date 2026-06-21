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
        public async void ShowList()
        {
            if (!SongInfoControl.I.CurrentData.HasValue) return;
            int songID = SongInfoControl.I.CurrentData.Value.SongData.SongID;
            DeleteChild();
            var patternData = await CustomManifestLoader.I.GetCustomPattern(songID);
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
            CustomManifestLoader.I.AddPattern(SongInfoControl.I.CurrentData.Value.SongData.SongID, newPattern);
        }

        private void AddPatternUI(PatternJsonData pattern)
        {
            var patternUI = InstantiateContent(_prefab);

            patternUI.SetData(pattern, SelectPattern);
            if (pattern.IsSelect)
            {
                SelectPattern(patternUI);
            }
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
        private void SavePattern()
        {
            _currentSelect.patternData.SoundPattern = _sound.GetCustom();
            CustomManifestLoader.I.SavePattern(_currentSelect.patternData);
        }
    }
}