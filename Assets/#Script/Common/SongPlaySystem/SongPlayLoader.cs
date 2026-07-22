using Title.Custom;
using Title.SongSelect;
using UnityEngine;

namespace Common.PlaySystem
{
    /// <summary>
    /// 再生する曲データ、設定を保存するクラスを生成する
    /// </summary>
    public class SongPlayLoader : MonoBehaviour
    {
        // TODO: 開始セクション指定に対応する
        private const int StartSection = 0;

        [SerializeField] private SongPlayContext _managerPrefab;

        public void CreatePlayManager(SongSelectData songData,PatternJsonData patternJsonData)
        {
            if(_managerPrefab == null)
            {
                Debug.LogError($"[PlaySystem] {nameof(SongPlayContext)} Prefabがないです");
                return;
            }
            if(patternJsonData == null)
            {
                Debug.LogError($"[PlaySystem] パターンデータが設定されていません");
                return;
            }

            var playManager = Instantiate(_managerPrefab);
            playManager.SetData(songData,patternJsonData, StartSection);
        }
    }
}
