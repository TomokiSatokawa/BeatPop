using UnityEngine;

namespace Title.Custom
{

    [CreateAssetMenu(fileName = "CustomColorData", menuName = "Scriptable Objects/Custom/CustomOtherData")]
    public class CustomOtherData : ScriptableObject
    {
        [SerializeField] private SerializableDictionary<CustomOtherType, bool> _defaultValue;

        public CustomOtherPattern GetDefault()
        {
            var result = new CustomOtherPattern();

            var flags = OtherCustomFlags.Create();
            foreach(var kv in _defaultValue.Items)
            {
                if (!kv.Value) continue;
                flags.Add(kv.Key);
            }

            result.Flags = flags.ToInt();
            return result;
        }
    }
}