using System.Threading.Tasks;
using Core;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Views.Components;

namespace Screens.ViewExercises
{
    public class ViewExercisesScreen : ScreenWithViewModel<ViewExercisesViewModel>
    {
        [SerializeField] private Transform _contentParent;
        [SerializeField] private ExerciseItem _exerciseItemPrefab;
        [SerializeField] private Button _createButton;

        public override async Task InitializeAsync(ViewExercisesViewModel viewModel, UiController uiController, object parameter = null)
        {
            await base.InitializeAsync(viewModel, uiController, parameter);

            Subscribe(() => Vm.ExercisesChanged -= MarkDirtyOrRefresh);
            Vm.ExercisesChanged += MarkDirtyOrRefresh;

            _createButton.onClick.RemoveAllListeners();
            _createButton.onClick.AddListener(() => UIController.OpenScreen(ScreenType.CreateExercise));
        }

        protected override void Refresh()
        {
            _isRefreshing = true;
            try
            {
                foreach (Transform t in _contentParent)
                    SimplePool.Return(t.gameObject, _exerciseItemPrefab.gameObject);

                foreach (var ex in Vm.Exercises)
                {
                    GameObject go = SimplePool.Get(_exerciseItemPrefab.gameObject, _contentParent);
                    var item = go.GetComponent<ExerciseItem>();
                    item.Setup(ex, OnExerciseClicked);
                }
            }
            finally
            {
                _isRefreshing = false;
            }
        }

        private void OnExerciseClicked(string exerciseId)
        {
            UIController.OpenScreen(ScreenType.ViewExercise, exerciseId);
        }
    }
}