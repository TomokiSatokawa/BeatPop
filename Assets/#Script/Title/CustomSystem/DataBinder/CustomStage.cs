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
            return _customStageData.GetDefault();
        }

        public override void OnDefault()
        {
            SetCustom(new CustomStagePattern()
            {
                NodeSpeed = _nodeSpeed.StartValue,
                JudgeOffset = _judgeOffset.StartValue
            });
        }

        public override void SetCustom(CustomStagePattern data)
        {
            _nodeSpeed.SetValue(data.NodeSpeed);
            _judgeOffset.SetValue(data.JudgeOffset);
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