using System;
using System.Linq;
using InGame.Stage;
using Newtonsoft.Json;
using UnityEngine;

namespace Common
{
    public static class StageDataSerializer
    {
        public const string Version = "2.2";

        public static readonly JsonSerializerSettings SerializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Formatting.Indented,

            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };


        public static StageSaveData DeserializeJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return null;

            try
            {
                var data = JsonConvert.DeserializeObject<StageSaveData>(
                    json,
                    SerializerSettings
                );

                if (data == null)
                    return null;

                if (data.Version != Version)
                {
                    Debug.LogWarning("データのバージョンが違います");
                }

                return data;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return null;
            }
        }


        public static string SerializeJson(
            LightPatternBaseData[] patternBaseDatas,
            int songIndex)
        {
            if (patternBaseDatas == null)
                return null;

            try
            {
                // 時間順に並び替え
                patternBaseDatas = patternBaseDatas
                    .OrderBy(x => x.Time)
                    .ToArray();

                StageSaveData saveData = new()
                {
                    Version = Version,
                    LightData = patternBaseDatas,
                    SongDataIndex = songIndex
                };


                return JsonConvert.SerializeObject(
                    saveData,
                    SerializerSettings
                );
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return null;
            }
        }
    }


    [Serializable]
    public class StageSaveData
    {
        public string Version;
        public int SongDataIndex;
        public LightPatternBaseData[] LightData;
    }
}