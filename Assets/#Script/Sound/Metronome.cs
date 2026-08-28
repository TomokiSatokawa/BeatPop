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
        [SerializeField] private AudioClip _firstBeatClip;
        [SerializeField] private AudioClip _subBeatClip;
        [SerializeField] private AudioClip _tickClip;

        private void Start()
        {
            BeatUpdateManager.BeatUpdate.Subscribe(8, 0, x =>
            {
                if (x.Division <= 1)
                {
                    SoundManager.SE.PlaySE(_firstBeatClip);
                }
                else if (x.Division <= 4)
                {
                    SoundManager.SE.PlaySE(_subBeatClip);
                }
                else
                {
                    SoundManager.SE.PlaySE(_tickClip);
                }
            });
        }
    }
}