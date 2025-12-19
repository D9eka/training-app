using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Views.Components;
using Zenject;

namespace Screens.WeightTracker
{
    public class WeightTrackerScreen : ScreenWithViewModel<WeightTrackerViewModel>
    {
        [SerializeField] private AreaGraph _weightGraph;
        [SerializeField] private Button _weekGraphButton;
        [SerializeField] private Button _monthGraphButton;
        [SerializeField] private Button _yearGraphButton;
        [SerializeField] private Button _maxGraphButton;
        [Space]
        [SerializeField] private Transform _contentParent;
        [SerializeField] private WeightItem _weightItemPrefab;
        [Space]
        [SerializeField] private Button _addWeightButton;
        [Space]
        [SerializeField] private Button _backButton;
        
        private ItemsGroup<WeightItem> _weightItemsGroup;
        
        [Inject]
        public void Construct(WeightTrackerViewModel viewModel, UiController uiController)
        {
            InitializeAsync(viewModel, uiController);
        }
        
        public override async Task InitializeAsync(WeightTrackerViewModel viewModel, UiController uiController, 
            object parameter = null)
        {
            await base.InitializeAsync(viewModel, uiController, parameter);

            _weightItemsGroup = new ItemsGroup<WeightItem>(_contentParent, _weightItemPrefab);

            Vm.WeightsChanged += MarkDirtyOrRefresh;
            Subscribe(() => Vm.WeightsChanged -= MarkDirtyOrRefresh);
            Vm.GraphDataChanged += MarkDirtyOrRefresh;
            Subscribe(() => Vm.GraphDataChanged -= MarkDirtyOrRefresh);

            _weekGraphButton.onClick.RemoveAllListeners();
            _weekGraphButton.onClick.AddListener(() => Vm.UpdateGraphData(GraphMode.Weekly));

            _monthGraphButton.onClick.RemoveAllListeners();
            _monthGraphButton.onClick.AddListener(() => Vm.UpdateGraphData(GraphMode.Monthly));

            _yearGraphButton.onClick.RemoveAllListeners();
            _yearGraphButton.onClick.AddListener(() => Vm.UpdateGraphData(GraphMode.Yearly));

            _maxGraphButton.onClick.RemoveAllListeners();
            _maxGraphButton.onClick.AddListener(() => Vm.UpdateGraphData(GraphMode.All));

            _addWeightButton.onClick.RemoveAllListeners();
            _addWeightButton.onClick.AddListener(() => uiController.OpenScreen(ScreenType.AddWeight));
            
            _backButton.onClick.RemoveAllListeners();
            _backButton.onClick.AddListener(() => UIController.CloseScreen());
        }
        
        protected override void Refresh()
        {
            _isRefreshing = true;
            try
            {
                List<WeightItem> items = _weightItemsGroup.Refresh(Vm.Weights.Count);
                for (int i = 0; i < Vm.Weights.Count; i++)
                {
                    items[i].Setup(Vm.Weights[i]);
                }
                _weightGraph.SetValues(Vm.GraphValues);
            }
            finally
            {
                _isRefreshing = false;
            }
        }
    }
}