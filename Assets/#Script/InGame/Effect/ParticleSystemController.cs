using UnityEngine;

namespace Common.Effect
{
    /// <summary>
    /// ParticleSystemÇÃêßå‰
    /// </summary>
    public class ParticleSystemController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] _particleSystems;

        public void SetEmission(bool enable)
        {
            foreach (var particle in _particleSystems)
            {
                var emission = particle.emission;
                emission.enabled = enable;
            }
        }

        public void Play()
        {
            foreach (var particle in _particleSystems)
            {
                particle.Play();
            }
        }

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

        public void Clear()
        {
            foreach (var particle in _particleSystems)
            {
                particle.Clear();
            }
        }
    }
}