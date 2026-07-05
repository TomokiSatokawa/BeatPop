using InGame.UI;
using Unity.Content;
using UnityEditor.Experimental;
using UnityEngine;

namespace InGame.Sound
{
    public class Metronome : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _clip;

        private float _bpmInterval;
        private int _currentBeat;

        void Start()
        {
            ChangeBPM();
        }

        void FixedUpdate()
        {
            ChangeBPM();
            float stageTime;
            if (EditorManager.I == null)
            {
                stageTime = StageTimeController.StageTime;
            }
            else
            {
                stageTime = (float)EditorManager.I.EditorTime.CurrentValue;
            }

            int beat = Mathf.FloorToInt(stageTime / _bpmInterval);

            if (beat > _currentBeat)
            {
                _currentBeat = beat;
                _audioSource.PlayOneShot(_clip);
            }
        }

        [ContextMenu("BPMを変更")]
        public void ChangeBPM()
        {
            _bpmInterval = 60f / StageTimeController.I.BPM;
        }
    }
}