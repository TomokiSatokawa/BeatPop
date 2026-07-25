using UnityEngine;

namespace Title.Custom
{

    public class CustomChart : CustomDataBinder<CustomChartPattern>
    {
        [SerializeField] private SegmentedControl _flickLevel;
        [SerializeField] private SegmentedControl _longLevel;
        public override CustomChartPattern GetCustom()
        {
            return new CustomChartPattern()
            {
                FlickConvertLevel =_flickLevel.CurrentIndex - _flickLevel.StartIndex,
                LongConvertLevel = _longLevel.CurrentIndex - _longLevel.StartIndex
            };
        }

        public override void OnDefault()
        {

            _flickLevel.OnClick(_flickLevel.StartIndex);
        }

        public override void SetCustom(CustomChartPattern data)
        {
            _flickLevel.OnClick(data.FlickConvertLevel + _flickLevel.StartIndex);
            _longLevel.OnClick(data.LongConvertLevel + _longLevel.StartIndex);
        }
    }

    [System.Serializable]
    public struct CustomChartPattern
    {
        public int FlickConvertLevel;
        public int LongConvertLevel;
    }
}