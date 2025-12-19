using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Screens.Factories.Parameters;
using Screens.Main;
using UnityEngine;
using UnityEngine.UI;
using Views.Components;
using Zenject;

namespace Screens.ViewStats
{
    public class ViewStatsScreen : ScreenWithUpdatableViewModel<ViewStatsViewModel, IScreenParameter>
    {
        [SerializeField] private Button _addWeightButton;
        [SerializeField] private Button _openWeightTrackerButton;
        [SerializeField] private AreaGraph _areaGraph;
        [Space]
        [SerializeField] private Transform _previousTrainingsParent;
        [SerializeField] private TrainingItem _trainingItemPrefab;
        
        private ItemsGroup<TrainingItem> _trainingItemsGroup;
        
        [Inject]
        public void Construct(ViewStatsViewModel viewModel, UiController uiController)
        {
            InitializeAsync(viewModel, uiController);
        }
        
        public override async Task InitializeAsync(ViewStatsViewModel viewModel, UiController uiController, object parameter = null)
        {
            await base.InitializeAsync(viewModel, uiController, parameter);

            _trainingItemsGroup = new ItemsGroup<TrainingItem>(_previousTrainingsParent, _trainingItemPrefab);

            Vm.DataUpdated += MarkDirtyOrRefresh;
            
            Subscribe(() => Vm.DataUpdated -= MarkDirtyOrRefresh);
            
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
                List<TrainingItem> items = _trainingItemsGroup.Refresh(Vm.PreviousTrainings.Count);
                for (int i = 0; i < Vm.PreviousTrainings.Count; i++)
                {
                    items[i].Setup(Vm.PreviousTrainings[i], OnTrainingClicked);
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