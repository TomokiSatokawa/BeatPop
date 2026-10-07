//AI
using UnityEngine;

namespace BeatPop.Editor
{
    /// <summary>
    /// 譜面難易度計算に使用する設定。
    /// </summary>
    [CreateAssetMenu(
        fileName = "BeatmapDifficultySettings",
        menuName = "BeatPop/Beatmap Difficulty Settings")]
    public class BeatmapDifficultySettings : ScriptableObject
    {
        [Header("難易度への影響度")]
        [Tooltip("平均ノーツ密度が難易度に与える影響度。")]
        [SerializeField] private float _densityWeight = 0.35f;

        [Tooltip("短時間に集中するノーツ密度が難易度に与える影響度。")]
        [SerializeField] private float _maxDensityWeight = 0.45f;

        [Tooltip("同時押しが難易度に与える影響度。")]
        [SerializeField] private float _chordWeight = 0.08f;

        [Tooltip("左右のレーン切り替えが難易度に与える影響度。")]
        [SerializeField] private float _switchWeight = 0.02f;

        [Tooltip("ノーツ種類が難易度に与える影響度。")]
        [SerializeField] private float _noteTypeWeight = 0.10f;

        [Tooltip("BPMが難易度に与える影響度。")]
        [SerializeField] private float _bpmWeight = 0.00f;


        [Header("ノーツ種類")]
        [Tooltip("通常ノーツの難易度係数。")]
        [SerializeField] private float _normalNoteWeight = 1.0f;

        [Tooltip("フリックノーツの難易度係数。")]
        [SerializeField] private float _flickNoteWeight = 1.2f;

        [Tooltip("ホールドノーツの難易度係数。")]
        [SerializeField] private float _holdNoteWeight = 1.3f;

        [Tooltip("ホールドフリックの難易度係数。")]
        [SerializeField] private float _holdFlickWeight = 1.5f;

        [Tooltip("Tickノーツの難易度係数。難易度計算には影響させない場合は0にする。")]
        [SerializeField] private float _tickNoteWeight = 0.0f;


        [Header("難易度レベル")]
        [Tooltip("算出される難易度レベルの最低値。")]
        [SerializeField] private int _minLevel = 1;

        [Tooltip("算出される難易度レベルの最高値。")]
        [SerializeField] private int _maxLevel = 30;


        [Header("難易度の基準値")]
        [Tooltip("平均ノーツ密度をスコアへ変換するときの基準値。")]
        [SerializeField] private float _densityBase = 1.0f;

        [Tooltip("最大ノーツ密度をスコアへ変換するときの基準値。")]
        [SerializeField] private float _maxDensityBase = 2.0f;


        [Header("難易度カーブ")]
        [Tooltip("最終的な難易度スコアに掛ける倍率。")]
        [SerializeField] private float _difficultyScale = 4.0f;

        [Tooltip("難易度の上昇カーブ。1より大きいほど高密度譜面の難易度が上がりやすくなる。")]
        [SerializeField] private float _difficultyExponent = 1.5f;

        [SerializeField] private float _offSetLevel;


        public float DensityWeight => _densityWeight;
        public float MaxDensityWeight => _maxDensityWeight;
        public float ChordWeight => _chordWeight;
        public float SwitchWeight => _switchWeight;
        public float NoteTypeWeight => _noteTypeWeight;
        public float BpmWeight => _bpmWeight;

        public float NormalNoteWeight => _normalNoteWeight;
        public float FlickNoteWeight => _flickNoteWeight;
        public float HoldNoteWeight => _holdNoteWeight;
        public float HoldFlickWeight => _holdFlickWeight;
        public float TickNoteWeight => _tickNoteWeight;

        public int MinLevel => _minLevel;
        public int MaxLevel => _maxLevel;

        public float DensityBase => _densityBase;
        public float MaxDensityBase => _maxDensityBase;

        public float DifficultyScale => _difficultyScale;
        public float DifficultyExponent => _difficultyExponent;

        public float OffSetLevel => _offSetLevel;
    }
}
