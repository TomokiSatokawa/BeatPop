using System.Collections.Generic;
using InGame.UI;
using UnityEngine;

public class EditorLineGenerator : MonoBehaviour
{
    [SerializeField] private int[] _lineWidth;
    [SerializeField] private RectTransform _content;
    [SerializeField] private float _extraClone;

    private readonly List<LineData> _clonedLine = new();

    private void Update()
    {
        float bpm = StageTimeController.I.BPM;
        float stageTime = StageTimeController.StageTime;

        if (bpm <= 0f)
            return;

        if (EditorManager.I.Division <= 0)
            return;

        if (EditorManager.I.Magnification <= 0f)
            return;

        double extraTime = _extraClone / EditorManager.I.Magnification;
        double displayTime = EditorManager.I.DisplayRange / EditorManager.I.Magnification;

        double minTime = stageTime - extraTime;
        double maxTime = stageTime + displayTime + extraTime;

        float beatInterval = 60f / bpm;
        float barInterval = beatInterval * 4f;
        float divisionInterval = barInterval / EditorManager.I.Division;

        // ”ÍˆÍŠOE×‚©‚·‚¬‚éü‚ðíœ
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
                lineData.SetPos(Vector3.right * 10000f);

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

    public void RemoveAll()
    {
        foreach (var line in _clonedLine)
        {
            line.Release();
        }

        _clonedLine.Clear();
    }
}