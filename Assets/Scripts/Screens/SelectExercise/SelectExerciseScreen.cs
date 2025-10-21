using System.Threading.Tasks;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Views.Components;

namespace Screens.SelectExercise
{
    public class SelectExerciseScreen : ScreenWithViewModel<SelectExerciseViewModel>, INeedUpdateId
    {
        [SerializeField] private TMP_InputField _searchInputField;
        [SerializeField] private Transform _contentParent;
        [SerializeField] private ExerciseItem _exerciseItemPrefab;
        [SerializeField] private Button _backButton;

        public override async Task InitializeAsync(SelectExerciseViewModel viewModel, UiController uiController, object parameter = null)
        {
            await base.InitializeAsync(viewModel, uiController, parameter);

            Vm.ExercisesWithQueryUpdated += MarkDirtyOrRefresh;
            
            Subscribe(() => Vm.ExercisesWithQueryUpdated -= MarkDirtyOrRefresh);

            _searchInputField.text = Vm.SearchQuery;
            _searchInputField.onValueChanged.RemoveAllListeners();
            _searchInputField.onValueChanged.AddListener((text) => Vm.SearchQuery = text);
            
            _backButton.onClick.RemoveAllListeners();
            _backButton.onClick.AddListener(() => UIController.CloseScreen());

            Refresh();
        }

        public void UpdateId(string id)
        {
            Vm.UpdateId(id);
        }

        protected override void Refresh()
        {
            _isRefreshing = true;
            try
            {
                foreach (Transform t in _contentParent)
                    SimplePool.Return(t.gameObject, _exerciseItemPrefab.gameObject);

                foreach (var ex in Vm.ExercisesWithQuery)
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
            Vm.Save(exerciseId);
            UIController.CloseScreen();
        }
    }
}