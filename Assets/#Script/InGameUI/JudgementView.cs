using UnityEngine;

public class JudgementView : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;

    [SerializeField] private RectTransform[] _pos;

    public void ViewPrefab(IReadOnlyJudgementData judgeData, int lane)
    {
       var judgeUI = PoolManager.I.Get<JudgeUIControl>(PoolPrefabType.JudgeUI,_canvas.transform);

        judgeUI.Text.text = judgeData.Name.ToString();
        judgeUI.Text.color = judgeData.TextColor;
        judgeUI.Text.rectTransform.anchoredPosition = _pos[lane].anchoredPosition;
    }
}
