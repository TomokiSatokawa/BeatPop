using R3;
using UnityEngine;

public class EditorSound : MonoBehaviour
{
    [SerializeField] private AudioSource _bgm;
    [SerializeField] private AudioSource _se;
    void Start()
    {
        EditorManager.I.IsPlaying.Where(b => b).Subscribe(_ => OnPlay()).AddTo(this);  
        EditorManager.I.IsPlaying.Where(b => !b).Subscribe(_ => OnStop()).AddTo(this);  
    }
    public void OnPlay()
    {
        _bgm.time = (float)EditorManager.I.EditorTime.CurrentValue;
        _bgm.clip = EditorManager.I.AudioClip;
        _bgm.PlayScheduled(AudioSettings.dspTime);
    }
    public void OnStop()
    {
        _bgm.Pause();
    }
}
