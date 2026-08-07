using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Result.UI
{

    public class LevelXPView : MonoBehaviour
    {
        //TODO:Level•ÊSO‚É‚·‚é
        private const int LevelUpXP = 1000;

        [SerializeField] private TextMeshProUGUI _rankText;
        [SerializeField] private TextMeshProUGUI _addXpText;
        [SerializeField] private Image _xpSlider;
        [SerializeField] private float _duration;
        public void PlayAnimation(int level, int xp, int addXp)
        {
            _addXpText.text = $"x{addXp}XP";
        }
    }
}