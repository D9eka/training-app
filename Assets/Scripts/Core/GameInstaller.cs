using System;
using Data;
using Screens;
using Screens.AddWeight;
using Screens.CreateBlock;
using Screens.CreateEquipment;
using Screens.CreateExercise;
using Screens.CreateTraining;
using Screens.Main;
using Screens.SelectExercise;
using Screens.StartTraining;
using Screens.Timer;
using Screens.ViewExercise;
using Screens.ViewExercises;
using Screens.ViewTraining;
using Screens.ViewTrainings;
using Screens.WeightTracker;
using UnityEngine;
using Zenject;

namespace Core
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private UnityEngine.Camera _camera;
        [SerializeField] private UiController _uiController;
        [SerializeField] private Canvas _canvas;
        
        public override void InstallBindings()
        {
            Container.Bind<UnityEngine.Camera>().FromInstance(_camera).AsSingle();
            InstallModels();
            InstallUI();
        }

        private void InstallModels()
        {
            Container.BindInterfacesTo<SaveService>().AsSingle();
            Container.BindInterfacesAndSelfTo<EquipmentDataService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ExerciseDataService>().AsSingle();
            Container.BindInterfacesAndSelfTo<TrainingDataService>().AsSingle();
            Container.BindInterfacesAndSelfTo<WeightTrackingDataService>().AsSingle();
            Container.Bind<DataService>().AsSingle().NonLazy();
        }

        private void InstallUI()
        {
            Container.BindInterfacesAndSelfTo<UiController>().FromInstance(_uiController).AsSingle();
            Container.Bind<Canvas>().FromInstance(_canvas).AsSingle();
            BindAllViewModels();
            
            Container
                .BindInterfacesTo<ScreensInitializer>()
                .AsSingle()
                .NonLazy();
        }

        private void BindAllViewModels()
        {
            Container.BindInterfacesAndSelfTo<CreateTrainingBlockViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<CreateEquipmentViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<CreateExerciseViewModel>().AsSingle();
            //Container.BindInterfacesAndSelfTo<CreateSimpleTrainingViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<CreateTrainingViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<MainViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<SelectExerciseViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<StartTrainingViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<TimerViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<ViewExerciseViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<ViewExercisesViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<ViewTrainingViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<ViewTrainingsViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<WeightTrackerViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<AddWeightViewModel>().AsSingle();
        }
    }
}