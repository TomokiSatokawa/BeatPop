using System.Collections.Generic;
using InGame.Effect;
using Input;
using Sound;
using UnityEngine;

namespace InGame.Node
{
    /// <summary>
    /// ロングノーツの長押し部分を生成する
    /// </summary>
    public class HoldNodeFillManager : MonoBehaviour
    {
        [SerializeField] private Transform[] _lane;
        [SerializeField]private JudgementTable _judgementTable;

        private Dictionary<NodeData, FillData> _activeFillData = new();

        public void AddClone(NodeData start, NodeData end, PoolObject startObject)
        {
            if (_activeFillData.ContainsKey(end)) return;

            float clone = StageContext.I.GetClonePos(0).z;
            float goal = StageContext.I.StageLayout.GoalPos;
            float delete = StageContext.I.StageLayout.DeletePos;

            var fillData = new FillData(start, end, _lane[start.Lane], clone, goal, delete);
            fillData.SetNodeObject(start: startObject);
            _activeFillData.Add(end, fillData);
        }

        public void SetEndObject(NodeData end, PoolObject endObject)
        {
            if (!_activeFillData.TryGetValue(end, out var fillData)) return;

            fillData.SetNodeObject(end: endObject);
        }

        public void DeleteFill(NodeData end)
        {
            if (!_activeFillData.TryGetValue(end, out var fillData)) return;

            fillData.Remove();
            _activeFillData.Remove(end);
        }

        public bool HasFill(int lane)
        {
            foreach (var fillData in _activeFillData.Values)
            {
                if (fillData.StartNode.Lane == lane && fillData.StartNode.Time + _judgementTable.ToleranceValue <= StageTimeController.StageTime)
                {
                    return true;
                }
            }
            return false;
        }

        private void Update()
        {
            foreach (var fillDataPir in _activeFillData)
            {
                fillDataPir.Value.Tick(Time.deltaTime);
            }
        }

        /// <summary>
        /// 1つのロングノーツを管理する
        /// </summary>
        public class FillData
        {
            private const float MinFillLength  = 0.001f;
            private PoolObject _fillObject;
            public NodeData StartNode { get; private set; }
            public PoolObject EndObject { get; private set; }

            private NodeData _endNode;
            private PoolObject _startObject;

            private bool _isInput;
            private bool _isPreviousInput;

            private readonly Transform _lane;
            private readonly float _clonePosZ;
            private readonly float _tapPosZ;
            private readonly float _deletePosZ;
            private readonly HoldEffect _effect;

            public FillData(NodeData start, NodeData end, Transform lane, float cloneZ, float tapZ,float deletePos)
            {
                _fillObject = PoolManager.I.Get<PoolObject>(PoolPrefabType.HoldNoteFill);
                _effect = PoolManager.I.Get<HoldEffect>(PoolPrefabType.HoldFillEffect);
                StartNode = start;
                _endNode = end;
                _lane = lane;
                _clonePosZ = cloneZ;
                _tapPosZ = tapZ;
                _deletePosZ = deletePos;
                _fillObject.gameObject.SetActive(false);

                _effect.transform.position = new Vector3(_lane.transform.position.x, _lane.transform.position.y, _tapPosZ);
            }

            public void Tick(float deltaTime)
            {
                UpdateFill();
                UpdateInput();
                UpdateSound();

                _effect.SetEmission(_isInput);
            }

            public void SetNodeObject(PoolObject start = null, PoolObject end = null)
            {
                if (start != null)
                    _startObject = start;

                if (end != null)
                    EndObject = end;

                Tick(0);
            }

            public void Remove()
            {
                _fillObject.Release();
                _effect.Release();
                SoundManager.LaneSE[StartNode.Lane].StopBGM();
            }

            private void UpdateSound()
            {
                if (_isInput == _isPreviousInput) return;

                if (_isInput)
                {
                    SoundManager.LaneSE[StartNode.Lane].PlayBGM(SESoundType.Hold1, isLoop: true);
                }
                else
                {
                    SoundManager.LaneSE[StartNode.Lane].StopBGM();
                }
            }

            private void UpdateInput()
            {
                _isPreviousInput = _isInput;
                _isInput = false;

                if (StartNode.Time <= StageTimeController.StageTime)
                {
                    if (StartNode.Lane == 0)
                    {
                        _isInput = InputManager.LeftLane.CurrentValue;
                    }
                    else
                    {
                        _isInput = InputManager.RightLane.CurrentValue;
                    }
                }
            }

            private void UpdateFill()
            {
                Vector3 start = _lane.transform.position;

                if (_startObject?.IsPoolActive ?? false)
                {
                    start.z = _startObject.transform.position.z;
                }
                else
                {
                    start.z = _isInput ? _tapPosZ: _deletePosZ;
                    _startObject = null;
                }

                Vector3 end = _lane.transform.position;
                if (EndObject?.IsPoolActive ?? false)
                {
                    end.z = EndObject.transform.position.z;
                }
                else
                {
                    end.z = _clonePosZ;
                    EndObject = null;
                }
                SetFill(_fillObject.transform, start, end);
            }

            private void SetFill(Transform fill, Vector3 startPos, Vector3 endPos)
            {
                float length = Mathf.Abs(endPos.z - startPos.z);

                fill.gameObject.SetActive(length > MinFillLength );

                if (length <= 0.001f)
                    return;

                fill.position = (startPos + endPos) * 0.5f;

                Vector3 scale = fill.localScale;
                scale.z = length;
                fill.localScale = scale;
            }
        }
    }
}