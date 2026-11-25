using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace CirclesSchedule
{
    public static class DataManager
    {
        private const string DataFile = "circles.json";

        public static List<CircleInfo> LoadData()
        {
            if (!File.Exists(DataFile))
            {
                return new List<CircleInfo>();
            }

            try
            {
                string json = File.ReadAllText(DataFile);
                var circles = JsonConvert.DeserializeObject<List<CircleInfo>>(json);
                return circles ?? new List<CircleInfo>();
            }
            catch (JsonException)
            {
                return new List<CircleInfo>();
            }
            catch (IOException)
            {
                return new List<CircleInfo>();
            }
        }

        public static void SaveData(List<CircleInfo> circles)
        {
            try
            {
                // Настройка для сохранения кириллицы в читаемом виде
                var settings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    StringEscapeHandling = StringEscapeHandling.Default
                };
                string json = JsonConvert.SerializeObject(circles, settings);
                File.WriteAllText(DataFile, json);
            }
            catch (IOException)
            {
                // Ошибка сохранения игнорируется
            }
        }
    }
}