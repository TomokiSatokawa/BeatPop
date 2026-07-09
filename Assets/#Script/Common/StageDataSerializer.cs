using InGame.Stage;
using UnityEngine;

namespace Common
{

    public class StageDataSerializer : MonoBehaviour
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
        public static string SerializeJson(StageSaveData saveData)
        {
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
        public LightPatternBaseData[] FrontUpperPanelLight;
        public LightPatternBaseData[] RearUpperPanelLight;
    }
}
