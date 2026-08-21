using UnityEngine;

namespace Title.Custom
{

    [CreateAssetMenu(fileName = "CustomStageData", menuName = "Scriptable Objects/CustomStageData")]
    public class CustomStageData : ScriptableObject
    {
        [SerializeField] private float _defaultNodeSpeedValue;

        public CustomStagePattern GetDefault()
        {
            var result  = new CustomStagePattern();

            result.NodeSpeed = _defaultNodeSpeedValue;
            result.JudgeOffset = 0;

            return result;
        }
    }
}