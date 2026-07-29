using System;
using Common.UI;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.UI
{
    /// <summary>
    /// チュートリアルUI
    /// </summary>
    public class TutorialUIControl : MonoBehaviour
    {
        [SerializeField] private Sprite[] _images;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _clauseButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private PageIndicatorControl _pageIndicator;
        [SerializeField] private ImageSlide _imageSlide;
        [SerializeField] private PanelControl _panel;

        public event Action OnClause;

        private int _currentPage;

        private void Start()
        {
            _nextButton.onClick.AddListener(NextPage);
            _clauseButton.onClick.AddListener(ClausePage);
            _backButton.onClick.AddListener(BackPage);

            _pageIndicator.OnClickButton.AddListener(x => ChangePage(x,true));
        }

        public void ShowTutorial()
        {
            _panel.OnActive();

            ChangePage(0,false);
        }

        private void NextPage()
        {
            ChangePage(_currentPage + 1,true);
        }
        private void BackPage()
        {
            ChangePage(_currentPage - 1, true);
        }

        private void ClausePage()
        {
            OnClause?.Invoke();
            _panel.OnHidden();

            OnClause = null;
        }

        public void ChangePage(int index,bool isAnimation)
        {
            _currentPage = index;
            _pageIndicator.ChangeIndex(_currentPage);
            UpdateButton();

            if (isAnimation)
            {
                _imageSlide.ChangeImage(_images[_currentPage]);
            }
            else
            {
                _imageSlide.SetImage(_images[_currentPage]);
            }
        }

        private void UpdateButton()
        {
            _backButton.interactable = _currentPage > 0;
            _nextButton. gameObject.SetActive(_currentPage < _images.Length -1);
            _clauseButton.gameObject.SetActive(_currentPage == _images.Length -1);
        }
    }
}