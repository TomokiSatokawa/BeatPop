using System.Collections.Generic;
using System.Linq;
using Editor.UI;
using InGame.Stage;
using R3;
using UnityEngine;

public class EditorLightGenerator : EditorGeneratorBase<EditorLightGenerator>
{
    [SerializeField] private RectTransform _content;
    [SerializeField] private RectTransform[] _lean;
    [SerializeField] private PatternSettingsControl _settingsControl;

    private readonly List<EditorLightNode> _clonedNode = new();

    private void Start()
    {
        EditorLightData.I.OnRemove.Subscribe(x => RemoveNode((x.time,x.channel))).AddTo(this);    
    }

    protected override void UpdateInRange(double minTime, double maxTime)
    {
        List<EditorLightNode> removeNode = new();

        foreach (var node in _clonedNode)
        {
            if (node.Time < minTime || node.Time > maxTime)
            {
                node.Release();
                removeNode.Add(node);
            }
        }

        foreach (var remove in removeNode)
        {
            _clonedNode.Remove(remove);
        }

        foreach(var data in EditorLightData.I.LightData)
        {
            NodeRendering(data, PoolPrefabType.EditorLightNode);
        }


        void NodeRendering(LightPatternBaseData data, PoolPrefabType editorNote)
        {
            if (data.Time < minTime || data.Time > maxTime)
                return;

            if (_clonedNode.Exists(x => x.PatternBaseData == data)) return;

            var newNode = PoolManager.I.Get<EditorLightNode>(editorNote, _content);

            newNode.SetData(data, _lean[data.Channel], _content, _settingsControl.ShowSettings);
            newNode.Time = data.Time;

            switch (data.GetType().Name)
            {
                case nameof(LightPatternBaseData):
                    break;
            }

            _clonedNode.Add(newNode);
        }
    }
    public void UpdateData(LightPatternBaseData old ,LightPatternBaseData newData)
    {
        var target = _clonedNode.Where(x => x.PatternBaseData == old).FirstOrDefault();
        if (target == null) return;

        target.ChangeData(newData);
    }


    public void RemoveNode((float time, float channel) data)
    {
        var poolObject = _clonedNode.Where(x => x.PatternBaseData.Channel == data.channel && Mathf.Abs((float)x.Time - data.time) < 0.001f).FirstOrDefault();
        poolObject.Release();
        _clonedNode.Remove(poolObject);
    }
}
