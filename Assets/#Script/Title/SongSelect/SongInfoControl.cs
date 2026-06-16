using Common.PlaySystem;
using Common.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SongInfoControl : SingletonMonoBehaviour<SongInfoControl>
{
    [Header("Animation")]
    [SerializeField] private PanelControl _panelControl;
    [SerializeField] private RectTransform _mainPanel;
    [SerializeField] private Vector2 _offScreen;
    [SerializeField] private Vector2 _pos;
    [SerializeField] private float _animationDuration;
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Button _clauseButton;
    [SerializeField] private Button _backGroundArea;
    [SerializeField] private SongPlayLoader _playLoader;
    [SerializeField] private SceneLoad _sceneLoad;

    private IReadOnlySongData _currentData;
    private void Start()
    {
        _clauseButton.onClick.AddListener(OnClause);
        _backGroundArea.onClick.AddListener(OnClause);
    }

    public void ShowInfo(IReadOnlySongData data)
    {
        _currentData = data;
        _nameText.text = data.SongName;
        OnActiveAnimation();
    }
    private void OnActiveAnimation()
    {
        _panelControl.OnActive();
        _mainPanel.DOKill(true);
        _mainPanel.anchoredPosition = _offScreen;
        _mainPanel.DOAnchorPos(_pos, _animationDuration);
    }
    public void OnClause()
    {
        _mainPanel.DOKill(true);
        _mainPanel.anchoredPosition = _pos;
        _mainPanel.DOAnchorPos(_offScreen, _animationDuration)
            .OnComplete(() => _panelControl.OnHidden());
    }
    public void OnPlay()
    {
        _playLoader.OnLoad(_currentData);
        _sceneLoad.ChangeScene("InGame");
    }
}
