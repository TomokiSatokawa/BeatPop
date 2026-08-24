using System;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Title.BackGround
{
    /// <summary>
    /// ê∂ê¨ÇµÇΩê}å`ÇìÆÇ©Ç∑
    /// </summary>
    [System.Serializable]
    public class AnimateBackgroundShapes
    {
        [SerializeField] private FloatRange _horizontalMovePos;
        [SerializeField] private FloatRange _verticalMovePos;
        [SerializeField, Range(0, 360)] private float _directionAngle;

        private Vector2 _moveDirection;
        private readonly List<ShapeAnimationData> _removeList = new();
        private readonly List<ShapeAnimationData> _shapes = new();
        private Action<ShapeAnimationData> _removeAction;

        public void Initialize(Action<ShapeAnimationData> removeAction)
        {
            _removeAction = removeAction;

          
        }

        public void Tick(float deltaTime)
        {
            float radian = _directionAngle * Mathf.Deg2Rad;
            _moveDirection = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
            _removeList.Clear();

            foreach (var shape in _shapes)
            {
                var rect = shape.Shape.rectTransform;

                rect.anchoredPosition += _moveDirection * shape.MoveSpeed * deltaTime;
                rect.localRotation *= Quaternion.Euler(0, 0, shape.RotationSpeed * deltaTime);

                //îÕàÕì‡Ç©ÇÁèoÇΩ
                if (!_verticalMovePos.Contains(rect.anchoredPosition.x)
                    || !_horizontalMovePos.Contains(rect.anchoredPosition.y))
                {
                    _removeAction?.Invoke(shape);
                    _removeList.Add(shape);
                }
            }

            foreach (var shape in _removeList)
                _shapes.Remove(shape);
        }

        public void AddShapes(ShapeAnimationData animationData)
        {
            _shapes.Add(animationData);
        }
    }
}