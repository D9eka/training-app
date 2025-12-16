using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Models;
using UnityEngine;

namespace Data
{
    public class SaveService : ISaveService
    {
        public List<Equipment> EquipmentsCache => _cache.Equipments;
        public List<Exercise> ExercisesCache =>  _cache.Exercises;
        public List<Training> TrainingsCache => _cache.Trainings;
        public List<WeightTracking> WeightsCache => _cache.Weights;
        
        private const string FileName = "appData.json";
        private readonly string _filePath;
        private readonly bool _deferSave;
        private readonly int _saveDelayMs;
        private readonly object _lock = new();
        private AppData _cache;
        private Timer _saveTimer;

        public SaveService(bool deferSave = true, int saveDelayMs = 2000)
        {
            _deferSave = deferSave;
            _saveDelayMs = Math.Max(1000, saveDelayMs);
            _filePath = Path.Combine(Application.persistentDataPath, FileName);
            _cache = new AppData();
            LoadFromDisk();
        }

        public void LoadFromDisk()
        {
            try
            {
                lock (_lock)
                {
                    if (!File.Exists(_filePath))
                    {
                        SaveToDisk();
                        return;
                    }

                    string json = File.ReadAllText(_filePath);
                    _cache = string.IsNullOrEmpty(json)
                        ? new AppData()
                        : JsonUtility.FromJson<AppData>(json) ?? new AppData();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[DataService] Load failed: {ex.Message}");
                _cache = new AppData();
                SaveToDisk();
            }
        }

        public void SaveToDisk()
        {
            try
            {
                lock (_lock)
                {
                    string json = JsonUtility.ToJson(_cache, true);
                    File.WriteAllText(_filePath, json);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[DataService] Save failed: {ex.Message}");
            }
        }

        public void ScheduleSave()
        {
            if (!_deferSave)
            {
                SaveToDisk();
                return;
            }

            if (_saveTimer == null)
                _saveTimer = new Timer(_ => SaveToDisk(), null, _saveDelayMs, Timeout.Infinite);
            else
                _saveTimer.Change(_saveDelayMs, Timeout.Infinite);
        }

        public void Commit()
        {
            _saveTimer?.Change(Timeout.Infinite, Timeout.Infinite);
            SaveToDisk();
        }

        ~SaveService()
        {
            try
            {
                Commit();
                _saveTimer?.Dispose();
            }
            catch { }
        }
    }
}