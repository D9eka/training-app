using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Screens.Factories.Parameters;
using Screens.ViewTrainings;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Views.Components;

namespace Screens.Main
{
    public class MainScreen : ScreenWithUpdatableViewModel<MainViewModel, IScreenParameter>
    {
        [SerializeField] private Button _startTrainingButton;
        [SerializeField] private Button _addWeightButton;
        [SerializeField] private Button _openWeightTrackerButton;
        [SerializeField] private Transform _lastTrainingsParent;
        [SerializeField] private TrainingItem _trainingItemPrefab;
        [SerializeField] private AreaGraph _areaGraph;
        
        private ItemsGroup<TrainingItem> _trainingItemsGroup;
        
        public override async Task InitializeAsync(MainViewModel viewModel, UiController uiController, object parameter = null)
        {
            await base.InitializeAsync(viewModel, uiController, parameter);

            _trainingItemsGroup = new ItemsGroup<TrainingItem>(_lastTrainingsParent, _trainingItemPrefab);

            Vm.DataUpdated += MarkDirtyOrRefresh;
            
            Subscribe(() => Vm.DataUpdated -= MarkDirtyOrRefresh);
            
            _startTrainingButton.onClick.RemoveAllListeners();
            _startTrainingButton.onClick.AddListener(() => UIController.OpenScreen(ScreenType.StartTraining));
            
            _addWeightButton.onClick.RemoveAllListeners();
            _addWeightButton.onClick.AddListener(() => UIController.OpenScreen(ScreenType.AddWeight));
            
            _openWeightTrackerButton.onClick.RemoveAllListeners();
            _openWeightTrackerButton.onClick.AddListener(() => UIController.OpenScreen(ScreenType.WeightTracker));
            
            Refresh();
        }

        protected override void Refresh()
        {
            _isRefreshing = true;
            try
            {
                List<TrainingItem> items = _trainingItemsGroup.Refresh(Vm.LastTrainings.Count);
                for (int i = 0; i < Vm.LastTrainings.Count; i++)
                {
                    items[i].Setup(Vm.LastTrainings[i], OnTrainingClicked);
                }
                
                _areaGraph.SetValues(Vm.WeekWeights);
            }
            finally
            {
                _isRefreshing = false;
            }
        }

        private void OnTrainingClicked(string trainingId)
        {
            UIController.OpenScreen(ScreenType.ViewTraining, new TrainingIdParameter(trainingId));
        }
    }
}
