using DG.Tweening;
using UnityEngine;

namespace InGame.Effect
{
    /// <summary>
    /// レーンクリック時のハイライト演出
    /// </summary>
    public class LaneClickEffect : MonoBehaviour
    {
        [System.Serializable]
        public class LaneView
        {
            public SpriteRenderer Highlight;
            public Vector3 EffectPosition;
        }

        [SerializeField] private LaneView[] _laneHighlight;
        [SerializeField] private float _startAlpha;
        [SerializeField] private float _emptyClickDuration;
        [SerializeField] private float _nodeClickAlphaMultiplier = 0.5f;

        private void Awake()
        {
            foreach(var lane in _laneHighlight)
            {
                lane.Highlight.DOFade(0, 0);
            }
        }
        public void PlayLaneHighlight(int lane,float alphaMultiplier = 1)
        {
            if (!IsValidLane(lane))
                return;

            Color color = Color.white;
            color.a = _startAlpha * alphaMultiplier;

            var highlight = _laneHighlight[lane].Highlight;
            highlight.DOKill();
            highlight.color = color;
            highlight.DOFade(0, _emptyClickDuration).SetEase(Ease.OutQuad);
        }
        public void PlayNodeClickEffect(int lane)
        {
            if (!IsValidLane(lane))
                return;

            var effect = PoolManager.I.Get<PoolObject>(PoolPrefabType.LaneEffect);
            effect.transform.position = _laneHighlight[lane].EffectPosition;
            PlayLaneHighlight(lane, _nodeClickAlphaMultiplier);
        }

        private bool IsValidLane(int lane)
        {
            if ((uint)lane < (uint)_laneHighlight.Length)
                return true;

            Debug.LogError($"[LaneClickEffect] Invalid lane : {lane}");
            return false;
        }
    }

}