using System;
using System.ComponentModel;
using System.Linq;
using DG.Tweening;

namespace InGame.Stage
{
    public class WaveLightPattern : LightPatternBase<WaveLightPatternData>
    {
        private Sequence _sequence;
        public override void Initialize(WaveLightPatternData data, LightControlBase[] lights)
        {
            base.Initialize(data, lights);

            _sequence = DOTween.Sequence()
                .SetAutoKill(true)
                .Pause();

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
        private void SetSideWave()
        {
            if (Data.Type != WaveType.InsideToOutside
                && Data.Type != WaveType.OutsideToInside)
                return;

            LightControlBase[] lights1 = _lights.Take(_lights.Length / 2).ToArray();
            LightControlBase[] lights2 = _lights.Skip(_lights.Length / 2).ToArray();

            if(Data.Type == WaveType.InsideToOutside)
            {
                lights1.Reverse();
                lights2.Reverse();
            }

            float lightCount = MathF.Max(lights1.Length, lights2.Length);
            float interval = Data.Division / lightCount;

            for (int i = 0; i < lightCount; i++)
            {
                _sequence.AppendInterval(0);
                if(i < lights1.Length)
                {
                    _sequence.JoinCallback(() => lights1[i].Flash(Data.FlashDivision, Data.Power));
                }

                if(i < lights2.Length)
                {
                    _sequence.JoinCallback(() => lights2[i].Flash(Data.FlashDivision, Data.Power));
                }

                _sequence.AppendInterval(interval);
            }
        }

        private void SetMoveWave()
        {
            if (Data.Type != WaveType.RightToLeft
             && Data.Type != WaveType.LeftToRight)
                return;

            LightControlBase[] lights = _lights;

            if(Data.Type == WaveType.LeftToRight)
                lights.Reverse();

            float interval = Data.Division / lights.Length;

            for (int i = 0;i < lights.Length;i++)
            {
                _sequence.AppendCallback(() => lights[i].Flash(Data.FlashDivision, Data.Power));
                _sequence.AppendInterval(interval);
            }
        }
        public override void BeatUpdate(int division)
        {
            if (division > Data.Division) return;
            _sequence.Restart(true);
        }
    }
    public class WaveLightPatternData : LightPatternBaseData
    {
        public WaveType Type;
        public float FlashDivision;
    }
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
