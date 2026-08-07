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
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _coinCountText;
        [SerializeField] private TextMeshProUGUI _jewelryCountText;
        [SerializeField] private Image _xpSlider;

        public void UpdateView()
        {
            _nameText.text = PlayerDataLoader.Info.Name;
            _levelText.text = $"Lv.{PlayerDataLoader.Info.Level}";
            _coinCountText.text = $"{PlayerDataLoader.Info.CoinCount}";
            _jewelryCountText.text = $"{PlayerDataLoader.Info.JewelryCount}";

            _xpSlider.fillAmount = PlayerDataLoader.Info.XP / (float)_levelData.GetLevelUpXp(PlayerDataLoader.Info.Level);
        }
    }
}