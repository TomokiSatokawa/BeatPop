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
        [SerializeField] private int _iconImageId;
        [SerializeField] private int _level;
        [SerializeField] private int _xp;
        [SerializeField] private int _coinCount;
        [SerializeField] private int _jewelryCount;

        private Subject<Unit> _onUpdateData = new();
        /// <summary>データ更新時</summary>
        public Observable<Unit> OnUpdateData => _onUpdateData;

        public string Name => _name;
        public int IconImageId => _iconImageId;
        public int Level => _level;
        public int XP => _xp;
        public int CoinCount => _coinCount;
        public int JewelryCount => _jewelryCount;

        public PlayerInfo()
        {
            _name = "";
            _iconImageId = 0;
            _level = 1;
            _xp = 0;
            _coinCount = 0;
            _jewelryCount = 0;
        }

        public PlayerInfo(string name,int icon, int level, int xp, int coinCount, int jewelryCount)
        {
            _name = name;
            _iconImageId = icon;
            _level = level;
            _xp = xp;
            _coinCount = coinCount;
            _jewelryCount = jewelryCount;
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

        /// <summary>
        /// LevelとXPを更新
        /// </summary>
        public void UpdateLevelXp(int level,int xp)
        {
            _level = level;
            _xp = xp;
            _onUpdateData.OnNext(Unit.Default);
        }

        /// <summary>
        /// コインを増やす
        /// </summary>
        public void AddCoin(int count)
        {
            _coinCount += count;
        }

        /// <summary>
        /// アイコンを変更する
        /// </summary>
        public void SetIconID(int iconId)
        {
            _iconImageId = iconId;
        }

        /// <summary>
        /// コピーを取得
        /// </summary>
        public PlayerInfo Clone()
        {
            return new(_name,_iconImageId, _level, _xp, _coinCount, _jewelryCount);
        }
    }

    public interface IReadOnlyPlayerInfo
    {
        public string Name { get; }
        public int IconImageId { get; }
        public int Level { get; }
        public int XP { get; }
        public int CoinCount { get; }
        public int JewelryCount { get; }
        public void UpdateName(string name);
        public void UpdateLevelXp(int level,int xp);
        public void AddCoin(int count);
        public void SetIconID(int iconId);
        public PlayerInfo Clone();
    }
}