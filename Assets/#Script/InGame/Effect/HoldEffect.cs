using UnityEngine;

namespace InGame.Effect
{
    /// <summary>
    /// ホールド中のエフェクト
    /// </summary>
    public class HoldEffect : PoolObject
    {
        [SerializeField] private ParticleSystem[] _particleSystems;

        public void SetEmission(bool b)
        {
            foreach (var p in _particleSystems)
            {
                var emission = p.emission;
                emission.enabled = b;
            }
        }
    }

}