using UnityEngine;
namespace Common.UI
{
    public abstract class ScrollViewBase : MonoBehaviour
    {
        [SerializeField] private RectTransform _content;
        protected virtual void DeleteChild()
        {
            for (int i = _content.childCount - 1; i >= 0; i--)
            {
                Destroy(_content.GetChild(i).gameObject);
            }
        }
        public T InstantiateContent<T>(T prefab) where T : MonoBehaviour
        {
            return Instantiate(prefab,_content);
        }
    }
}
