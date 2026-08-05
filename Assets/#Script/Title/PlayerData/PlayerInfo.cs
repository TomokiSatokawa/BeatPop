using R3;
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

        private Subject<Unit> _onUpdateData = new();
        /// <summary>データ更新時</summary>
        public Observable<Unit> OnUpdateData => _onUpdateData;

        public string Name => _name;
        public int Level => _level;
        public int XP => _xp;
        public int CoinCount => _coinCount;
        public int JewelryCount => _jewelryCount;

        public PlayerInfo()
        {
            _name = "";
            _level = 1;
            _xp = 0;
            _coinCount = 0;
            _jewelryCount = 0;
        }

        /// <summary>
        ///プレイヤー名を変更する 
        /// </summary>
        public void UpdateName(string name)
        {
            //プレイヤー名が適正か
            var valid = PlayerNameValidator.IsValid(name);
            if (valid != "")
            {
                Debug.LogError($"[PlayerInfo] 不正なプレイヤー名 理由:{valid}");
                return;
            }

            _name = name;
            _onUpdateData.OnNext(Unit.Default);
        }
    }

    public interface IReadOnlyPlayerInfo
    {
        public string Name { get; }
        public int Level { get; }
        public int XP { get; }
        public int CoinCount { get; }
        public int JewelryCount { get; }
        public void UpdateName(string name);
    }
}