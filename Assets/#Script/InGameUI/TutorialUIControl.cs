using System;
using Common.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace InGame.UI
{
    /// <summary>
    /// チュートリアルUI
    /// </summary>
    public class TutorialUIControl : MonoBehaviour
    {
        [SerializeField] private VideoClip[] _videos;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _clauseButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private PageIndicatorControl _pageIndicator;
        [SerializeField] private VideoSlide _imageSlide;
        [SerializeField] private PanelControl _panel;

        public event Action OnClause;

        private int _currentPage;

        private void Start()
        {
            _nextButton.onClick.AddListener(NextPage);
            _clauseButton.onClick.AddListener(ClausePage);
            _backButton.onClick.AddListener(BackPage);

            _pageIndicator.OnClickButton.AddListener(x =>
            {
                ChangePage(x);

            });
        }

        public void ShowTutorial()
        {
            _panel.OnActive();

            ChangePage(0);
        }

        private void NextPage()
        {
            ChangePage(_currentPage + 1);
        }
        private void BackPage()
        {
            ChangePage(_currentPage - 1);
        }

        private void ClausePage()
        {
            OnClause?.Invoke();
            _panel.OnHidden();

            OnClause = null;
        }

        public void ChangePage(int index)
        {
            //if (!_imageSlide.IsCompleteAnimation()) return;

            if (_currentPage == index)
            {
                _imageSlide.SetVideo(_videos[index]);
            }
            else if(_currentPage > index)
            {
                _imageSlide.ChangeVideoSlideFromLeft(_videos[index]);
            }
            else
            {
                _imageSlide.ChangeVideoSlideFromRight(_videos[index]);
            }

            _currentPage = index;
            _pageIndicator.ChangeIndex(_currentPage);
            UpdateButton();
        }

        private void UpdateButton()
        {
            _backButton.interactable = _currentPage > 0;
            _nextButton. gameObject.SetActive(_currentPage < _videos.Length -1);
            _clauseButton.gameObject.SetActive(_currentPage == _videos.Length -1);
        }
    }
}