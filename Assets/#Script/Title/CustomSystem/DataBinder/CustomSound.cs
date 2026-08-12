using System.Collections.Generic;
using Sound;
using TMPro;
using UnityEngine;

namespace Title.Custom
{
    /// <summary>
    /// ノーツSEのカスタム
    /// </summary>
    public class CustomSound : CustomDataBinder<CustomSoundPattern>
    {
        [SerializeField] private CustomSoundData _soundData;
        [Header("UI")]
        [SerializeField] private TMP_Dropdown _normalNotes;
        [SerializeField] private TMP_Dropdown _flickNotes;
        [SerializeField] private TMP_Dropdown _longStartNotes;
        [SerializeField] private TMP_Dropdown _longFillNotes;
        [SerializeField] private TMP_Dropdown _longEndNotes;

        private bool _enablePreview = true;
        private void Start()
        {
            var tapOption = new List<TMP_Dropdown.OptionData>();
            foreach (var item in _soundData.TapSE)
            {
                tapOption.Add(new(item.Name));
            }

            //TODO:ノーツ別SEになったらコレクションにする
            InitializeDropdown(_normalNotes, tapOption, _soundData.TapSE);
            InitializeDropdown(_flickNotes, tapOption, _soundData.TapSE);
            InitializeDropdown(_longStartNotes, tapOption, _soundData.TapSE);
            InitializeDropdown(_longFillNotes, tapOption, _soundData.TapSE);
            InitializeDropdown(_longEndNotes, tapOption, _soundData.TapSE);

            OnDefault();
        }

        private void InitializeDropdown(TMP_Dropdown dropdown, List<TMP_Dropdown.OptionData> options
            , IReadOnlyList<SEData> soundData)
        {
            dropdown.options = options;
            dropdown.onValueChanged.AddListener(x => OnChangeValue(soundData[x].Clip));
        }

        public void OnChangeValue(AudioClip seSound)
        {
            if (!_enablePreview) return;
            SoundManager.SE.PlaySE(seSound);
        }

        public override void SetCustom(CustomSoundPattern customSound)
        {
            _enablePreview = false;
            _normalNotes.value = customSound.NormalSE;
            _flickNotes.value = customSound.FlickSE;
            _longStartNotes.value = customSound.HoldStart;
            _longFillNotes.value = customSound.HoldFill;
            _longEndNotes.value = customSound.HoldEnd;
            _enablePreview = true;
        }

        public override CustomSoundPattern GetCustom()
        {
            var result = _soundData.GetDefaultCustom();

            result.NormalSE = _normalNotes.value;
            result.FlickSE = _flickNotes.value;
            result.HoldStart = _longStartNotes.value;
            result.HoldFill = _longFillNotes.value;
            result.HoldEnd = _longEndNotes.value;

            return result;
        }

        public override void OnDefault()
        {
            _enablePreview = false;
            _normalNotes.value = _soundData._normalDefault;
            _flickNotes.value = _soundData._flickDefault;
            _longStartNotes.value = _soundData._longStartDefault;
            _longFillNotes.value = _soundData._longFillDefault;
            _longEndNotes.value = _soundData._longEndDefault;
            _enablePreview = true;
        }

    }
    [System.Serializable]
    public struct CustomSoundPattern
    {
        public int NormalSE;
        public int HighScore;
        public int FlickSE;
        public int HoldStart;
        public int HoldFill;
        public int HoldEnd;
        public int TickNode;
    }
}

