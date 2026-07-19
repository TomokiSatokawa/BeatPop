using InGame;
using R3;

namespace Common.BeatUpdate
{
    public class BeatUpdateManager : SingletonMonoBehaviour<BeatUpdateManager>
    {
        public static readonly BeatGroup FastBeatUpdate = new();
        public static readonly BeatGroup BeatUpdate = new();
        public static readonly BeatGroup LateBeatUpdate = new();

        private float _previousTime;

        private void Start()
        {
            //StageTimeControllerがファイルからBPMを読み込む時差を考慮
            StageTimeController.I.OnInitialized.Subscribe(_ =>
            {
                FastBeatUpdate.RefreshAll();
                BeatUpdate.RefreshAll();
                LateBeatUpdate.RefreshAll();
            }).AddTo(this);
        }
   
        private void Update()
        {
            float currentTime = StageTimeController.StageTime;

            if (currentTime < _previousTime)
            {
                FastBeatUpdate.UpdateAllNextTime();
                BeatUpdate.UpdateAllNextTime();
                LateBeatUpdate.UpdateAllNextTime();
            }

            FastBeatUpdate.Tick();
            BeatUpdate.Tick();
            LateBeatUpdate.Tick();

            _previousTime = currentTime;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy(); 
            
            FastBeatUpdate.Clear();
            BeatUpdate.Clear();
            LateBeatUpdate.Clear();
        }
    }
}