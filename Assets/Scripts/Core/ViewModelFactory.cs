#nullable enable
using System;
using System.Collections.Generic;
using Data;
using Models;
using Screens;
using Screens.CreateBlock;
using Screens.CreateEquipment;
using Screens.CreateExercise;
using Screens.CreateTraining;
using Screens.Factories;
using Screens.Factories.Parameters;
using Screens.ViewExercise;
using Screens.ViewExercises;
using Screens.ViewModels;

namespace Core
{
    public class ViewModelFactory
    {
        private readonly Dictionary<ScreenType, Func<IScreenParameter, IViewModel>> _creators =
            new Dictionary<ScreenType, Func<IScreenParameter, IViewModel>>
            {
                [ScreenType.Main] = param =>
                    DiContainer.Instance.Resolve<MainFactory>().Create(param),
                [ScreenType.CreateEquipment] = param => 
                    DiContainer.Instance.Resolve<CreateEquipmentFactory>().Create(param),
                [ScreenType.CreateExercise] = param => 
                    DiContainer.Instance.Resolve<CreateExerciseFactory>().Create(RequireParam<ExerciseIdParameter>(param)),
                [ScreenType.CreateBlock] = param => 
                    DiContainer.Instance.Resolve<CreateTrainingBlockFactory>().Create(RequireParam<CreateTrainingBlockParameter>(param)),
                [ScreenType.CreateTraining] = param => 
                    DiContainer.Instance.Resolve<CreateTrainingFactory>().Create(RequireParam<TrainingIdParameter>(param)),
                [ScreenType.SelectExercise] = param => 
                    DiContainer.Instance.Resolve<SelectExerciseFactory>().Create(RequireParam<SelectExerciseParameter>(param)),
                [ScreenType.ViewExercise] = param => 
                    DiContainer.Instance.Resolve<ViewExerciseFactory>().Create(RequireParam<ExerciseIdParameter>(param)),
                [ScreenType.ViewExercises] = param => 
                    DiContainer.Instance.Resolve<ViewExercisesFactory>().Create(param),
                [ScreenType.ViewTraining] = param => 
                    DiContainer.Instance.Resolve<ViewTrainingFactory>().Create(RequireParam<TrainingIdParameter>(param)),
                [ScreenType.ViewTrainings] = param => 
                    DiContainer.Instance.Resolve<ViewTrainingsFactory>().Create(RequireParam<ExerciseIdParameter>(param)),
                [ScreenType.StartTraining] = param =>
                    DiContainer.Instance.Resolve<StartTrainingFactory>().Create(param),
            };
        
        public IViewModel CreateForScreen(ScreenType type, IScreenParameter param)
        {
            return _creators[type](param);
        }
        
        private static T RequireParam<T>(IScreenParameter param)
            where T : class, IScreenParameter
        {
            if (param == null)
                return null;
            
            if (param is T typedParam)
                return typedParam;

            throw new InvalidOperationException(
                $"Screen expects parameter of type '{typeof(T).Name}', " +
                $"but received '{param?.GetType().Name ?? "null"}'.");
        }
    }
}