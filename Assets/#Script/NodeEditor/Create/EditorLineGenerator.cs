using System.Collections.Generic;
using InGame;
using UnityEngine;

namespace Editor
{
    /// <summary>
    /// ê¸ÇÃï\é¶
    /// </summary>
    public class EditorLineGenerator : MonoBehaviour
    {
        private const float HiddenPositionX = 10000f;
        [SerializeField] private int[] _lineWidth;
        [SerializeField] private RectTransform _content;
        [SerializeField] private float _extraClone;

        private readonly List<LineData> _clonedLine = new();

        private void Update()
        {
            if (!CanUpdate())
                return;

            double minTime, maxTime;
            float barInterval, divisionInterval;

            CalculateRange(out minTime, out maxTime, out barInterval, out divisionInterval);

            RemoveLine(minTime, maxTime);

            GenerateLines(minTime, maxTime, barInterval, divisionInterval);
        }

        private void GenerateLines(double minTime, double maxTime, float barInterval, float divisionInterval)
        {
            int startBar = Mathf.FloorToInt((float)(minTime / barInterval));
            int endBar = Mathf.CeilToInt((float)(maxTime / barInterval));

            for (int bar = startBar; bar <= endBar; bar++)
            {
                double barStartTime = bar * barInterval;

                for (int division = 0; division < EditorManager.I.Division; division++)
                {
                    double time = barStartTime + division * divisionInterval;

                    if (time < minTime)
                        continue;

                    bool exists = false;

                    foreach (var line in _clonedLine)
                    {
                        if (Mathf.Abs((float)(line.Time - time)) < 0.001f)
                        {
                            exists = true;
                            break;
                        }
                    }

                    if (exists)
                        continue;

                    var lineData = PoolManager.I.Get<LineData>(
                        PoolPrefabType.EditorLine,
                        _content);

                    lineData.Time = time;
                    lineData.SetPos(Vector3.right * HiddenPositionX);

                    int width;

                    if (EditorManager.I.Division == 1)
                    {
                        width = _lineWidth[0];
                        lineData.Wide = 1;
                    }
                    else if (EditorManager.I.Division == 4)
                    {
                        width = division == 0 ? _lineWidth[0] : _lineWidth[1];
                        lineData.Wide = 4;
                    }
                    else
                    {
                        if (division == 0)
                        {
                            width = _lineWidth[0];
                            lineData.Wide = 4;
                        }
                        else if (EditorManager.I.Division >= 4 &&
                                 division % (EditorManager.I.Division / 4) == 0)
                        {
                            width = _lineWidth[1];
                            lineData.Wide = 4;
                        }
                        else if (EditorManager.I.Division >= 8 &&
                                 division % (EditorManager.I.Division / 8) == 0)
                        {
                            width = _lineWidth[2];
                            lineData.Wide = 8;
                        }
                        else
                        {
                            width = _lineWidth[3];
                            lineData.Wide = 16;
                        }
                    }

                    lineData.SetWidth(width);
                    _clonedLine.Add(lineData);
                }
            }
        }

        private void RemoveLine(double minTime, double maxTime)
        {
            // îÕàÕäOÅEç◊Ç©Ç∑Ç¨ÇÈê¸ÇçÌèú
            for (int i = _clonedLine.Count - 1; i >= 0; i--)
            {
                var line = _clonedLine[i];

                bool outOfRange =
                    line.Time < minTime ||
                    line.Time > maxTime;

                bool tooDense = line.Wide > EditorManager.I.Division;

                if (outOfRange || tooDense)
                {
                    _clonedLine.RemoveAt(i);
                    line.Release();
                }
            }
        }

        private void CalculateRange(out double minTime, out double maxTime, out float barInterval, out float divisionInterval)
        {
            float bpm = StageTimeController.I.BPM;
            float stageTime = StageTimeController.StageTime;
            double extraTime = _extraClone / EditorManager.I.Magnification;
            double displayTime = EditorManager.I.DisplayRange / EditorManager.I.Magnification;

            minTime = stageTime - extraTime;
            maxTime = stageTime + displayTime + extraTime;
            float beatInterval = 60f / bpm;
            barInterval = beatInterval * 4f;
            divisionInterval = barInterval / EditorManager.I.Division;
        }

        private bool CanUpdate()
        {
            if (StageTimeController.I.BPM <= 0f)
                return false;

            if (EditorManager.I.Division <= 0)
                return false;

            if (EditorManager.I.Magnification <= 0f)
                return false;

            return true;
        }

        public void RemoveAll()
        {
            foreach (var line in _clonedLine)
            {
                line.Release();
            }

            _clonedLine.Clear();
        }
    }
}