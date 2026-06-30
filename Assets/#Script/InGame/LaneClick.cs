using DG.Tweening;
using UnityEngine;

namespace InGame.Node
{
    public class LaneClick : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer[] _laneHighlight;
        [SerializeField] private Vector3[] _effectPos;
        [SerializeField] private float _startAlpha;
        [SerializeField] private float _duration;

        public void EmptyClick(int lane,float alphaMultiplier = 1)
        {
            Debug.Log(_laneHighlight[lane]);
            Color color = Color.white;
            color.a = _startAlpha * alphaMultiplier;
            _laneHighlight[lane].DOKill();
            _laneHighlight[lane].color = color;
            _laneHighlight[lane].DOFade(0, _duration);
        }
        public void NodeClick(int lane)
        {
            var effect = PoolManager.I.Get<PoolObject>(PoolPrefabType.LaneEffect);
            effect.transform.position = _effectPos[lane];
            EmptyClick(lane, 0.5f);
        }
    }

}