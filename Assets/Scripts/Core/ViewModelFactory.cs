#nullable enable
using System;
using Data;
using Models;
using Screens;
using Screens.CreateEquipment;
using Screens.CreateExercise;
using Screens.CreateTraining;
using Screens.ViewExercise;
using Screens.ViewExercises;
using Screens.ViewTrainings;

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

        public object CreateForScreen(ScreenType type, object parameter = null)
        {
            return type switch
            {
                ScreenType.ViewExercises => Create<ViewExercisesViewModel>(),
                ScreenType.ViewExercise => Create<ViewExerciseViewModel>(parameter),
                ScreenType.CreateExercise => Create<CreateExerciseViewModel>(parameter),
                ScreenType.CreateEquipment => Create<CreateEquipmentViewModel>(),
                ScreenType.ViewTrainings => Create<ViewTrainingsViewModel>(),
                ScreenType.CreateTraining => Create<CreateTrainingViewModel>(),
                _ => throw new InvalidOperationException($"Unknown screen type {type}")
            };
        }

        public T? Create<T>(object parameter = null) where T : class
        {
            if (typeof(T) == typeof(ViewExercisesViewModel))
                return new ViewExercisesViewModel(_exerciseDataService, _equipmentDataService) as T;
            if (typeof(T) == typeof(ViewExerciseViewModel))
            {
                return new ViewExerciseViewModel(_exerciseDataService, _equipmentDataService, GetId(parameter)) as T;
            }
            if (typeof(T) == typeof(CreateExerciseViewModel))
            {
                return new CreateExerciseViewModel(_exerciseDataService, _equipmentDataService, GetId(parameter, false)) as T;
            }
            if (typeof(T) == typeof(CreateEquipmentViewModel))
                return new CreateEquipmentViewModel(_equipmentDataService) as T;
            if (typeof(T) == typeof(ViewTrainingsViewModel))
            {
                return new ViewTrainingsViewModel(_trainingDataService) as T;
            }
            if (typeof(T) == typeof(CreateTrainingViewModel))
            {
                return new CreateTrainingViewModel(_trainingDataService, GetId(parameter, false)) as T;
            }
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