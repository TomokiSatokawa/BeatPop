using System;
using UnityEngine;
namespace InGame.Stage
{
    /// <summary>
    /// AudioSpectrum‚ð•\Ž¦‚·‚é
    /// </summary>
    public class AudioSpectrumScaler : MonoBehaviour
    {
        [SerializeField] private AudioSpectrum _spectrum;
        [SerializeField] private Transform[] _cubes;
        [SerializeField] private Gradient _gradient;
        [SerializeField] private float _scale = 10f;
        [SerializeField] private float _hiddenScale;
        [SerializeField] private bool _mirror = true;
        [SerializeField] private bool _reverse = false;

        private void Awake()
        {
            Array.Sort(_cubes, (a, b) => a.position.x.CompareTo(b.position.x));

            UpdateGradient();
        }

        private void Update()
        {
            if (_spectrum.Levels == null || _spectrum.Levels.Length == 0)
                return;

            if (_mirror)
            {
                UpdateMirror();
            }
            else
            {
                UpdateNormal();
            }
        }

        private void UpdateMirror()
        {
            int half = _cubes.Length / 2;

            for (int i = 0; i < half; i++)
            {
                int index = _reverse ? half - 1 - i : i;

                float value = GetInterpolatedLevel(index, half);

                SetScale(_cubes[i], value);
                SetScale(_cubes[_cubes.Length - 1 - i], value);
            }

            // Šï”ŒÂ‚Ìê‡A’†‰›‚ðˆ—
            if (_cubes.Length % 2 == 1)
            {
                SetScale(_cubes[half], GetInterpolatedLevel(half, _cubes.Length));
            }
        }

        private void UpdateNormal()
        {
            for (int i = 0; i < _cubes.Length; i++)
            {
                int levelIndex = i;
                if (_reverse)
                {
                    levelIndex = _cubes.Length - 1 - i;
                }

                float value = GetInterpolatedLevel(levelIndex, _cubes.Length);
                SetScale(_cubes[i], value);
            }
        }

        private float GetInterpolatedLevel(int index, int count)
        {
            if (count <= 1)
                return _spectrum.Levels[0];

            float t = (float)index / (count - 1);

            float levelPosition = t * (_spectrum.Levels.Length - 1);

            int lowerIndex = Mathf.FloorToInt(levelPosition);
            int upperIndex = Mathf.Clamp(lowerIndex + 1, 0, _spectrum.Levels.Length - 1);

            float lerp = levelPosition - lowerIndex;

            return Mathf.Lerp(_spectrum.Levels[lowerIndex], _spectrum.Levels[upperIndex], lerp);
        }

        private void SetScale(Transform cube, float value)
        {
            var localScale = cube.localScale;
            localScale.y = value * _scale;
            cube.localScale = localScale;

            cube.gameObject.SetActive(localScale.y > _hiddenScale);
        }
        private void UpdateGradient()
        {
            if (_gradient == null)
                return;

            if (_mirror)
            {
                int half = _cubes.Length / 2;

                for (int i = 0; i < half; i++)
                {
                    float t = half <= 1
                        ? 0f
                        : (float)i / (half - 1);

                    Color color = _gradient.Evaluate(t);

                    SetColor(_cubes[i], color);
                    SetColor(_cubes[_cubes.Length - 1 - i], color);
                }

                // Šï”ŒÂ‚Ìê‡’†‰›
                if (_cubes.Length % 2 == 1)
                {
                    SetColor(_cubes[half], _gradient.Evaluate(1f));
                }
            }
            else
            {
                for (int i = 0; i < _cubes.Length; i++)
                {
                    float t = _cubes.Length <= 1
                        ? 0f
                        : (float)i / (_cubes.Length - 1);

                    SetColor(_cubes[i], _gradient.Evaluate(t));
                }
            }
        }

        private void SetColor(Transform cube, Color color)
        {
            var renderer = cube.GetComponent<Renderer>();

            if (renderer == null)
                return;

            renderer.material.color = color;
        }
    }
}