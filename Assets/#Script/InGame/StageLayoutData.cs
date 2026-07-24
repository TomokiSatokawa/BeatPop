using UnityEngine;

[CreateAssetMenu(fileName = "StageLayoutData", menuName = "Scriptable Objects/StageLayoutData")]
public class StageLayoutData : ScriptableObject
{
    [SerializeField] private float _goalPos;
    [SerializeField] private float _deletePos;

    public float GoalPos => _goalPos;
    public float DeletePos => _deletePos;
}
