using System.Collections.Generic;
using Sound;
using TMPro;
using UnityEngine;

namespace Title.Custom
{
    public class CustomSound : MonoBehaviour
    {
        [SerializeField] private CustomSoundData _soundData;
        [Header("UI")]
        [SerializeField] private TMP_Dropdown _normalNotes;
        [SerializeField] private TMP_Dropdown _flickNotes;
        [SerializeField] private TMP_Dropdown _longStartNotes;
        [SerializeField] private TMP_Dropdown _longFillNotes;
        [SerializeField] private TMP_Dropdown _longEndNotes;

        private bool _isReviewSound = true;
        private void Start()
        {
            var tapOption = new List<TMP_Dropdown.OptionData>();
            foreach (var item in _soundData.TapSE)
            {
                tapOption.Add(new(item.Key));
            }
            InitializeDropdown(_normalNotes, tapOption,_soundData.TapSE);
            InitializeDropdown(_flickNotes, tapOption, _soundData.TapSE);
            InitializeDropdown(_longStartNotes, tapOption, _soundData.TapSE);
            InitializeDropdown(_longFillNotes, tapOption, _soundData.TapSE);
            InitializeDropdown(_longEndNotes, tapOption, _soundData.TapSE);

            OnDefault();
        }

        private void InitializeDropdown(TMP_Dropdown dropdown,List<TMP_Dropdown.OptionData> options
            ,IReadOnlyList<SerializableDictionary<string,SESoundType>.KeyPair> soundData)
        {
            dropdown.options = options;
            dropdown.onValueChanged.AddListener(x =>
            {
                SESoundType soundType = soundData[x].Value;
                OnChangeValue(soundType);
            });
;        }

        public void OnChangeValue(SESoundType seSound)
        {
            if (!_isReviewSound) return;
            SoundManager.I.PlaySESound(seSound);
        }
        public CustomSoundPattern GetCustom()
        {
            var result = new CustomSoundPattern();

            result.NormalSE = _soundData.TapSE[_normalNotes.value].Value;

            return result;
        }
        private void OnDefault()
        {
            _isReviewSound = false;
            _normalNotes.value = _soundData._normalDefault;
            _flickNotes.value = _soundData._flickDefault;
            _longStartNotes.value = _soundData._longStartDefault;
            _longFillNotes.value = _soundData._longFillDefault;
            _longEndNotes.value = _soundData._longEndDefault;
            _isReviewSound = true;
        }
        
    }
    public struct CustomSoundPattern
    {
        public SESoundType NormalSE;
    }
}

