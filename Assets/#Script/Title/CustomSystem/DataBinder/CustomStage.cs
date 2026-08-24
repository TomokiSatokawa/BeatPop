using UnityEngine;

namespace Title.Custom
{

    public class CustomStage : CustomDataBinder<CustomStagePattern>
    {
        [SerializeField] private CustomStageData _customStageData;
        [SerializeField] private ValueStepper _nodeSpeed;
        [SerializeField] private ValueStepper _judgeOffset;

        public override CustomStagePattern GetCustom()
        {
            return new CustomStagePattern()
            {
                NodeSpeed = ValueStepper.GetInterpolationFactor(_nodeSpeed, _nodeSpeed.Value),
                JudgeOffset = ValueStepper.GetInterpolationFactor(_judgeOffset, _judgeOffset.Value)
            };
        }

        public override void OnDefault()
        {
            SetCustom(new CustomStagePattern()
            {
                NodeSpeed = ValueStepper.GetInterpolationFactor(_nodeSpeed,_nodeSpeed.StartValue),
                JudgeOffset = ValueStepper.GetInterpolationFactor(_judgeOffset, _judgeOffset.StartValue)
            });
        }

        public override void SetCustom(CustomStagePattern data)
        {
            _nodeSpeed.SetInterpolationFactor(data.NodeSpeed);
            _judgeOffset.SetInterpolationFactor(data.JudgeOffset);
        }
    }

    [System.Serializable]
    public struct CustomStagePattern
    {
        public float NodeSpeed;
        public float JudgeOffset;
    }

    public enum CustomStageType
    {
        Node, Judge
    }
}