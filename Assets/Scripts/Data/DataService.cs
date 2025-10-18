using Models;

namespace Data
{
    public class DataService
    {
        private readonly ISaveService _saveService;
        private readonly IDataService<Equipment> _equipmentDataService;
        private readonly IDataService<Exercise> _exerciseDataService;
        private readonly IDataService<Training> _trainingDataService;
        
        public DataService(ISaveService saveService, IDataService<Equipment> equipmentDataService, 
            IDataService<Exercise> exerciseDataService, IDataService<Training> trainingDataService)
        {
            _saveService = saveService;
            
            _equipmentDataService = equipmentDataService;
            _equipmentDataService.DataUpdated += list => _saveService.Commit();
            _equipmentDataService.DataRemoved += TryDeleteEquipmentInExercises;
            
            _exerciseDataService = exerciseDataService;
            _exerciseDataService.DataUpdated += list => _saveService.Commit();
            _exerciseDataService.DataRemoved += TryDeleteExerciseInTrainings;
            
            _trainingDataService = trainingDataService;
            _trainingDataService.DataUpdated += list => _saveService.Commit();
        }
        
        

        private void TryDeleteEquipmentInExercises(string equipmentId)
        {
            bool modified = false;

            foreach (var exercise in _saveService.ExercisesCache)
            {
                int before = exercise.RequiredEquipment.Count;
                exercise.RemoveEquipment(equipmentId);
                modified = modified || exercise.RequiredEquipment.Count != before;
            }

            if (modified)
            {
                _exerciseDataService.ForceUpdate();
            }
        }

        private void TryDeleteExerciseInTrainings(string exerciseId)
        {
            bool modified = false;

            foreach (var training in _saveService.TrainingsCache)
            {
                foreach (TrainingBlock trainingBlock in training.Blocks)
                {
                    int before = trainingBlock.Exercises.Count;
                    trainingBlock.RemoveExercise(exerciseId);
                    modified = modified || trainingBlock.Exercises.Count != before;
                }
            }

            if (modified)
            {
                _trainingDataService.ForceUpdate();
            }
        }
    }
}