using Common.UI;
using UnityEngine;

namespace Title.Custom
{
    /// <summary>
    /// ‚»‚Ì‘¼‚ÌƒJƒXƒ^ƒ€
    /// </summary>
    public class CustomOther : CustomDataBinder<CustomOtherPattern>
    {
        [SerializeField] private CustomOtherData _customOtherData;
        [SerializeField] private SerializableDictionary<CustomOtherType, DoubleToggle> _toggles;

        public override CustomOtherPattern GetCustom()
        {
            var result = _customOtherData.GetDefault();
            var flags = OtherCustomFlags.Create(result.Flags);
            foreach (var kv in _toggles.Items)
            {
                if (kv.Value.IsOn)
                {

                    flags.Add(kv.Key);
                }
                else
                {
                    flags.Remove(kv.Key);
                }
            }
            result.Flags = flags.ToInt();
            return result ;
        }

        public override void OnDefault()
        {
            SetCustom(_customOtherData.GetDefault());
        }

        public override void SetCustom(CustomOtherPattern data)
        {
            var flags = OtherCustomFlags.Create(data.Flags);
            foreach (var kv in _toggles.Items)
            {
                kv.Value.SetValue(flags.Has(kv.Key));
            }
        }
    }

    [System.Serializable]   
    public struct CustomOtherPattern
    {
        public int Flags;
    }
}