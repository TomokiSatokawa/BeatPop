using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using InGame.Node;
using InGame.UI;
using UnityEngine;

public static class NodeDataSerializer
{
    public const string Version = "2.0";
    public static string SerializeCSV(List<NodeData> nodeDatas)
    {
        float bpmInterval = 60f / StageTimeController.I.BPM;

        List<string> lines = new() { "NodeID,Lane,MeasureNumber,Beat,Type" };

        foreach (var nodeData in nodeDatas)
        {
            float totalBeat = nodeData.Time / bpmInterval;

            int measureNumber = (int)(totalBeat / 4f) + 1;
            float beat = (totalBeat % 4f) + 1f;

            int type = nodeData.PrefabType switch
            {
                PoolPrefabType.Line => -1,
                _ => 0
            };

            lines.Add(
                $"{nodeData.NodeID}," +
                $"{nodeData.Lane}," +
                $"{measureNumber}," +
                $"{beat}," +
                $"{type}"
            );
        }

        return string.Join("\n", lines);
    }

    public static List<NodeData> DeserializeCSV(string csvData)
    {
        var _nodeDatas = new List<NodeData>();
        _nodeDatas.Clear();

        float bpmInterval = 60f / StageTimeController.I.BPM;

        string[] lines = csvData.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            var value = lines[i].Split(',');

            NodeData nodeData = new();

            nodeData.NodeID = int.Parse(value[0]);
            nodeData.Lane = int.Parse(value[1]);

            int measureNumber = int.Parse(value[2]);
            float timeBeat = float.Parse(value[3]);

            float totalBeat = ((measureNumber - 1) * 4) + (timeBeat - 1);

            nodeData.Time = totalBeat * bpmInterval;

            nodeData.PrefabType = ChangePrefabType(int.Parse(value[4]));

            _nodeDatas.Add(nodeData);
        }
        _nodeDatas = _nodeDatas.OrderBy(x => x.Time).ToList();

        return _nodeDatas;
    }

    private static PoolPrefabType ChangePrefabType(int type)
    {
        switch (type)
        {
            case 0:
                return PoolPrefabType.NormalNote;
            case -1:
                return PoolPrefabType.Line;

        }
        return PoolPrefabType.NormalNote;
    }

    public static async UniTask<NodeSaveData> DesrializeJson(string json)
    {
        await UniTask.Yield();
        if (string.IsNullOrWhiteSpace(json))
            return null;

        try
        {
            var data = JsonUtility.FromJson<NodeSaveData>(json);
            if (data.DataVersion != Version)
            {
                Debug.LogWarning("データのバージョンが違います");
            }
            data.Nodes = data.Nodes.OrderBy(x => x.Time).ToList();
            data.Section.Sort();
            return data;
        }
        catch
        {
            return null;
        }
    }
    public static string SerializeJson(List<NodeData> nodes, List<float> sectionTime ,float bpm, int songIndex, string songName = "null")
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            var node = nodes[i];
            node.NodeID = i;
            nodes[i] = node;
        }

        var data = new NodeSaveData();

        data.DataVersion = Version;
        data.BPM = bpm;
        data.SoundIndex = songIndex;
        data.SongName = songName;
        data.Nodes = nodes;
        data.Section = sectionTime;

        if (data == null)
            return null;

        try
        {
            return JsonUtility.ToJson(data, true);
        }
        catch
        {
            return null;
        }
    }
    public static async UniTask<NodeSaveData> AutoDeserialize(string text, string path)
    {
        string extension = Path.GetExtension(path).ToLower();

        if (extension == ".json")
        {
            return await DesrializeJson(text);
        }
        else if (extension == ".csv")
        {
            var saveData = new NodeSaveData();
            saveData.Nodes = DeserializeCSV(text).OrderBy(x => x.Time).ToList();
            saveData.DataVersion = Version;
            saveData.BPM = EditorManager.I.BPM;
            saveData.SoundIndex = -1;
            return saveData;
        }
        else
        {
            Debug.LogError($"Error Extension {extension}");
            return null;
        }
    }
    public static async UniTask<NodeSaveData> AutoDeserialize(string text)
    {
        return await DesrializeJson(text);
    }
}

[System.Serializable]
public class NodeSaveData
{
    public string DataVersion;
    public float BPM;
    public int SoundIndex;
    public string SongName = "Null";

    public List<NodeData> Nodes;
    public List<float> Section;
}