using System;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private PatternOptionMenuControl _optionMenu;
        [SerializeField] private CustomSound _sound;
        [SerializeField] private CustomChart _chart;
        [SerializeField] private CustomColor _color;
        [SerializeField] private CustomJudge _judge;
        [SerializeField] private CustomStage _stage;
        [SerializeField] private CustomOther _other;
        [SerializeField] private UnityEvent _onPatternSelect;

        private List<PatternUIControl> _clonedUI = new();
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

        protected override void OnDeletedChildren()
        {
            base.OnDeletedChildren();
            _clonedUI.Clear();
        }

        public void CreatePattern()
        {
            var newPattern = _patternLoader.GetDefaultPattern();
            AddPatternUI(newPattern);
            CustomDataLoader.I.AddPattern(newPattern).Forget();
        }

        public void DeletePattern(PatternJsonData pattern)
        {
            //生成済みUIを検索
            if (!TryFindPatternUI(pattern, out var patternUI)) return;

            //UIを削除
            _clonedUI.Remove(patternUI);
            Destroy(patternUI.gameObject);

            if (patternUI.PatternData.IsSelect)
            {
                SetPattern(_clonedUI[0]);
            }

            //ファイルを削除
            CustomDataLoader.I.DeletePattern(pattern).Forget();
        }

        private void AddPatternUI(PatternJsonData pattern)
        {
            var patternUI = InstantiateContent(_prefab);

            InitializePatternUI(pattern, patternUI);
            _clonedUI.Add(patternUI);
        }

        private void InitializePatternUI(PatternJsonData pattern, PatternUIControl patternUI)
        {
            Action<PatternUIControl> onSelect = uiData =>
            {
                SelectPattern(uiData);
                _onPatternSelect?.Invoke();
            };

            patternUI.SetData(pattern, onSelect, _optionMenu.Open);
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
            _judge.SetCustom(patternUI.PatternData.JudgePattern);
            _stage.SetCustom(patternUI.PatternData.SpeedPattern);
            _other.SetCustom(patternUI.PatternData.OtherPattern);
        }

        public async void SetPattern(PatternUIControl patternUI)
        {
            if (_usePattern == patternUI) return;
            _usePattern?.ShowSetPattern(false);
            _usePattern.PatternData.IsSelect = false;
            await CustomDataLoader.I.SavePattern(_usePattern.PatternData);

            _usePattern = patternUI;
            _usePattern.PatternData.IsSelect = true;
            await CustomDataLoader.I.SavePattern(_usePattern.PatternData);
            _usePattern.ShowSetPattern(true);

            InitializePatternUI(patternUI.PatternData,patternUI);
        }

        public void SetPattern()
        {
            SetPattern(_currentSelect); 
        }

        public void SetPattern(PatternJsonData patternData)
        {
            //生成済みUIを検索
            if (!TryFindPatternUI(patternData, out var patternUI)) return;

            SetPattern(patternUI);
        }

        public async void SavePattern()
        {
            if (_currentSelect == null) return;
            _currentSelect.PatternData.SoundPattern = _sound.GetCustom();
            _currentSelect.PatternData.ChartPattern = _chart.GetCustom();
            _currentSelect.PatternData.ColorPattern = _color.GetCustom();
            _currentSelect.PatternData.JudgePattern = _judge.GetCustom();
            _currentSelect.PatternData.SpeedPattern = _stage.GetCustom();
            _currentSelect.PatternData.OtherPattern = _other.GetCustom();
            await CustomDataLoader.I.SavePattern(_currentSelect.PatternData);
        }

        public void RenamePattern(PatternJsonData patternData,string newName)
        {
            patternData.PatternName = newName;
            CustomDataLoader.I.SavePattern(patternData).Forget();

            if (!TryFindPatternUI(patternData,out var patternUI)) return;

            InitializePatternUI(patternData, patternUI);
        }

        private  bool TryFindPatternUI(PatternJsonData patternData,out PatternUIControl patternUI)
        {
            patternUI = _clonedUI.FirstOrDefault(x => x.PatternData == patternData);
            return patternUI != null;
        }
    }
}