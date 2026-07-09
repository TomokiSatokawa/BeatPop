using InGame.UI;
using R3;
using UnityEngine;

public class EditorSound : MonoBehaviour
{
    [SerializeField] private AudioSource _bgm;
    [SerializeField] private AudioSource _se;
    void Start()
    {
        StageTimeController.I.IsPlaying.Where(b => b).Subscribe(_ => OnPlay()).AddTo(this);
        StageTimeController.I.IsPlaying.Where(b => !b).Subscribe(_ => OnStop()).AddTo(this);  
    }
    public void OnPlay()
    {
        _bgm.time = (float)StageTimeController.StageTime;
        _bgm.clip = EditorManager.I.Audio;
        _bgm.PlayScheduled(AudioSettings.dspTime);
    }
    public void OnStop()
    {
        _bgm.Pause();
    }
}
