using Common;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Title.SongSelect
{   
    public class SongUIControl : MonoBehaviour
    {
        [SerializeField] private UIPointerHover _uIPointerHover;
        [SerializeField] private Button _selectButton;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Image _jacketImage;
        [SerializeField] private Image _levelImage;
        [SerializeField] private DifficultyColor _difficultyColor;

        private SongSelectData _songData;
        private void Start()
        {
            _selectButton.onClick.AddListener(OnSelect);

            _uIPointerHover.IsPointerOver.Where(x => x).Subscribe(_ => SongPreviewPlayer.I.PlayPreview(_songData.SongData));
            _uIPointerHover.IsPointerOver.Where(x => !x).Subscribe(_ => SongPreviewPlayer.I.StopPreview());
        }
        public void SetData(SongSelectData data)
        {
            _songData = data;
            _nameText.text = data.SongData.SongName;
            _levelImage.color = _difficultyColor.GetDifficultyColor(data.Difficulty);
            _levelText.text = data.SongData.Charts.GetLevel(data.Difficulty).ToString();
            _jacketImage.sprite = data.SongData.Jacket;
        }
        public void OnSelect()
        {
            SongInfoControl.I.ShowInfo(_songData);
        }
    }
}