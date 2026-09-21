using Common.Effect;
using UnityEngine;

namespace InGame.Effect
{
    /// <summary>
    /// ホールド中のエフェクト
    /// </summary>
    public class HoldEffect : PoolObject
    {
        [SerializeField] private ParticleSystemController _particleController;

        public void Start()
        {
            _particleController.SetColor(InGameCustomColorData.I.GetNodeColor(PoolPrefabType.HoldNoteFill));
        }

        public void SetEmission(bool enable)
        {
            _particleController.SetEmission(enable);
        }
    }

}