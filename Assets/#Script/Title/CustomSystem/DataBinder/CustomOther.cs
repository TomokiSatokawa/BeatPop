using UnityEngine;

namespace Title.Custom
{
    /// <summary>
    /// ‚»‚Ì‘¼‚ÌƒJƒXƒ^ƒ€
    /// </summary>
    public class CustomOther : CustomDataBinder<CustomOtherPattern>
    {
        [SerializeField] private CustomOtherData _customOtherData;

        public override CustomOtherPattern GetCustom()
        {
            return _customOtherData.GetDefault();
        }

        public override void OnDefault()
        {
            SetCustom(_customOtherData.GetDefault());
        }

        public override void SetCustom(CustomOtherPattern data)
        {

        }
    }
    public struct CustomOtherPattern
    {
        public int Flags;
    }
}