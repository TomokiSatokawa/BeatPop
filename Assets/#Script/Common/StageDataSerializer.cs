using System;
using System.Linq;
using InGame.Stage;
using Newtonsoft.Json;
using UnityEngine;

namespace Common
{
    public static class StageDataSerializer
    {
        public const string Version = "2.1";

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

                // RGBA → Color 復元
                foreach (var patternData in data.LightData)
                {
                    patternData.Color = new Color(
                        patternData.R,
                        patternData.G,
                        patternData.B,
                        patternData.A
                    );
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


                // Color → RGBA 同期
                foreach (var patternData in patternBaseDatas)
                {
                    patternData.R = patternData.Color.r;
                    patternData.G = patternData.Color.g;
                    patternData.B = patternData.Color.b;
                    patternData.A = patternData.Color.a;
                }


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