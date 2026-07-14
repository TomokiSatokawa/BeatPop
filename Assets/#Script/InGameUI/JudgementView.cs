using UnityEngine;

public class JudgementView : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private JudgeUIControl _judgeUI;
    [SerializeField] private RectTransform[] _pos;

    public void ViewPrefab(IReadOnlyJudgementData judgeData, int lane)
    {
        //var judgeUI = PoolManager.I.Get<JudgeUIControl>(PoolPrefabType.JudgeUI,_canvas.transform);

        _judgeUI.Text.text = judgeData.Name.ToString();
        _judgeUI.Text.color = judgeData.TextColor;
        //_judgeUI.Text.rectTransform.anchoredPosition = _pos[lane].anchoredPosition;
        _judgeUI.OnPlay();
    }
}
