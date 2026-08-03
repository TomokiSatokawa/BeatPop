using UnityEngine;

namespace Common.Effect
{
    /// <summary>
    /// ParticleSystemの制御
    /// </summary>
    public class ParticleSystemController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] _particleSystems;

        /// <summary>
        /// すべてのParticleSystemのEmissionの有効・無効にする
        /// </summary>
        public void SetEmission(bool enable)
        {
            foreach (var particle in _particleSystems)
            {
                var emission = particle.emission;
                emission.enabled = enable;
            }
        }

        /// <summary>
        /// すべてのParticleSystemの再生を開始
        /// </summary>
        public void Play()
        {
            foreach (var particle in _particleSystems)
            {
                particle.Play();
            }
        }

        /// <summary>
        /// すべてのParticleSystemの再生を停止
        /// </summary>
        /// /// <param name="clear">
        /// trueの場合はパーティクルを消去して停止し
        /// falseの場合は発生のみ停止
        /// </param>
        public void Stop(bool clear = true)
        {
            var stopBehavior = clear
                ? ParticleSystemStopBehavior.StopEmittingAndClear
                : ParticleSystemStopBehavior.StopEmitting;

            foreach (var particle in _particleSystems)
            {
                particle.Stop(true, stopBehavior);
            }
        }

        /// <summary>
        /// すべてのParticleSystemを消去します。
        /// </summary>
        public void Clear()
        {
            foreach (var particle in _particleSystems)
            {
                particle.Clear();
            }
        }
    }
}