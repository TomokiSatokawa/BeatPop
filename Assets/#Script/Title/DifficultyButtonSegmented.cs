using Common;
using UnityEngine;

namespace Title
{
    public class DifficultyButtonSegmented : SegmentedControl
    {
        [SerializeField] private DifficultyColor _buttonColor;
        [SerializeField] private DifficultyButtonControl[] _difficultyButtons;

        protected override void InitializeButton(int i)
        {
            int index = i;
            _difficultyButtons[index].MainButton.onClick.AddListener(() => OnClick(index));
            Color mainColor = _buttonColor.GetDifficultyColor((Difficulty)i);
            _difficultyButtons[index].SetColor(mainColor, _selectColor, _unSelectColor);
            _difficultyButtons[index].IsSelect(false);
        }

        public override void OnClick(int i)
        {
            //ButtonÇÃêFïœçX
            _difficultyButtons[_currentIndex].IsSelect(false);
            _difficultyButtons[i].IsSelect(true);

            _currentIndex = i;
            _onValueChanged?.Invoke(i);
        }

        public override void SetButtonActive(int i, bool isActive)
        {
            _difficultyButtons[i].SetInteractable(isActive);
        }

        public void SetButtonLevel(int i, int level)
        {
            _difficultyButtons[i].SetLevel(level);
        }
    }
}
