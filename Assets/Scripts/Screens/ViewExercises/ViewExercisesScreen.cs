using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Screens.Factories.Parameters;
using UnityEngine;
using UnityEngine.UI;
using Views.Components;
using Zenject;

namespace Screens.ViewExercises
{
    public class ViewExercisesScreen : ScreenWithViewModel<ViewExercisesViewModel>
    {
        [SerializeField] private Transform _contentParent;
        [SerializeField] private ExerciseItem _exerciseItemPrefab;
        [SerializeField] private Button _createButton;
        
        private ItemsGroup<ExerciseItem> _exerciseItemsGroup;
        
        [Inject]
        public void Construct(ViewExercisesViewModel viewModel, UiController uiController)
        {
            InitializeAsync(viewModel, uiController);
        }

        public override async Task InitializeAsync(ViewExercisesViewModel viewModel, UiController uiController, 
            object parameter = null)
        {
            await base.InitializeAsync(viewModel, uiController, parameter);
            
            _exerciseItemsGroup = new ItemsGroup<ExerciseItem>(_contentParent, _exerciseItemPrefab);
            
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
                List<ExerciseItem> items = _exerciseItemsGroup.Refresh(Vm.Exercises.Count);
                for (int i = 0; i < Vm.Exercises.Count; i++)
                {
                    items[i].Setup(Vm.Exercises[i], OnExerciseClicked);
                }
            }
            finally
            {
                _isRefreshing = false;
            }
        }

        private void OnExerciseClicked(string exerciseId)
        {
            UIController.OpenScreen(ScreenType.ViewExercise, new ExerciseIdParameter(exerciseId));
        }
    }
}