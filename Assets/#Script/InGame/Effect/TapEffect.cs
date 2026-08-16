using UnityEngine;

namespace InGame.Effect
{

    public class TapEffect : AutoRelease
    {
        [SerializeField] private ParticleSystem[] _particleSystem;

        public void SetColor(Color color)
        {
            foreach (var particle in _particleSystem)
            {
                var main = particle.main;

                Color startColor = main.startColor.color;
                startColor.r = color.r;
                startColor.g = color.g;
                startColor.b = color.b;

                main.startColor = startColor;
            }
        }
    }
}