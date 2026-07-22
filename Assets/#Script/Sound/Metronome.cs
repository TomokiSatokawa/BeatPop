using Common.BeatUpdate;
using Sound;
using UnityEngine;

namespace InGame.Sound
{
    /// <summary>
    /// ƒƒgƒƒm[ƒ€
    /// </summary>
    public class Metronome : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _firstBeatClip;
        [SerializeField] private AudioClip _subBeatClip;

        private void Start()
        {
            BeatUpdateManager.BeatUpdate.Subscribe(4, 0, x =>
            {
                if (x.Division == 1)
                {
                    SoundManager.SE.PlaySE(_firstBeatClip);
                }
                else
                {
                    SoundManager.SE.PlaySE(_subBeatClip);
                }
            });
        }
    }
}