//AI
#if UNITY_EDITOR

using System;
using System.Linq;
using InGame.Node;
using UnityEditor;
using UnityEngine;

namespace BeatPop.Editor
{
    /// <summary>
    /// 譜面を解析して難易度を算出するEditorWindow。
    /// </summary>
    public class BeatmapDifficultyAnalyzerWindow : EditorWindow
    {
        private TextAsset _beatmapAsset;
        private BeatmapDifficultySettings _settings;

        private DifficultyResult _result;

        [MenuItem("BeatPop/Beatmap Difficulty Analyzer")]
        private static void Open()
        {
            GetWindow<BeatmapDifficultyAnalyzerWindow>(
                "譜面難易度解析");
        }
        private Vector2 _scrollPosition;

        private void OnGUI()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(
                _scrollPosition);

            EditorGUILayout.Space(8);

            EditorGUILayout.LabelField(
                "譜面難易度解析",
                EditorStyles.boldLabel);

            EditorGUILayout.Space(8);

            _beatmapAsset =
                (TextAsset)EditorGUILayout.ObjectField(
                    "譜面",
                    _beatmapAsset,
                    typeof(TextAsset),
                    false);

            _settings =
                (BeatmapDifficultySettings)EditorGUILayout.ObjectField(
                    "難易度設定",
                    _settings,
                    typeof(BeatmapDifficultySettings),
                    false);

            EditorGUILayout.Space(8);

            using (new EditorGUI.DisabledScope(
                       _beatmapAsset == null ||
                       _settings == null))
            {
                if (GUILayout.Button(
                        "解析",
                        GUILayout.Height(32)))
                {
                    Analyze();
                }
            }

            if (_result != null)
            {
                EditorGUILayout.Space(16);

                DrawResult();
            }

            EditorGUILayout.EndScrollView();
        }

        private void Analyze()
        {
            try
            {
                NodeSaveData beatmap =
                    JsonUtility.FromJson<NodeSaveData>(
                        _beatmapAsset.text);

                if (beatmap == null)
                {
                    Debug.LogError(
                        "[BeatmapDifficultyAnalyzerWindow] " +
                        "譜面データの解析に失敗しました。");

                    return;
                }

                _result =
                    DifficultyCalculator.Calculate(
                        beatmap,
                        _settings);
            }
            catch (Exception exception)
            {
                Debug.LogError(
                    "[BeatmapDifficultyAnalyzerWindow] " +
                    $"譜面解析中にエラーが発生しました。\n{exception}");

                _result = null;
            }
        }

        private void DrawResult()
        {
            EditorGUILayout.LabelField(
                "解析結果",
                EditorStyles.boldLabel);

            EditorGUILayout.Space(4);

            EditorGUILayout.LabelField(
                "難易度",
                $"Lv.{_result.Level}");

            EditorGUILayout.LabelField(
                "難易度スコア",
                _result.Score.ToString("F2"));

            EditorGUILayout.Space(8);

            EditorGUILayout.LabelField(
                "基本情報",
                EditorStyles.boldLabel);

            EditorGUILayout.LabelField(
                "BPM",
                _result.Bpm.ToString("F1"));

            EditorGUILayout.LabelField(
                "ノーツ数",
                _result.NoteCount.ToString());

            EditorGUILayout.LabelField(
                "譜面時間",
                $"{_result.Duration:F2} 秒");

            EditorGUILayout.LabelField(
                "平均ノーツ密度",
                $"{_result.AverageNotesPerSecond:F2} notes/s");

            EditorGUILayout.LabelField(
                "最大ノーツ密度",
                $"{_result.MaxNotesPerSecond:F2} notes/s");

            EditorGUILayout.LabelField(
                "同時押し数",
                _result.ChordCount.ToString());

            EditorGUILayout.LabelField(
                "左右切り替え数",
                _result.LaneSwitchCount.ToString());

            EditorGUILayout.Space(12);

            EditorGUILayout.LabelField(
                "ノーツ種類",
                EditorStyles.boldLabel);

            EditorGUILayout.LabelField(
                "通常ノーツ",
                _result.NormalNoteCount.ToString());

            EditorGUILayout.LabelField(
                "フリックノーツ",
                _result.FlickNoteCount.ToString());

            EditorGUILayout.LabelField(
                "ホールドノーツ",
                _result.HoldNoteCount.ToString());

            EditorGUILayout.LabelField(
                "ホールドフリック",
                _result.HoldFlickCount.ToString());

            EditorGUILayout.LabelField(
                "Tickノーツ",
                _result.TickNoteCount.ToString());

            EditorGUILayout.Space(12);

            EditorGUILayout.LabelField(
                "スコア内訳",
                EditorStyles.boldLabel);

            EditorGUILayout.LabelField(
                "平均密度",
                _result.DensityScore.ToString("F2"));

            EditorGUILayout.LabelField(
                "最大密度",
                _result.MaxDensityScore.ToString("F2"));

            EditorGUILayout.LabelField(
                "同時押し",
                _result.ChordScore.ToString("F2"));

            EditorGUILayout.LabelField(
                "レーン切り替え",
                _result.SwitchScore.ToString("F2"));

            EditorGUILayout.LabelField(
                "ノーツ種類",
                _result.NoteTypeScore.ToString("F2"));

            EditorGUILayout.LabelField(
                "BPM",
                _result.BpmScore.ToString("F2"));

            EditorGUILayout.Space(8);

            EditorGUILayout.LabelField(
                "最終スコア",
                _result.Score.ToString("F2"));
        }
    }


    /// <summary>
    /// 難易度解析結果。
    /// </summary>
    public class DifficultyResult
    {
        public int Level;
        public float Score;

        public float Bpm;

        public int NoteCount;
        public float Duration;

        public float AverageNotesPerSecond;
        public float MaxNotesPerSecond;

        public int ChordCount;
        public int LaneSwitchCount;

        public int NormalNoteCount;
        public int FlickNoteCount;
        public int HoldNoteCount;
        public int HoldFlickCount;
        public int TickNoteCount;

        public float DensityScore;
        public float MaxDensityScore;
        public float ChordScore;
        public float SwitchScore;
        public float NoteTypeScore;
        public float BpmScore;
    }


    /// <summary>
    /// 譜面の難易度を計算する。
    /// </summary>
    public static class DifficultyCalculator
    {
        public static DifficultyResult Calculate(
            NodeSaveData beatmap,
            BeatmapDifficultySettings settings)
        {
            if (beatmap.Nodes == null ||
                beatmap.Nodes.Count == 0)
            {
                return new DifficultyResult
                {
                    Level = settings.MinLevel,
                    Bpm = beatmap.BPM
                };
            }

            NodeData[] nodes = beatmap.Nodes
                .Where(IsDifficultyNote)
                .OrderBy(node => node.Time)
                .ToArray();

            if (nodes.Length == 0)
            {
                return new DifficultyResult
                {
                    Level = settings.MinLevel,
                    Bpm = beatmap.BPM
                };
            }

            float duration =
                Mathf.Max(
                    0.01f,
                    nodes[^1].Time - nodes[0].Time);

            float averageNotesPerSecond =
                nodes.Length / duration;

            float maxNotesPerSecond =
                CalculateMaxNotesPerSecond(nodes);

            int chordCount =
                CalculateChordCount(nodes);

            int laneSwitchCount =
                CalculateLaneSwitchCount(nodes);

            float chordRate =
                (float)chordCount / nodes.Length;

            float switchRate =
                nodes.Length > 1
                    ? (float)laneSwitchCount /
                      (nodes.Length - 1)
                    : 0f;

            float densityScore =
                averageNotesPerSecond /
                settings.DensityBase;

            float maxDensityScore =
                maxNotesPerSecond /
                settings.MaxDensityBase;

            float chordScore =
                chordRate * 10f;

            float switchScore =
                switchRate * 10f;

            float noteTypeScore =
                CalculateNoteTypeScore(
                    nodes,
                    settings);

            float bpmScore =
                beatmap.BPM / 100f;

            float baseScore =
                densityScore *
                settings.DensityWeight +

                maxDensityScore *
                settings.MaxDensityWeight +

                chordScore *
                settings.ChordWeight +

                switchScore *
                settings.SwitchWeight +

                noteTypeScore *
                settings.NoteTypeWeight +

                bpmScore *
                settings.BpmWeight;

            float score =
                Mathf.Pow(
                    Mathf.Max(0f, baseScore),
                    settings.DifficultyExponent) *
                settings.DifficultyScale;

            score -= settings.OffSetLevel;

            int level =
                Mathf.Clamp(
                    Mathf.RoundToInt(score),
                    settings.MinLevel,
                    settings.MaxLevel);

            CountNoteTypes(
                beatmap.Nodes,
                out int normalNoteCount,
                out int flickNoteCount,
                out int holdNoteCount,
                out int holdFlickCount,
                out int tickNoteCount);

            return new DifficultyResult
            {
                Level = level,
                Score = score,

                Bpm = beatmap.BPM,

                NoteCount = nodes.Length,
                Duration = duration,

                AverageNotesPerSecond =
                    averageNotesPerSecond,

                MaxNotesPerSecond =
                    maxNotesPerSecond,

                ChordCount = chordCount,
                LaneSwitchCount = laneSwitchCount,

                NormalNoteCount = normalNoteCount,
                FlickNoteCount = flickNoteCount,
                HoldNoteCount = holdNoteCount,
                HoldFlickCount = holdFlickCount,
                TickNoteCount = tickNoteCount,

                DensityScore = densityScore,
                MaxDensityScore = maxDensityScore,
                ChordScore = chordScore,
                SwitchScore = switchScore,
                NoteTypeScore = noteTypeScore,
                BpmScore = bpmScore
            };
        }


        /// <summary>
        /// 難易度計算対象のノーツか判定する。
        /// </summary>
        private static bool IsDifficultyNote(
            NodeData node)
        {
            return node.PrefabType switch
            {
                PoolPrefabType.NormalNote => true,
                PoolPrefabType.FlickNote => true,
                PoolPrefabType.HoldNoteStart => true,
                PoolPrefabType.HoldFlickStart => true,

                // Tickは難易度計算に含めない。
                _ => false
            };
        }


        /// <summary>
        /// ノーツ種類による難易度スコアを計算する。
        /// </summary>
        private static float CalculateNoteTypeScore(
            NodeData[] nodes,
            BeatmapDifficultySettings settings)
        {
            if (nodes.Length == 0)
            {
                return 0f;
            }

            float totalWeight = 0f;

            foreach (NodeData node in nodes)
            {
                totalWeight +=
                    GetNoteDifficultyWeight(
                        node.PrefabType,
                        settings);
            }

            float averageWeight =
                totalWeight / nodes.Length;

            return averageWeight * 10f;
        }


        /// <summary>
        /// ノーツ種類ごとの難易度係数を取得する。
        /// </summary>
        private static float GetNoteDifficultyWeight(
            PoolPrefabType prefabType,
            BeatmapDifficultySettings settings)
        {
            return prefabType switch
            {
                PoolPrefabType.NormalNote =>
                    settings.NormalNoteWeight,

                PoolPrefabType.FlickNote =>
                    settings.FlickNoteWeight,

                PoolPrefabType.HoldNoteStart =>
                    settings.HoldNoteWeight,

                PoolPrefabType.HoldFlickStart =>
                    settings.HoldFlickWeight,

                _ => 0f
            };
        }


        /// <summary>
        /// ノーツ種類ごとの数を集計する。
        /// </summary>
        private static void CountNoteTypes(
            System.Collections.Generic.List<NodeData> nodes,
            out int normalNoteCount,
            out int flickNoteCount,
            out int holdNoteCount,
            out int holdFlickCount,
            out int tickNoteCount)
        {
            normalNoteCount = 0;
            flickNoteCount = 0;
            holdNoteCount = 0;
            holdFlickCount = 0;
            tickNoteCount = 0;

            foreach (NodeData node in nodes)
            {
                switch (node.PrefabType)
                {
                    case PoolPrefabType.NormalNote:
                        normalNoteCount++;
                        break;

                    case PoolPrefabType.FlickNote:
                        flickNoteCount++;
                        break;

                    case PoolPrefabType.HoldNoteStart:
                        holdNoteCount++;
                        break;

                    case PoolPrefabType.HoldFlickStart:
                        holdFlickCount++;
                        break;

                    case PoolPrefabType.TickNode:
                        tickNoteCount++;
                        break;
                }
            }
        }


        /// <summary>
        /// 1秒間に存在するノーツ数の最大値を計算する。
        /// </summary>
        private static float CalculateMaxNotesPerSecond(
            NodeData[] nodes)
        {
            const float Window = 1.0f;

            int maxCount = 0;
            int startIndex = 0;

            for (int i = 0; i < nodes.Length; i++)
            {
                while (
                    nodes[i].Time -
                    nodes[startIndex].Time >
                    Window)
                {
                    startIndex++;
                }

                int count =
                    i - startIndex + 1;

                if (count > maxCount)
                {
                    maxCount = count;
                }
            }

            return maxCount / Window;
        }


        /// <summary>
        /// 同時押し数を計算する。
        /// </summary>
        private static int CalculateChordCount(
            NodeData[] nodes)
        {
            int count = 0;

            for (int i = 1; i < nodes.Length; i++)
            {
                if (!Mathf.Approximately(
                        nodes[i].Time,
                        nodes[i - 1].Time))
                {
                    continue;
                }

                if (nodes[i].Lane ==
                    nodes[i - 1].Lane)
                {
                    continue;
                }

                count++;
            }

            return count;
        }


        /// <summary>
        /// 左右のレーン切り替え数を計算する。
        /// </summary>
        private static int CalculateLaneSwitchCount(
            NodeData[] nodes)
        {
            int count = 0;
            int previousLane = nodes[0].Lane;

            for (int i = 1; i < nodes.Length; i++)
            {
                if (Mathf.Approximately(
                        nodes[i].Time,
                        nodes[i - 1].Time))
                {
                    continue;
                }

                if (nodes[i].Lane != previousLane)
                {
                    count++;
                }

                previousLane = nodes[i].Lane;
            }

            return count;
        }
    }
}

#endif
