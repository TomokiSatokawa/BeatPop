using System;
using System.ComponentModel;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace InGame.Stage
{
    public class WaveLightPattern : LightPatternBase<WaveLightPatternData>
    {
        private Sequence _sequence;

        public override void InitializeCore(WaveLightPatternData data, LightControlBase[] lights)
        {
            base.InitializeCore(data, lights);

            CreateSequence();

            switch (data.Type)
            {
                case WaveType.InsideToOutside:
                case WaveType.OutsideToInside:
                    SetSideWave();
                    break;

                case WaveType.LeftToRight:
                case WaveType.RightToLeft:
                    SetMoveWave();
                    break;
            }
        }


        private void CreateSequence()
        {
            _sequence?.Kill();

            _sequence = DOTween.Sequence()
                .SetAutoKill(false)
                .Pause();
        }


        private void SetSideWave()
        {
            var lights1 = _lights.Take(_lights.Length / 2).ToArray();

            var lights2 = _lights.Skip(_lights.Length / 2).ToArray();


            // 内→外
            if (Data.Type == WaveType.InsideToOutside)
            {
                lights1 = lights1.Reverse().ToArray();
                lights2 = lights2.ToArray();
            }
            // 外→内
            else
            {
                lights1 = lights1.ToArray();
                lights2 = lights2.Reverse().ToArray();
            }


            int lightCount = Math.Max(
                lights1.Length,
                lights2.Length
            );


            float interval = (float)Data.Duration / lightCount;

            for (int i = 0; i < lightCount; i++)
            {
                int index = i;


                if (index < lights1.Length)
                {
                    var light = lights1[index];

                    _sequence.AppendCallback(() => light.Wave(Data.FlashDivision, Data.Power));
                }


                if (index < lights2.Length)
                {
                    var light = lights2[index];

                    _sequence.AppendCallback(() => light.Wave(Data.FlashDivision, Data.Power));
                }

                _sequence.AppendInterval(interval);
            }
        }


        private void SetMoveWave()
        {
            var lights = _lights.ToArray();


            // 左→右
            if (Data.Type == WaveType.LeftToRight)
            {
                lights = lights.Reverse().ToArray();
            }


            float interval =    (float)Data.Duration / lights.Length;


            foreach (var light in lights)
            {
                var target = light;

                _sequence.AppendCallback(() => target.Wave(Data.FlashDivision, Data.Power));

                _sequence.AppendInterval(interval);
            }
        }


        public override void BeatUpdate(int division)
        {
            if (division > Data.Division)
                return;

            if (_sequence == null)
                return;

            _sequence.Restart();
        }
    }


    [Serializable]
    public class WaveLightPatternData : LightPatternBaseData
    {
        public WaveType Type;
        public float FlashDivision;
    }


    [Serializable]
    public enum WaveType
    {
        [Description("内 → 外")]
        InsideToOutside,

        [Description("外 → 内")]
        OutsideToInside,

        [Description("右 → 左")]
        RightToLeft,

        [Description("左 → 右")]
        LeftToRight
    }
}