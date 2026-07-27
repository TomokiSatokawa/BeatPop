using System;
using Common;
using Common.PlaySystem;
using Common.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Title.SongSelect
{
    public class SongInfoControl : SingletonMonoBehaviour<SongInfoControl>//TODO:シングルトンいる？
    {
        [Header("Animation")]
        [SerializeField] private PanelControl _panelControl;
        [SerializeField] private RectTransform _mainPanel;
        [SerializeField] private Vector2 _offScreen;
        [SerializeField] private Vector2 _pos;
        [SerializeField] private float _animationDuration;
        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Image _jacket;
        [SerializeField] private Image _difficultyImage;
        [SerializeField] private Button _clauseButton;
        [SerializeField] private Button _backGroundArea;
        [SerializeField] private TextMeshProUGUI _bpmInfo;
        [SerializeField] private TextMeshProUGUI _secondInfo;
        [SerializeField] private TextMeshProUGUI _levelText;
        [Header("Other")]
        [SerializeField] private DifficultyColor _difficultyColor;
        [SerializeField] private SongPlayLoader _playLoader;
        [SerializeField] private SceneTransition _sceneLoad;//TODO:仮
        [SerializeField] private SegmentedControl _segmentControl;
        [SerializeField] private SongPreviewPlayer _songPreviewPlayer;

        private SongSelectData? _currentData;
        public SongSelectData? CurrentData => _currentData;
        private void Start()
        {
            _clauseButton.onClick.AddListener(OnClause);
            _backGroundArea.onClick.AddListener(OnClause);
        }

        public void ShowInfo(SongSelectData data)
        {
            _currentData = data;
            UpdateInfoUI(data);

            OnActiveAnimation();
            foreach (Difficulty difficulty in Enum.GetValues(typeof(Difficulty)))
            {
                bool isExist = data.SongData.Charts.GetChart(difficulty) != null;
                _segmentControl.SetButtonActive((int)difficulty, isExist);
            }
            _segmentControl.OnClick((int)data.Difficulty);

            _songPreviewPlayer.PlayPreview(CurrentData.Value.SongData);
        }

        private void UpdateInfoUI(SongSelectData data)
        {
            _nameText.text = data.SongData.SongName;
            _jacket.sprite = data.SongData.Jacket;
            _difficultyImage.color = _difficultyColor.GetDifficultyColor(data.Difficulty);
            _levelText.text = data.SongData.Charts.GetLevel(data.Difficulty).ToString();
            _bpmInfo.text = data.SongData.BPM.ToString();
            _secondInfo.text = UIFormat.SecondToText(data.SongData.Audio.length);
        }

        public void OnChangeDifficulty(int value)
        {
            if (!_currentData.HasValue)
            {
                return;
            }
            if (Enum.IsDefined(typeof(Difficulty), value))
            {
                _currentData = new SongSelectData(_currentData.Value.SongData, (Difficulty)value);
                UpdateInfoUI(_currentData.Value);
                return;
            }
            Debug.LogError("不正な難易度の値");
        }

        public void OnClause()
        {
            _currentData = null;
            OnHiddenAnimation();
            _songPreviewPlayer.StopPreview();
        }

        private void OnActiveAnimation()
        {
            _panelControl.OnActive();
            _mainPanel.DOKill(true);
            _mainPanel.anchoredPosition = _offScreen;
            _mainPanel.DOAnchorPos(_pos, _animationDuration);
        }

        private void OnHiddenAnimation()
        {
            _mainPanel.DOKill(true);
            _mainPanel.anchoredPosition = _pos;
            _mainPanel.DOAnchorPos(_offScreen, _animationDuration)
                .OnComplete(() => _panelControl.OnHidden());
        }

        public void OnPlay()
        {
            if (!_currentData.HasValue) return;
            TitleManager.I.StartPlay(CurrentData.Value);
        }
    }
}