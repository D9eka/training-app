using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Screens.Factories.Parameters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Views.Components;

namespace Screens.SelectExercise
{
    public class SelectExerciseScreen : ScreenWithUpdatableViewModel<SelectExerciseViewModel, SelectExerciseParameter>
    {
        [SerializeField] private TMP_InputField _searchInputField;
        [SerializeField] private Transform _contentParent;
        [SerializeField] private ExerciseItem _exerciseItemPrefab;
        [SerializeField] private Button _backButton;
        
        private ItemsGroup<ExerciseItem> _exerciseItemsGroup;

        public override async Task InitializeAsync(SelectExerciseViewModel viewModel, UiController uiController, object parameter = null)
        {
            await base.InitializeAsync(viewModel, uiController, parameter);

            _exerciseItemsGroup = new ItemsGroup<ExerciseItem>(_contentParent, _exerciseItemPrefab);

            Vm.ExercisesWithQueryUpdated += MarkDirtyOrRefresh;
            
            Subscribe(() => Vm.ExercisesWithQueryUpdated -= MarkDirtyOrRefresh);

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
                List<ExerciseItem> items = _exerciseItemsGroup.Refresh(Vm.ExercisesWithQuery.Count);
                for (int i = 0; i < Vm.ExercisesWithQuery.Count; i++)
                {
                    items[i].Setup(Vm.ExercisesWithQuery[i], OnExerciseClicked);
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
