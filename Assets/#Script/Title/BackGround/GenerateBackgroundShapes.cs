using System.Collections.Generic;
using Common;
using UnityEngine;
using UnityEngine.UI;

namespace Title.BackGround
{
    /// <summary>
    /// îwåiÇ≈ìÆÇ©Ç∑ê}å`ÇÃê∂ê¨
    /// </summary>
    public class GenerateBackgroundShapes : MonoBehaviour
    {
        [Header("éQè∆")]
        [SerializeField] private AnimateBackgroundShapes _animateShapes = new();
        [SerializeField] private Transform _parent;
        [SerializeField] private Sprite _shapeSprite;
        [SerializeField] private Image _prefab;
        [Header("ê∂ê¨")]
        [SerializeField] private Color[] _colors;
        [SerializeField,Range(0f,1f)] private float _alpha = 0.25f;
        [SerializeField] private FloatRange _size;
        [SerializeField] private FloatRange _moveSpeed;
        [SerializeField] private FloatRange _rotationSpeed;
        [SerializeField] private FloatRange _generateInterval;
        [SerializeField] private FloatRange _verticalGeneratePos;
        [SerializeField] private FloatRange _horizontalGeneratePos;

        private float _nextGenerateTime;
        private Queue<Image> _pool = new();

        private void Start()
        {
            _nextGenerateTime = 0f;
            _pool.Clear();
            _animateShapes.Initialize(x => ReleaseShape(x.Shape));
        }

        private void Update()
        {
            _animateShapes.Tick(Time.deltaTime);

            if (Time.time > _nextGenerateTime)
            {
                GenerateShapes();
                _nextGenerateTime = Time.time + _generateInterval.GetRandom();
            }
        }

        private void GenerateShapes()
        {
            var shape = GetShape();

            Color color = _colors[Random.Range(0, _colors.Length)];
            color.a = _alpha;
            shape.color = color;

            shape.transform.localScale = Vector3.one * _size.GetRandom();

            if (Random.value < 0.5f)
            {
                // ç∂äOë§
                shape.rectTransform.anchoredPosition = new Vector2(
                    _horizontalGeneratePos.Min,
                    _verticalGeneratePos.GetRandom());
            }
            else
            {
                // â∫äOë§
                shape.rectTransform.anchoredPosition = new Vector2(
                    _horizontalGeneratePos.GetRandom(),
                    _verticalGeneratePos.Min);
            }

            _animateShapes.AddShapes(new(shape, _rotationSpeed.GetRandom(), _moveSpeed.GetRandom()));
        }

        public void ReleaseShape(Image shape)
        {
            shape.gameObject.SetActive(false);
            _pool.Enqueue(shape);
        }

        private Image GetShape()
        {
            if (_pool.Count == 0)
            {
                Image clonedImage = Instantiate(_prefab, _parent);
                clonedImage.sprite = _shapeSprite;
                return clonedImage;
            }

            Image image = _pool.Dequeue();
            image.gameObject.SetActive(true);
            return image;
        }

    }
}