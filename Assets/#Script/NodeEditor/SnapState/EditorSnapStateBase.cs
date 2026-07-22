using UnityEngine;

namespace Editor
{
    /// <summary>
    /// スナップステートのベースクラス
    /// </summary>
    [System.Serializable]
    public abstract class EditorSnapStateBase
    {
        [SerializeField] private Sprite _pointerImage;
        [SerializeField] private Color _pointerColor = Color.black;

        public Sprite PointerImage => _pointerImage;
        public Color PointerColor => _pointerColor;

        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void OnCreate(int laneIndex, double noteTime);
        public abstract void OnDelete(int laneIndex, double noteTime);
    }
}