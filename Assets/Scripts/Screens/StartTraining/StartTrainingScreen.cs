using System.Threading.Tasks;
using Core;
using Screens.Factories.Parameters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Views.Components;

namespace Screens.StartTraining
{
    public class StartTrainingScreen : ScreenWithViewModel<StartTrainingViewModel>
    {
        [SerializeField] private Button _createSimpleTrainingButton;
        [SerializeField] private TMP_InputField _searchInputField;
        [SerializeField] private Transform _contentParent;
        [SerializeField] private TrainingItem _trainingItemPrefab;
        [SerializeField] private Button _backButton;

        public override async Task InitializeAsync(StartTrainingViewModel viewModel, UiController uiController, object parameter = null)
        {
            await base.InitializeAsync(viewModel, uiController, parameter);

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
                foreach (Transform t in _contentParent)
                    SimplePool.Return(t.gameObject, _trainingItemPrefab.gameObject);

                foreach (var trainingViewData in Vm.TrainingsWithQuery)
                {
                    GameObject go = SimplePool.Get(_trainingItemPrefab.gameObject, _contentParent);
                    TrainingItem item = go.GetComponent<TrainingItem>();
                    item.Setup(trainingViewData, OnTrainingClicked);
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