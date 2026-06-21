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
        public void SetCustom(CustomSoundPattern customSound)
        {
            _isReviewSound = false;
            _normalNotes.value = customSound.NormalSE;
            _flickNotes.value = customSound.FlickSE;
            _longStartNotes.value = customSound.HoldStart;
            _longFillNotes.value = customSound.HoldFill;
            _longEndNotes.value = customSound.HoldEnd;
            _isReviewSound = true;
        }
        public CustomSoundPattern GetCustom()
        {
            var result = new CustomSoundPattern();

            result.NormalSE = _normalNotes.value;
            result.FlickSE = _flickNotes.value;
            result.HoldStart = _longStartNotes.value;
            result.HoldFill  = _longFillNotes.value;
            result.HoldEnd = _longEndNotes.value;

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
}

