using System;
using Result.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace InGame.UI
{
    /// <summary>
    /// リザルト結果に応じて適切なクリアアニメーションを呼び出す
    /// </summary>
    public class ClearAnimationSwitcher : MonoBehaviour
    {
        [SerializeField] private ClearAnimation _liveClear;
        [SerializeField] private ClearAnimation _fullCombo;
        [SerializeField] private ClearAnimation _allPerfect;

        public void Play()
        {
            Play((ResultType)Random.Range(0, 3));
        }

        public void Play(ResultType type, Action callback = null)
        {
            _liveClear.SetActive(false);
            _fullCombo.SetActive(false);
            _allPerfect.SetActive(false);
            switch (type)
            {
                case ResultType.Clear:
                    _liveClear.StartAnimation(callback);
                    break;
                case ResultType.FullCombo:
                    _fullCombo.StartAnimation(callback);
                    break;
                case ResultType.AllPerfect:
                    _allPerfect.StartAnimation(callback);
                    break;
            }
        }
    }
}