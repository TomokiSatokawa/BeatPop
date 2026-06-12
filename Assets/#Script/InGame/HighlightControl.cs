using UnityEngine;

namespace InGame.Field
{
    public class HighlightControl : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private float _alpha;
        [SerializeField] private float _duration;
    }
}