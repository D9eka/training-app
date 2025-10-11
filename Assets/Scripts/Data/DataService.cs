using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Models;
using UnityEngine;

namespace Data
{
    public class DataService : IDataService
    {
        private const string FileName = "appData.json";
        private readonly string _filePath;
        private readonly bool _deferSave;
        private readonly int _saveDelayMs;
        private readonly object _lock = new();
        private AppData _cache;
        private Timer _saveTimer;

        public event Action ExercisesUpdated;
        public event Action EquipmentsUpdated;

        public DataService(bool deferSave = true, int saveDelayMs = 2000)
        {
            _deferSave = deferSave;
            _saveDelayMs = Math.Max(1000, saveDelayMs);
            _filePath = Path.Combine(Application.persistentDataPath, FileName);
            _cache = new AppData();
            LoadFromDisk();
        }

        public AppData GetCache() => _cache;

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

        private void ScheduleSave()
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

        public IReadOnlyList<Exercise> GetAllExercises() => _cache.Exercises.AsReadOnly();
        public Exercise GetExerciseById(string id) => _cache.Exercises.Find(e => e.Id == id);
        public void AddExercise(Exercise exercise)
        {
            _cache.Exercises.Add(exercise);
            ScheduleSave();
            ExercisesUpdated?.Invoke();
        }
        public void UpdateExercise(Exercise exercise)
        {
            int i = _cache.Exercises.FindIndex(e => e.Id == exercise.Id);
            if (i >= 0) _cache.Exercises[i] = exercise;
            ScheduleSave();
            ExercisesUpdated?.Invoke();
        }
        public void RemoveExercise(string id)
        {
            Exercise it = _cache.Exercises.Find(e => e.Id == id);
            if (it != null) _cache.Exercises.Remove(it);
            ScheduleSave();
            ExercisesUpdated?.Invoke();
        }

        public IReadOnlyList<Equipment> GetAllEquipments() => _cache.Equipments.AsReadOnly();
        public Equipment GetEquipmentById(string id) => _cache.Equipments.Find(e => e.Id == id);
        public void AddEquipment(Equipment equipment)
        {
            _cache.Equipments.Add(equipment);
            ScheduleSave();
            EquipmentsUpdated?.Invoke();
        }
        public void UpdateEquipment(Equipment equipment)
        {
            int i = _cache.Equipments.FindIndex(e => e.Id == equipment.Id);
            if (i >= 0) _cache.Equipments[i] = equipment;
            ScheduleSave();
            EquipmentsUpdated?.Invoke();
        }
        public void RemoveEquipment(string id)
        {
            Equipment eq = _cache.Equipments.Find(e => e.Id == id);
            bool equipmentWasUsedInExercises = false;
            if (eq != null)
            {
                _cache.Equipments.Remove(eq);
                equipmentWasUsedInExercises = TryDeleteEquipmentInExercises(eq);
            }
            ScheduleSave();
            EquipmentsUpdated?.Invoke();
            if (equipmentWasUsedInExercises)
            {
                ExercisesUpdated?.Invoke();
            }
        }
        
        private bool TryDeleteEquipmentInExercises(Equipment eq)
        {
            bool modified = false;

            foreach (var exercise in _cache.Exercises)
            {
                int before = exercise.RequiredEquipment.Count;
                exercise.RemoveEquipment(eq.Id);
                if (exercise.RequiredEquipment.Count != before)
                    modified = true;
            }
            return modified;
        }

        ~DataService()
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