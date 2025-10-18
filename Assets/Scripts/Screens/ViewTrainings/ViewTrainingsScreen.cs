using System.Threading.Tasks;
using Core;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Views.Components;

namespace Screens.ViewTrainings
{
    public class ViewTrainingsScreen : ScreenWithViewModel<ViewTrainingsViewModel>
    {
        [SerializeField] private Transform _contentParent;
        [SerializeField] private TrainingItem _trainingItemPrefab;
        [SerializeField] private Button _createButton;

        public override async Task InitializeAsync(ViewTrainingsViewModel viewModel, UiController uiController, object parameter = null)
        {
            await base.InitializeAsync(viewModel, uiController, parameter);

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
                foreach (Transform t in _contentParent)
                    SimplePool.Return(t.gameObject, _trainingItemPrefab.gameObject);

                foreach (var trainingViewData in Vm.Trainings)
                {
                    GameObject go = SimplePool.Get(_trainingItemPrefab.gameObject, _contentParent);
                    var item = go.GetComponent<TrainingItem>();
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
            UIController.OpenScreen(ScreenType.ViewTraining, trainingId);
        }
    }
}