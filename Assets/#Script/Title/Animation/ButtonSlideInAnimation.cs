using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSlideInAnimation : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Button[] _buttons;

    [Header("Move")]
    [SerializeField] private float _startPos = 100f;
    [SerializeField] private float _duration = 0.45f;
    [SerializeField] private float _interval = 0.06f;
    [SerializeField] private Direction _moveStartDirection = Direction.Down;

    [Header("Scale")]
    [SerializeField] private float _startScale = 0.9f;

    [Header("Fade")]
    [SerializeField] private bool _useFade = true;

    [Header("Ease")]
    [SerializeField] private Ease _ease = Ease.OutBack;

    [Header("Punch")]
    [SerializeField] private bool _usePunch = true;
    [SerializeField] private float _punchPower = 0.05f;
    [SerializeField] private float _punchDuration = 0.2f;

    private Sequence _animation;

    private void Start()
    {
        CreateAnimation();
    }

    private void CreateAnimation()
    {
        _animation = DOTween.Sequence()
                    .SetAutoKill(false)
                    .Pause();

        Button[] buttons = _moveStartDirection == Direction.Up
            ? _buttons.Reverse().ToArray()
            : _buttons;

        Vector2 direction = _moveStartDirection switch
        {
            Direction.Up => Vector2.up,
            Direction.Down => Vector2.down,
            Direction.Left => Vector2.left,
            Direction.Right => Vector2.right,
            _ => Vector2.zero
        };

        for (int i = 0; i < buttons.Length; i++)
        {
            Button button = buttons[i];
            RectTransform rect = button.targetGraphic.rectTransform;

            Vector2 targetPos = rect.anchoredPosition;
            Vector2 startPos = targetPos + direction * _startPos;

            button.interactable = false;

            _animation.InsertCallback(i * _interval, () =>
            {
                button.interactable = false;
            });

            // Position
            _animation.Insert(
                i * _interval,
                rect.DOAnchorPos(targetPos, _duration)
                    .From(startPos)
                    .SetEase(_ease)
            );

            // Scale
            _animation.Insert(
                i * _interval,
                rect.DOScale(Vector3.one, _duration)
                    .From(Vector3.one * _startScale)
                    .SetEase(_ease)
            );

            // Fade
            if (_useFade)
            {

                _animation.Insert(
                    i * _interval,
                    button.image.DOFade(1f, _duration)
                        .From(0f)
                );
            }

            // Finish
            _animation.InsertCallback(i * _interval + _duration, () =>
            {
                button.interactable = true;

                if (_usePunch)
                {
                    rect.DOPunchScale(
                        Vector3.one * _punchPower,
                        _punchDuration,
                        8,
                        0.8f);
                }
            });
        }
    }

    public void PlayAnimation()
    {
        _animation.Restart();
    }

    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}