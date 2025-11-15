using System;
using System.Collections.Generic;
using Core;
using Data;
using Models;
using Screens.Factories.Parameters;
using UnityEngine;

namespace Screens.Timer
{
    public class TimerViewModel : IUpdatableViewModel<TrainingIdParameter>, ITickable
    {
        public Action ValueUpdated;
        
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
            IDataService<Equipment> equipmentDataService, TrainingIdParameter param)
        {
            _trainingDataService = trainingDataService;
            _timerScreenDataCreator = new TimerScreenDataCreator(exerciseDataService, equipmentDataService);
            UpdateParameter(param);
        }

        public void UpdateParameter(TrainingIdParameter param)
        {
            _currentTraining = _trainingDataService.GetDataById(param.TrainingId);
            _timeScreens = _timerScreenDataCreator.CreateTimeScreens(_currentTraining);
            SelectTimeScreen(0);
        }

        public void Tick(float deltaTime)
        {
            if (!_isTimerEnabled) return;

            _secondsLeft -= deltaTime;
            ValueText = ((int)_secondsLeft).ToString();
        }
        
        public void OnPreviousExerciseClicked()
        {
            if (_timeScreenIndex > 0)
            {
                SelectTimeScreen(_timeScreenIndex - 1);
            }
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

            if (currentTimerScreen.ValueType == TimeValueType.Seconds)
            {
                _secondsLeft = currentTimerScreen.Value;
                _isTimerEnabled = true;
            }
        }
    }
}
