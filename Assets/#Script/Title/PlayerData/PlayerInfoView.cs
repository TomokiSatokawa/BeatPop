using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Title.PlayerData
{
    /// <summary>
    /// プレイヤーデータをUIに表示する
    /// </summary>
    public class PlayerInfoView : MonoBehaviour
    {
        [SerializeField] private LevelData _levelData;
        [SerializeField] private PlayerIconDatabase _playerIconDatabase;
        [SerializeField] private Image _playerIcon;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _coinCountText;
        [SerializeField] private TextMeshProUGUI _jewelryCountText;
        [SerializeField] private Image _xpSlider;

        public void UpdateView()
        {
            if (_playerIcon != null)
                _playerIcon.sprite = _playerIconDatabase.GetIcon(PlayerDataLoader.Info.IconImageId);

            if (_nameText != null)
                _nameText.text = PlayerDataLoader.Info.Name;

            if (_levelText != null)
                _levelText.text = $"{PlayerDataLoader.Info.Level}";

            if (_coinCountText != null)
                _coinCountText.text = $"{PlayerDataLoader.Info.CoinCount}";

            if (_jewelryCountText != null)
                _jewelryCountText.text = $"{PlayerDataLoader.Info.JewelryCount}";

            if (_xpSlider != null)
                _xpSlider.fillAmount = PlayerDataLoader.Info.XP / (float)_levelData.GetLevelUpXp(PlayerDataLoader.Info.Level);
        }
    }
}