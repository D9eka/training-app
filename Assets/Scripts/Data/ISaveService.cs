using System.Collections.Generic;
using Models;

namespace Data
{
    public interface ISaveService
    {
        void LoadFromDisk();

        void SaveToDisk();

        void ScheduleSave();
        
        void Commit();
        
        List<Equipment> EquipmentsCache { get; }
        List<Exercise> ExercisesCache { get; }
        List<Training> TrainingsCache { get; }
        List<WeightTracking> WeightsCache { get; }
    }
}