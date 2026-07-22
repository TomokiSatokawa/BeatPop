using Common;
using Common.PlaySystem;
using Cysharp.Threading.Tasks;
using InGame;
using R3;
using Title.SongSelect;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Editor
{
    /// <summary>
    /// エディターのマネージャー
    /// </summary>
    public class EditorManager : SingletonMonoBehaviour<EditorManager>
    {
        [SerializeField] private SongListDataBase _songData;
        [SerializeField] private Scrollbar _scrollBar;
        [SerializeField] private RectTransform _displayRange;
        [SerializeField] private Difficulty _difficulty;
        [SerializeField] private int _division;
        [SerializeField] private float _magnification;
        public TextMeshProUGUI _te;

        public float DisplayRange => _displayRange.sizeDelta.x;
        public float Magnification => _magnification;
        public int Division => _division;

        private void Start()
        {
            EditorNodeData.I?.LoadedFile.Subscribe(x => Initialize(x)).AddTo(this);
            EditorLightData.I?.LoadedFile.Subscribe(x => Initialize(x)).AddTo(this);
            StageTimeController.I.IsPlaying.Subscribe(x => _scrollBar.interactable = !x).AddTo(this);

            ChangeDivisionCount(4);
        }

        public async void Initialize(NodeSaveData saveData)
        {
            if (saveData == null) return;
            GenerateSongPlayManager(saveData.SoundIndex);
            StageTimeController.I.SetPlayData(saveData);
            await PrepareSongAsync();
        }

        public async void Initialize(StageSaveData saveData)
        {
            if (saveData == null) return;
            var data = GenerateSongPlayManager(saveData.SongDataIndex);
            StageTimeController.I.SetPlayData(data.BPM, 0, 0);
            await PrepareSongAsync();
        }

        private async UniTask PrepareSongAsync()
        {
            await StageTimeController.I.SongLoadAsync();
            StageTimeController.I.StartSongPlay();
            StageTimeController.I.Pause();
        }

        private void Update()
        {
            _te.text = StageTimeController.StageTime.ToString("N2");

            StageTimeController.I.UpdateStageTime();

            //データがなかったら動かさない
            if (EditorNodeData.I != null && EditorNodeData.I.LoadedFile.CurrentValue == null) return;
            if (EditorLightData.I != null && EditorLightData.I.LoadedFile.CurrentValue == null) return;

            if (StageTimeController.I.IsPlaying.CurrentValue)
            {
                _scrollBar.SetValueWithoutNotify((float)(StageTimeController.StageTime / StageTimeController.I.SongClip.length));
            }
        }

        public void OnBarChangeValue(float value)
        {
            float time = StageTimeController.I.SongClip.length * value;
            float moveAmount = time - StageTimeController.StageTime;
            StageTimeController.I.MoveStageTime(moveAmount);
        }

        public void ChangeDivisionCount(int count)
        {
            _division = count;
        }

        private IReadOnlySongData GenerateSongPlayManager(int index)
        {
            if (SongPlayContext.I == null)
            {
                Debug.LogError("SongPlayManagerがありません");
                return null;
            }

            var songData = _songData.GetSongData(index);
            SongPlayContext.I.SetData(new SongSelectData(songData, _difficulty), null, 0);

            return songData;
        }
    }
}