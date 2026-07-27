using System.Collections.Generic;
using UnityEngine;

namespace Title.Custom
{
    [CreateAssetMenu(fileName = "CustomSoundData", menuName = "Scriptable Objects/CustomSoundData")]
    public class CustomSoundData : ScriptableObject
    {
        //TODO:ÉmÅ[Écï SEÇ…ïœçXÇ∑ÇÈ
        [SerializeField] private List<SEData> _holdSE;
        [SerializeField] private List<SEData> _tapSE;
        [SerializeField] public int _normalDefault;
        [SerializeField] public int _flickDefault;
        [SerializeField] public int _longStartDefault;
        [SerializeField] public int _longFillDefault;
        [SerializeField] public int _longEndDefault;
        [SerializeField] public int _tickDefault;
        public IReadOnlyList<SEData> TapSE => _tapSE;
        public IReadOnlyList<SEData> HoldSE => _holdSE;

        public CustomSoundPattern GetDefaultCustom()
        {
            var result = new CustomSoundPattern();

            result.NormalSE = _normalDefault;
            result.FlickSE = _flickDefault;
            result.HoldStart = _longStartDefault;
            result.HoldFill = _longFillDefault;
            result.HoldEnd = _longEndDefault;
            result.TickNode = _tickDefault;

            return result;
        }
    }
    [System.Serializable]
    public class SEData
    {
        [SerializeField] private string _name;
        [SerializeField] private AudioClip _clip;
        [SerializeField] private float _volume;
        public string Name => _name;
        public AudioClip Clip => _clip;
        public float Volume => _volume;
    }
}