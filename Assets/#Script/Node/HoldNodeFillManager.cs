using System.Collections.Generic;
using InGame.Node;
using InGame.UI;
using Input;
using UnityEngine;

public class HoldNodeFillManager : MonoBehaviour
{
    [SerializeField] private Transform[] _lane;
    [SerializeField] private float _cloneZ;
    [SerializeField] private float _tapZ;
    [SerializeField] private float _deleteZ;

    private Dictionary<NodeData, FillData> _activeFillData = new();
    public void AddClone(NodeData start, NodeData end, PoolObject startObject)
    {
        if (_activeFillData.ContainsKey(end)) return;
        var fillData = new FillData(start, end, _lane[start.Lane], _cloneZ, _deleteZ,_tapZ);
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
            if (fillData.StartNode.Lane == lane && fillData.StartNode.Time <= StageTimeController.StageTime)
            {
                return true;
            }
        }
        return false;
    }
    public void Update()
    {
        foreach (var fillDataPir in _activeFillData)
        {
            fillDataPir.Value.Tick(Time.deltaTime);
        }
    }

    public void OnDestroy()
    {
        foreach(var  fillData in _activeFillData.Values)
        {
            fillData.Remove();
        }
        _activeFillData.Clear();
    }
    public class FillData
    {
        private PoolObject _fillObject;
        public NodeData StartNode { get; private set; }
        public NodeData _endNode;
        public PoolObject _startObject;
        public PoolObject EndObject { get; private set; }

        private readonly Transform _lane;
        private readonly float _clonePosZ;
        private readonly float _tapPosZ;
        private readonly float _deletePosZ;
        private readonly HoldEffect _effect;
        public FillData(NodeData start, NodeData end, Transform lane, float cloneZ, float deleteZ,float tapZ)
        {
            _fillObject = PoolManager.I.Get<PoolObject>(PoolPrefabType.HoldNoteFill);
            _effect = PoolManager.I.Get<HoldEffect>(PoolPrefabType.HoldFillEffect);
            StartNode = start;
            _endNode = end;
            _lane = lane;
            _clonePosZ = cloneZ;
            _tapPosZ = tapZ;
            _deletePosZ = deleteZ;
            _fillObject.gameObject.SetActive(false);

            _effect.transform.position = new Vector3(_lane.transform.position.x, _lane.transform.position.y, _tapPosZ);
        }
        public void Tick(float deltaTime)
        {
            Vector3 start = _lane.transform.position;
            if (_startObject?.IsPoolActive ?? false)
            {
                start.z = _startObject.transform.position.z;
            }
            else
            {
                start.z = _tapPosZ;
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

            bool isInput = false;
            if(StartNode.Lane == 0)
            {
                isInput = InputManager.LeftLane.CurrentValue;
            }
            else
            {
                isInput = InputManager.RightLane.CurrentValue;
            }

            _effect.SetEmission(isInput);
        }
        private void SetFill(Transform fill, Vector3 startPos, Vector3 endPos)
        {
            float length = Mathf.Abs(endPos.z - startPos.z);

            fill.gameObject.SetActive(length > 0.001f);

            if (length <= 0.001f)
                return;

            fill.position = (startPos + endPos) * 0.5f;

            Vector3 scale = fill.localScale;
            scale.z = length;
            fill.localScale = scale;
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
        }
    }
}