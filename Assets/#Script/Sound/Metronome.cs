using Common.BeatUpdate;
using UnityEngine;

namespace InGame.Sound
{
    public class Metronome : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _clip;

        void Start()
        {
            BeatUpdateManager.BeatUpdate.Subscribe(4,0,_ => _audioSource.PlayOneShot(_clip));
        }
    }
}