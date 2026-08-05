using Common.UI;
using DG.Tweening;
using UnityEngine;

namespace Result.UI
{
    /// <summary>
    /// パネルをスライド切り替えする
    /// </summary>
    public class PanelSlider : MonoBehaviour
    {
        [SerializeField] private float _moveAmount;
        [SerializeField] private float _duration;
        [SerializeField] private PanelManager _panelManager;

        private Sequence _animation;

        public void ChangePanel(PanelControl panel)
        {
            _animation?.Kill();
            _animation = DOTween.Sequence();

            SetPosX(panel,_moveAmount);
            SetPosX(_panelManager.CurrentActive, 0);

            _animation.Append(panel.Rect.DOAnchorPosX(0, _duration));
            _animation.Join(_panelManager.CurrentActive.Rect.DOAnchorPosX(-_moveAmount, _duration));
            _animation.JoinCallback(() => _panelManager.ChangeActivePanel(panel, _duration));
        }

        private void SetPosX(PanelControl panel,float pos)
        {
            var panelPos = panel.Rect.anchoredPosition;
            panelPos.x = pos;
            panel.Rect.anchoredPosition = panelPos;
        }
    }
}