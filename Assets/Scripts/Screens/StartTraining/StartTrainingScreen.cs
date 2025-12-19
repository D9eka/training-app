using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Screens.Factories.Parameters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Views.Components;
using Zenject;

namespace Screens.StartTraining
{
    public class StartTrainingScreen : ScreenWithViewModel<StartTrainingViewModel>
    {
        [SerializeField] private Button _createSimpleTrainingButton;
        [SerializeField] private TMP_InputField _searchInputField;
        [SerializeField] private Transform _contentParent;
        [SerializeField] private TrainingItem _trainingItemPrefab;
        [SerializeField] private Button _backButton;
        
        private ItemsGroup<TrainingItem> _trainingItemsGroup;
        
        [Inject]
        public void Construct(StartTrainingViewModel viewModel, UiController uiController)
        {
            InitializeAsync(viewModel, uiController);
        }

        public override async Task InitializeAsync(StartTrainingViewModel viewModel, UiController uiController, object parameter = null)
        {
            await base.InitializeAsync(viewModel, uiController, parameter);

            _trainingItemsGroup = new ItemsGroup<TrainingItem>(_contentParent, _trainingItemPrefab);

            Vm.TrainingsWithQueryUpdated += MarkDirtyOrRefresh;
            
            Subscribe(() => Vm.TrainingsWithQueryUpdated -= MarkDirtyOrRefresh);
            
            _createSimpleTrainingButton.onClick.RemoveAllListeners();
            _createSimpleTrainingButton.onClick.AddListener(() => 
                UIController.OpenScreen(ScreenType.CreateSimpleTraining));

            _searchInputField.text = Vm.SearchQuery;
            _searchInputField.onValueChanged.RemoveAllListeners();
            _searchInputField.onValueChanged.AddListener((text) => Vm.SearchQuery = text);
            
            _backButton.onClick.RemoveAllListeners();
            _backButton.onClick.AddListener(() => UIController.CloseScreen());

            Refresh();
        }

        protected override void Refresh()
        {
            _isRefreshing = true;
            try
            {
                List<TrainingItem> items = _trainingItemsGroup.Refresh(Vm.TrainingsWithQuery.Count);
                for (int i = 0; i < Vm.TrainingsWithQuery.Count; i++)
                {
                    items[i].Setup(Vm.TrainingsWithQuery[i], OnTrainingClicked);
                }
            }
            finally
            {
                _isRefreshing = false;
            }
            
        }

        private void OnTrainingClicked(string trainingId)
        {
            Vm.Clear();
            UIController.OpenScreen(ScreenType.Timer, new TimerParameter(trainingId));
        }
    }
}