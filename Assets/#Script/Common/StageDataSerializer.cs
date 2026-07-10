using System.Linq;
using InGame.Stage;
using UnityEngine;

namespace Common
{

    public static class StageDataSerializer
    {
        public const string Version = "2.0";
        public static StageSaveData DeserializeJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return null;

            try
            {
                var data = JsonUtility.FromJson<StageSaveData>(json);
                if (data.Version != Version)
                {
                    Debug.LogWarning("データのバージョンが違います");
                }
                return data;
            }
            catch
            {
                return null;
            }
        }
        public static string SerializeJson(LightPatternBaseData[] patternBaseDatas)
        {
            StageSaveData saveData = new();
            patternBaseDatas = patternBaseDatas.OrderBy(x => x.Time).ToArray();

            saveData.Version =  Version;
            saveData.LightData  = patternBaseDatas;
            if (saveData == null)
                return null;

            try
            {
                return JsonUtility.ToJson(saveData, true);
            }
            catch
            {
                return null;
            }
        }
    }

    public class StageSaveData
    {
        public string Version;
        public int SongDataIndex;
        public LightPatternBaseData[] LightData;
    }
}
