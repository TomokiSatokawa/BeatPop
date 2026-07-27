using UnityEngine;

namespace Title.Custom
{
    /// <summary>
    /// •ˆ–ÊƒJƒXƒ^ƒ€
    /// </summary>
    public class CustomChart : CustomDataBinder<CustomChartPattern>
    {
        [SerializeField] private SegmentedControl _flickLevel;
        [SerializeField] private SegmentedControl _longLevel;
        public override CustomChartPattern GetCustom()
        {
            return new CustomChartPattern
            {
                FlickConvertLevel = GetConvertLevel(_flickLevel),
                LongConvertLevel = GetConvertLevel(_longLevel)
            };
        }

        public override void OnDefault()
        {
            _flickLevel.OnClick(_flickLevel.StartIndex);
            _longLevel.OnClick(_longLevel.StartIndex);
        }

        public override void SetCustom(CustomChartPattern data)
        {
            _flickLevel.OnClick(data.FlickConvertLevel + _flickLevel.StartIndex);
            _longLevel.OnClick(data.LongConvertLevel + _longLevel.StartIndex);
        }
        private int GetConvertLevel(SegmentedControl control)
        {
            return control.CurrentIndex - control.StartIndex;
        }
    }

    [System.Serializable]
    public struct CustomChartPattern
    {
        public int FlickConvertLevel;
        public int LongConvertLevel;
    }
}