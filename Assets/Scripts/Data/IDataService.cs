using System;
using System.Collections.Generic;
using Models;

namespace Data
{
    public interface IDataService
    {
        AppData GetCache();

        void LoadFromDisk();

        void SaveToDisk();

        void Commit();

        IReadOnlyList<Exercise> GetAllExercises();

        Exercise GetExerciseById(string id);

        void AddExercise(Exercise exercise);

        void UpdateExercise(Exercise exercise);

        void RemoveExercise(string id);

        IReadOnlyList<Equipment> GetAllEquipments();

        Equipment GetEquipmentById(string id);

        void AddEquipment(Equipment equipment);

        void UpdateEquipment(Equipment equipment);

        void RemoveEquipment(string id);

        event Action ExercisesUpdated;
        event Action EquipmentsUpdated;
    }
}