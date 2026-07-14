using DG.Tweening;
using UnityEngine;

namespace InGame.Node
{
    public class LaneClick : MonoBehaviour
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

        private void Start()
        {
            foreach(var lane in _laneHighlight)
            {
                lane.Highlight.DOFade(0, 0);
            }
        }
        public void PlayLaneHighlight(int lane,float alphaMultiplier = 1)
        {
            if ((uint)lane >= (uint)_laneHighlight.Length)
            {
                Debug.LogError($"Invalid lane : {lane}");
                return;
            }

            Color color = Color.white;
            color.a = _startAlpha * alphaMultiplier;

            _laneHighlight[lane].Highlight.DOKill();
            _laneHighlight[lane].Highlight.color = color;
            _laneHighlight[lane].Highlight.DOFade(0, _emptyClickDuration).SetEase(Ease.OutQuad);
        }
        public void PlayNodeClickEffect(int lane)
        {
            if ((uint)lane >= (uint)_laneHighlight.Length)
            {
                Debug.LogError($"Invalid lane : {lane}");
                return;
            }

            var effect = PoolManager.I.Get<PoolObject>(PoolPrefabType.LaneEffect);
            effect.transform.position = _laneHighlight[lane].EffectPosition;
            PlayLaneHighlight(lane, _nodeClickAlphaMultiplier);
        }
    }

}