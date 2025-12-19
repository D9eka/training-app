using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
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
using DiContainer = Zenject.DiContainer;

namespace Screens
{
    public class ScreensInitializer : IInitializable
    {
        private const string RESOURCE_PATH = "Screens/";
        
        private readonly UiController _uiController;
        private readonly DiContainer _container;
        private readonly Camera _camera;
        private readonly Dictionary<ScreenType, Screen> _screens = new Dictionary<ScreenType, Screen>();
        private readonly Canvas _canvas;

        public ScreensInitializer(UiController uiController, DiContainer container, Camera camera, Canvas canvas)
        {
            _uiController = uiController;
            _container = container;
            _camera = camera;
            _canvas = canvas;
        }

        public async void Initialize()
        {
            try
            {
                BindAllScreens();
                _uiController.Initialize(_screens);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void BindAllScreens()
        {
            BindScreen<CreateTrainingBlockScreen>(ScreenType.CreateBlock);
            BindScreen<CreateEquipmentScreen>(ScreenType.CreateEquipment);
            BindScreen<CreateExerciseScreen>(ScreenType.CreateExercise);
            //BindScreen<CreateSimpleTrainingScreen>(ScreenType.CreateSimpleTraining);
            BindScreen<CreateTrainingScreen>(ScreenType.CreateTraining);
            BindScreen<MainScreen>(ScreenType.Main);
            BindScreen<SelectExerciseScreen>(ScreenType.SelectExercise);
            BindScreen<StartTrainingScreen>(ScreenType.StartTraining);
            BindScreen<TimerScreen>(ScreenType.Timer);
            BindScreen<ViewExerciseScreen>(ScreenType.ViewExercise);
            BindScreen<ViewExercisesScreen>(ScreenType.ViewExercises);
            BindScreen<ViewTrainingScreen>(ScreenType.ViewTraining);
            BindScreen<ViewTrainingsScreen>(ScreenType.ViewTrainings);
            BindScreen<WeightTrackerScreen>(ScreenType.WeightTracker);
            BindScreen<AddWeightScreen>(ScreenType.AddWeight);
            
            BindScreen<Screen>(ScreenType.ViewStats);
        }

        private void BindScreen<TScreen>(ScreenType screenType)
            where TScreen : Screen
        {
            GameObject screenPrefab = Resources.Load<GameObject>($"{RESOURCE_PATH}{screenType.ToString()}");
            GameObject screenGo = _container.InstantiatePrefab(screenPrefab, _canvas.transform);
            
            TScreen screen = screenGo.GetComponent<TScreen>();
            _screens.Add(screenType, screen);
            screenGo.SetActive(false);

            _container.Bind<Screen>().FromInstance(screen).AsCached();
            _container.Bind<Screen>()
                .WithId(screenType)
                .FromInstance(screen)
                .AsCached();
        }
    }
}