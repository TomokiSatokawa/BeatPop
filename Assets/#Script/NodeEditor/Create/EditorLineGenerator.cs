using System.Collections.Generic;
using UnityEngine;

public class EditorLineGenerator : MonoBehaviour
{
    [SerializeField] private int[] _lineWidth;
    [SerializeField] private RectTransform _content;
    [SerializeField] private float _extraClone;

    private readonly List<LineData> _clonedLine = new();

    private void Update()
    {
        double extraTime = _extraClone / EditorManager.I.Magnification;

        double displayTime =
            EditorManager.I.DisplayRange / EditorManager.I.Magnification;

        double minTime = EditorManager.I.EditorTime.CurrentValue - extraTime;

        double maxTime = EditorManager.I.EditorTime.CurrentValue + displayTime + extraTime;


        float beatInterval = 60f / EditorManager.I.BPM;
        float barInterval = beatInterval * 4f;

        float divisionInterval = barInterval / EditorManager.I.Divition;

        float minPixelSpacing = 5f;
        double minTimeSpacing = minPixelSpacing / EditorManager.I.Magnification;

        // ”ÍˆÍŠOE×‚©‚·‚¬‚éü‚ðíœ
        for (int i = _clonedLine.Count - 1; i >= 0; i--)
        {
            FollowTime line = _clonedLine[i];

            bool outOfRange =
                line.Time < minTime ||
                line.Time > maxTime;

            bool tooDense = _clonedLine[i].Wide > EditorManager.I.Divition;

            if (outOfRange || tooDense)
            {
                _clonedLine.RemoveAt(i);
                line.Release();
            }
        }

        int startBar = Mathf.FloorToInt((float)minTime / barInterval);

        for (int bar = startBar; ; bar++)
        {
            double barStartTime = bar * barInterval;

            for (int division = 0; division < EditorManager.I.Divition; division++)
            {
                double time = barStartTime + division * divisionInterval;

                if (time < minTime)
                    continue;

                if (time > maxTime)
                    return;

                bool exists = false;

                foreach (FollowTime line in _clonedLine)
                {
                    if (Mathf.Abs((float)(line.Time - time)) < 0.001f)
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    var line =
                        PoolManager.I.Get<LineData>(
                            PoolPrefabType.EditorLine,
                            _content);

                    line.Time = time;
                    line.SetPos(Vector3.right * 10000f);

                    _clonedLine.Add(line);

                    int width;
                    if (EditorManager.I.Divition == 1)
                    {
                        width = _lineWidth[0];
                    }
                    else if (EditorManager.I.Divition == 4)
                    {
                        width = division == 0 ? _lineWidth[0] : _lineWidth[1];
                    }
                    else
                    {
                        if (division == 0)
                        {
                            width = _lineWidth[0];
                            line.Wide = 4;
                        }
                        else if (division % (EditorManager.I.Divition / 4) == 0)
                        {
                            width = _lineWidth[1];
                            line.Wide = 4;
                        }
                        else if (division % (EditorManager.I.Divition / 8) == 0)
                        {
                            width = _lineWidth[2];
                            line.Wide = 8;
                        }
                        else
                        {
                            width = _lineWidth[3];
                            line.Wide = 16;
                        }
                    }
                    line.Setwidth(width);
                }
            }
        }
    }
    public void RemoveAll()
    {
        foreach(var line in _clonedLine)
        {
            line.Release();
        }
        _clonedLine.Clear();
        EditorManager.I.ReloadTime();
    }
}