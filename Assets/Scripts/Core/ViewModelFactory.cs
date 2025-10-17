#nullable enable
using System;
using Data;
using Models;
using Screens.CreateEquipment;
using Screens.CreateExercise;
using Screens.ViewExercise;
using Screens.ViewExercises;

namespace Core
{
    public class ViewModelFactory
    {
        private readonly IDataService<Equipment> _equipmentDataService;
        private readonly IDataService<Exercise> _exerciseDataService;
        private readonly IDataService<Training> _trainingDataService;

        public ViewModelFactory(IDataService<Equipment> equipmentDataService, 
            IDataService<Exercise> exerciseDataService, IDataService<Training> trainingsDataService)
        {
            _equipmentDataService = equipmentDataService;
            _exerciseDataService = exerciseDataService;
            _trainingDataService = trainingsDataService;
        }

        public T? Create<T>(object parameter = null) where T : class
        {
            if (typeof(T) == typeof(ViewExercisesViewModel))
                return new ViewExercisesViewModel(_exerciseDataService) as T;
            if (typeof(T) == typeof(ViewExerciseViewModel))
            {
                return new ViewExerciseViewModel(_exerciseDataService, GetId(parameter)) as T;
            }
            if (typeof(T) == typeof(CreateExerciseViewModel))
            {
                return new CreateExerciseViewModel(_exerciseDataService, _equipmentDataService, GetId(parameter, false)) as T;
            }
            if (typeof(T) == typeof(CreateEquipmentViewModel))
                return new CreateEquipmentViewModel(_equipmentDataService) as T;
            throw new InvalidOperationException($"Unknown ViewModel type {typeof(T).Name}");
        }

        private string GetId(object parameter, bool needNonEmpty = true)
        {
            if (parameter is string id)
            {
                if (needNonEmpty && string.IsNullOrEmpty(id))
                    throw new ArgumentException("Requires a non-empty string ID", nameof(parameter));
                return id;
            }
            return !needNonEmpty ? String.Empty : throw new ArgumentException("Parameter must be a string", nameof(parameter));
        }
    }
}