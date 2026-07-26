using Title.SongSelect;
using UnityEngine;

namespace Common.UI
{
    /// <summary>
    /// ランクスライダーにランク基準ラインを表示させる
    /// </summary>
    public class RankLineView : MonoBehaviour
    {
        [SerializeField] private RectTransform _slider;
        [SerializeField] private RankDataBase _rankData;
        [SerializeField] private RectTransform[] _rankLines;


        private void Start()
        {
            RectTransform sliderRect = _slider;

            float left = sliderRect.rect.xMin;
            float right = sliderRect.rect.xMax;

            for (int i = 0; i < _rankData.RankValue.Count && i < _rankLines.Length; i++)
            {
                float t = Mathf.Clamp01((float)_rankData.RankValue[i].Rate);

                RectTransform rankLine = _rankLines[i];

                Vector2 pos = rankLine.anchoredPosition;
                pos.x = Mathf.Lerp(left, right, t);
                rankLine.anchoredPosition = pos;
            }
        }
    }
}