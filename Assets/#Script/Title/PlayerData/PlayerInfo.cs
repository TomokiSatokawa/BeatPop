using UnityEngine;

namespace Title.PlayerData
{
    /// <summary>
    /// プレイヤーの基本情報
    /// </summary>
    [System.Serializable]
    public class PlayerInfo : IReadOnlyPlayerInfo
    {
        [SerializeField] private string _name;
        [SerializeField] private int _level;
        [SerializeField] private int _xp;
        [SerializeField] private int _coinCount;
        [SerializeField] private int _jewelryCount;

        public string Name => _name;
        public int Level => _level;
        public int XP => _xp;
        public int CoinCount => _coinCount;
        public int JewelryCount => _jewelryCount;

        public PlayerInfo()
        {
            _name = "DefaultName";
            _level = 1;
            _xp = 0;
            _coinCount = 0;
            _jewelryCount = 0;
        }
    }

    public interface IReadOnlyPlayerInfo
    {
        public string Name { get; }
        public int Level { get; }
        public int XP { get; }
        public int CoinCount { get; }
        public int JewelryCount { get; }
    }
}