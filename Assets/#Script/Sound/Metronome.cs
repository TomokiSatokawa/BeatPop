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
            float stageTime = GameManager.I.StageTime;

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
            _bpmInterval = 60f / GameManager.I.BPM;
        }
    }
}