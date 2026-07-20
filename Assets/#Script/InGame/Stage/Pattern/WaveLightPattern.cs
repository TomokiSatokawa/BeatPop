using System;
using System.ComponentModel;
using System.Linq;
using DG.Tweening;

namespace InGame.Stage
{
    /// <summary>
    /// ウェーブパターン
    /// </summary>
    public class WaveLightPattern : LightPatternBase<WaveLightPatternData>
    {
        private Sequence _sequence;

        public override void InitializeCore(WaveLightPatternData data, StageLightBase[] lights)
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

        public override void BeatUpdate(int division)
        {
            if (division > Data.Division)
                return;

            if (_sequence == null)
                return;

            _sequence.Restart();
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
            var takeLights = _lights.Take(_lights.Length / 2);
            var skipLights = _lights.Skip(_lights.Length / 2);

            StageLightBase[] lights1;
            StageLightBase[] lights2;

            // 内→外
            if (Data.Type == WaveType.InsideToOutside)
            {
                lights1 = takeLights.Reverse().ToArray();
                lights2 = skipLights.ToArray();
            }
            // 外→内
            else
            {
                lights1 = takeLights.ToArray();
                lights2 = skipLights.Reverse().ToArray();
            }

            int lightCount = Math.Max(lights1.Length, lights2.Length);
            float interval = (float)Data.Duration / lightCount;

            for (int i = 0; i < lightCount; i++)
            {
                int index = i;

                if (index < lights1.Length)
                {
                    var target = lights1[index];
                    AppendWave(target);
                }

                if (index < lights2.Length)
                {
                    var target = lights2[index];
                    AppendWave(target);
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

            float interval = (float)Data.Duration / lights.Length;

            foreach (var light in lights)
            {
                var target = light;

                AppendWave(target);
                _sequence.AppendInterval(interval);
            }
        }

        private void AppendWave(StageLightBase light)
        {
            if (light is not PanelLightControl panelLight)
            {
                return;
            }

            _sequence.AppendCallback(() =>
                panelLight.Wave(Data.FlashDivision, Data.Power));
        }
    }


    [Serializable]
    public class WaveLightPatternData : LightPatternBaseData
    {
        public WaveType Type;
        public float FlashDivision;

        public override LightPatternBaseData Clone()
        {
            return new WaveLightPatternData()
            {
                PatternType = this.PatternType,
                Time = this.Time,
                Channel = this.Channel,
                Division = this.Division,
                Duration = this.Duration,
                Power = this.Power,
                MainColor = this.MainColor,

                Type = this.Type,
                FlashDivision = this.FlashDivision

            };
        }

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