using UnityEngine;

public class JudgementView : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;

    [SerializeField] private RectTransform[] _pos;

    public void ViewPrefab(IReadOnlyJudgementData judgeData, int lane)
    {
        //TODO:ObjectPool�ɕύX
        GameObject obj = Instantiate(judgeData.Prefab, _canvas.transform);

        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchoredPosition = _pos[lane].anchoredPosition;
    }
}
