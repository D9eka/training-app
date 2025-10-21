#nullable enable
using System;
using Data;
using Models;
using Screens;
using Screens.CreateBlock;
using Screens.CreateEquipment;
using Screens.CreateExercise;
using Screens.CreateTraining;
using Screens.SelectExercise;
using Screens.ViewExercise;
using Screens.ViewExercises;
using Screens.ViewTrainings;
using Utils;

namespace Core
{
    public class ViewModelFactory
    {
        private readonly IDataService<Equipment> _equipmentDataService;
        private readonly IDataService<Exercise> _exerciseDataService;
        private readonly TrainingDataService _trainingDataService;

        public ViewModelFactory(IDataService<Equipment> equipmentDataService, 
            IDataService<Exercise> exerciseDataService, TrainingDataService trainingsDataService)
        {
            _equipmentDataService = equipmentDataService;
            _exerciseDataService = exerciseDataService;
            _trainingDataService = trainingsDataService;
        }

        public object CreateForScreen(ScreenType type, object parameter = null)
        {
            return type switch
            {
                ScreenType.ViewExercises => Create<ViewExercisesViewModel>(parameter),
                ScreenType.ViewExercise => Create<ViewExerciseViewModel>(parameter),
                ScreenType.CreateExercise => Create<CreateExerciseViewModel>(parameter),
                ScreenType.CreateEquipment => Create<CreateEquipmentViewModel>(parameter),
                ScreenType.ViewTrainings => Create<ViewTrainingsViewModel>(parameter),
                ScreenType.CreateTraining => Create<CreateTrainingViewModel>(parameter),
                ScreenType.CreateBlock => Create<CreateTrainingBlockViewModel>(parameter),
                ScreenType.SelectExercise => Create<SelectExerciseViewModel>(parameter),
                _ => throw new InvalidOperationException($"Unknown screen type {type}")
            };
        }

        public T? Create<T>(object parameter = null) where T : class
        {
            if (typeof(T) == typeof(ViewExercisesViewModel))
                return new ViewExercisesViewModel(_exerciseDataService, _equipmentDataService) as T;
            if (typeof(T) == typeof(ViewExerciseViewModel))
            {
                return new ViewExerciseViewModel(_exerciseDataService, parameter.GetId()) as T;
            }
            if (typeof(T) == typeof(CreateExerciseViewModel))
            {
                return new CreateExerciseViewModel(_exerciseDataService, _equipmentDataService, 
                    parameter.GetId(false)) as T;
            }
            if (typeof(T) == typeof(CreateEquipmentViewModel))
                return new CreateEquipmentViewModel(_equipmentDataService) as T;
            if (typeof(T) == typeof(ViewTrainingsViewModel))
            {
                return new ViewTrainingsViewModel(_trainingDataService) as T;
            }
            if (typeof(T) == typeof(CreateTrainingViewModel))
            {
                return new CreateTrainingViewModel(_trainingDataService, parameter.GetId(false)) as T;
            }
            if (typeof(T) == typeof(CreateTrainingBlockViewModel))
            {
                return new CreateTrainingBlockViewModel(_trainingDataService, parameter.GetId(false)) as T;
            }
            if (typeof(T) == typeof(SelectExerciseViewModel))
            {
                return new SelectExerciseViewModel(_exerciseDataService, _trainingDataService, 
                    parameter.GetId(false)) as T;
            }
            throw new InvalidOperationException($"Unknown ViewModel type {typeof(T).Name}");
        }
    }
}