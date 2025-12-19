using System;
using System.Collections.Generic;
using Core;
using Data;
using Models;
using Screens.Factories.Parameters;
using UnityEngine;
using Zenject;

namespace Screens.Timer
{
    public class TimerViewModel : IUpdatableViewModel<TimerParameter>, ITickable
    {
        public Action ValueUpdated;
        
        private readonly UiController _uiController;
        private readonly TrainingDataService _trainingDataService;
        private readonly TimerScreenDataCreator _timerScreenDataCreator;
    
        private Training _currentTraining;
        private List<TimerScreenData> _timeScreens;
        private int _timeScreenIndex;

        private float _secondsLeft;
        private bool _isTimerEnabled;
        
        public Color BackgroundColor { get; private set; }
        public string CurrentExerciseText { get; private set; }
        public string ValueText { get; private set; }
        public string ValueTypeText { get; private set; }
        public string NextExerciseText { get; private set; }
        public string CurrentExerciseIndexText { get; private set; }

        public TimerViewModel(TrainingDataService trainingDataService,
            IDataService<Exercise> exerciseDataService,
            IDataService<Equipment> equipmentDataService, UiController uiController)
        {
            _trainingDataService = trainingDataService;
            _timerScreenDataCreator = new TimerScreenDataCreator(exerciseDataService, equipmentDataService);
            _uiController = uiController;
        }

        public void UpdateParameter(TimerParameter param)
        {
            _currentTraining = null;
            if (param.HasTrainingId)
            {
                _currentTraining = _trainingDataService.GetDataById(param.TrainingId);
                _timeScreens = _timerScreenDataCreator.CreateTimeScreens(_currentTraining);
            }
            else if (param.HaveSimpleTrainingData)
            {
                _timeScreens = _timerScreenDataCreator.CreateTimeScreens(param.SimpleTrainingData);
            }
            else
            {
                throw new NullReferenceException("Parameter has no training id or timer data!");
            }
            SelectTimeScreen(0);
        }

        public void Tick()
        {
            if (!_isTimerEnabled) return;

            _secondsLeft -= Time.deltaTime;
            if (_secondsLeft <= 1)
            {
                OnNextExerciseClicked();
                return;
            }
            ValueText = ((int)_secondsLeft).ToString();
            ValueUpdated?.Invoke();
        }
        
        public void OnPreviousExerciseClicked()
        {
            int prevIndex = _timeScreenIndex - (_timeScreenIndex > 0 ? 1 : 0);
            SelectTimeScreen(prevIndex);
        }

        public void OnPauseExerciseClicked()
        {
            _isTimerEnabled = !_isTimerEnabled;
        }

        public void OnNextExerciseClicked()
        {
            if (_timeScreenIndex < _timeScreens.Count - 1)
            {
                SelectTimeScreen(_timeScreenIndex + 1);
            }
            else
            {
                _isTimerEnabled = false;
                _uiController.CloseScreen();
            }
        }

        private void SelectTimeScreen(int index)
        {
            _timeScreenIndex = index;
            UpdateValues();
        }

        private void UpdateValues()
        {
            TimerScreenData currentTimerScreen = _timeScreens[_timeScreenIndex];
            BackgroundColor = currentTimerScreen.Color;
            
            CurrentExerciseText = currentTimerScreen.CurrentExerciseHeader;
            ValueText = currentTimerScreen.Value.ToString();
            ValueTypeText = currentTimerScreen.ValueTypeText;
            NextExerciseText = _timeScreenIndex < _timeScreens.Count - 1 ? 
                _timeScreens[_timeScreenIndex + 1].CurrentExerciseHeader : "";
            CurrentExerciseIndexText = currentTimerScreen.IndexText;
            
            bool isSeconds = currentTimerScreen.ValueType == TimeValueType.Seconds;
            _isTimerEnabled = isSeconds;
            _secondsLeft = isSeconds ? currentTimerScreen.Value + 1 : 0;
            
            ValueUpdated?.Invoke();
        }
    }
}
