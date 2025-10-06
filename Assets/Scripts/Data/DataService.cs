using System;
using System.IO;
using Models;
using UnityEngine;

namespace Data
{
    public class DataService
    {
        private const string FileName = "appData.json";
        private string FilePath => Path.Combine(Application.persistentDataPath, FileName);

        public event Action DataChanged; // Событие для уведомления об изменениях

        public AppData Load()
        {
            if (!File.Exists(FilePath)) return new AppData();
            string json = File.ReadAllText(FilePath);
            return JsonUtility.FromJson<AppData>(json);
        }

        public void Save(AppData data)
        {
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(FilePath, json);
            DataChanged?.Invoke(); // Уведомляем всех подписчиков
        }

        public AppData Import(string externalPath)
        {
            string json = File.ReadAllText(externalPath);
            AppData data = JsonUtility.FromJson<AppData>(json);
            DataChanged?.Invoke();
            return data;
        }

        public void Export(string targetPath)
        {
            File.Copy(FilePath, targetPath);
        }
    }
}