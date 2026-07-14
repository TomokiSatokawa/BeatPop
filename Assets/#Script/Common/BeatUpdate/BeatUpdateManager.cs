using InGame.UI;
using R3;

namespace Common.BeatUpdate
{
    public class BeatUpdateManager : SingletonMonoBehaviour<BeatUpdateManager>
    {
        public static readonly BeatGroup FastBeatUpdate = new();
        public static readonly BeatGroup BeatUpdate = new();
        public static readonly BeatGroup LateBeatUpdate = new();
        private float _previousTime;
        public void Start()
        {
            StageTimeController.I.OnInitialized.Subscribe(_ =>
            {
                FastBeatUpdate.UpdateConstants();
                BeatUpdate.UpdateConstants();
                LateBeatUpdate.UpdateConstants();
            });
        }
   
        private void Update()
        {
            float current = StageTimeController.StageTime;

            if (current < _previousTime)
            {
                FastBeatUpdate.UpdateNextTime();
                BeatUpdate.UpdateNextTime();
                LateBeatUpdate.UpdateNextTime();
            }

            FastBeatUpdate.Tick();
            BeatUpdate.Tick();
            LateBeatUpdate.Tick();

            _previousTime = current;
        }
    }
}