using Common.BeatUpdate;
using UnityEngine;

namespace InGame.Sound
{
    public class Metronome : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _clip;
        [SerializeField] private AudioClip _headSE;

        void Start()
        {
            BeatUpdateManager.BeatUpdate.Subscribe(4,0,x =>
            {
                if(x.Division == 1)
                {
                    _audioSource.PlayOneShot(_headSE);
                }
                else
                {
                    _audioSource.PlayOneShot(_clip);
                }
            });
        }
    }
}