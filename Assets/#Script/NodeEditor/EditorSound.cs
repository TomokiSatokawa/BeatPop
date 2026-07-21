using InGame;
using R3;
using UnityEngine;
namespace Editor
{
    public class EditorSound : MonoBehaviour
    {
        [SerializeField] private AudioSource _bgm;
        void Start()
        {
            StageTimeController.I.IsPlaying.Where(b => b).Subscribe(_ => OnPlay()).AddTo(this);
            StageTimeController.I.IsPlaying.Where(b => !b).Subscribe(_ => OnStop()).AddTo(this);
        }
        public void OnPlay()
        {
            _bgm.clip = StageTimeController.I.SongClip;
            _bgm.time = (float)StageTimeController.StageTime;
            _bgm.PlayScheduled(AudioSettings.dspTime);
        }
        public void OnStop()
        {
            _bgm.Pause();
        }
    }
}