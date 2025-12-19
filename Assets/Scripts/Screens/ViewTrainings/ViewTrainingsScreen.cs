using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Screens.Factories.Parameters;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Views.Components;
using Zenject;

namespace Screens.ViewTrainings
{
    public class ViewTrainingsScreen : ScreenWithViewModel<ViewTrainingsViewModel>
    {
        [SerializeField] private Transform _contentParent;
        [SerializeField] private TrainingItem _trainingItemPrefab;
        [SerializeField] private Button _createButton;
        
        private ItemsGroup<TrainingItem> _trainingItemsGroup;
        
        [Inject]
        public void Construct(ViewTrainingsViewModel viewModel, UiController uiController)
        {
            InitializeAsync(viewModel, uiController);
        }

        public override async Task InitializeAsync(ViewTrainingsViewModel viewModel, UiController uiController, 
            object parameter = null)
        {
            await base.InitializeAsync(viewModel, uiController, parameter);

            _trainingItemsGroup = new ItemsGroup<TrainingItem>(_contentParent, _trainingItemPrefab);

            Subscribe(() => Vm.TrainingsChanged -= MarkDirtyOrRefresh);
            Vm.TrainingsChanged += MarkDirtyOrRefresh;

            _createButton.onClick.RemoveAllListeners();
            _createButton.onClick.AddListener(() => UIController.OpenScreen(ScreenType.CreateTraining));
        }

        protected override void Refresh()
        {
            _isRefreshing = true;
            try
            {
                List<TrainingItem> items = _trainingItemsGroup.Refresh(Vm.Trainings.Count);
                for (int i = 0; i < Vm.Trainings.Count; i++)
                {
                    items[i].Setup(Vm.Trainings[i], OnTrainingClicked);
                }
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