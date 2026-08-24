using UnityEngine;

namespace Title.Custom
{

    [CreateAssetMenu(fileName = "CustomStageData", menuName = "Scriptable Objects/CustomStageData")]
    public class CustomStageData : ScriptableObject
    {
        [SerializeField] private float _defaultNodeSpeedValue;
        [SerializeField] private float _maxNodeSpeedSecond;
        [SerializeField] private float _minNodeSpeedSecond;

        public CustomStagePattern GetDefault()
        {
            var result  = new CustomStagePattern();

            result.NodeSpeed = _defaultNodeSpeedValue;
            result.JudgeOffset = 0;

            return result;
        }

        public float GetSpeedSecond(float speedValue)
        {
            return Mathf.Lerp(_minNodeSpeedSecond, _maxNodeSpeedSecond, speedValue);
        }
    }
}