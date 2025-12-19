using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Screens.Factories.Parameters;
using Screens.ViewTrainings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Views.Components;
using Zenject;

namespace Screens.ViewExercise
{
    public class ViewExerciseScreen : ScreenWithUpdatableViewModel<ViewExerciseViewModel, ExerciseIdParameter>
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private TMP_Text _equipmentsText;
        [SerializeField] private Button _editButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _deleteButton;
        [Space]
        [SerializeField] private Transform _usingInTrainingsParent;
        [SerializeField] private TrainingItem _trainingItemPrefab;
        
        private ItemsGroup<TrainingItem> _trainingItemsGroup;
        
        [Inject]
        public void Construct(ViewExerciseViewModel viewModel, UiController uiController)
        {
            InitializeAsync(viewModel, uiController);
        }

        public override async Task InitializeAsync(ViewExerciseViewModel viewModel, UiController uiController, object parameter = null)
        {
            await base.InitializeAsync(viewModel, uiController, parameter);

            _trainingItemsGroup = new ItemsGroup<TrainingItem>(_usingInTrainingsParent, _trainingItemPrefab);

            Vm.ExerciseChanged += MarkDirtyOrRefresh;
            Subscribe(() => Vm.ExerciseChanged -= MarkDirtyOrRefresh);

            _editButton.onClick.RemoveAllListeners();
            _editButton.onClick.AddListener(() => 
                UIController.OpenScreen(ScreenType.CreateExercise, new ExerciseIdParameter(Vm.ExerciseId)));
            
            _backButton.onClick.RemoveAllListeners();
            _backButton.onClick.AddListener(() => UIController.CloseScreen());
            
            _deleteButton.onClick.RemoveAllListeners();
            _deleteButton.onClick.AddListener(DeleteExercise);
            
            Refresh();
        }

        protected override void Refresh()
        {
            _isRefreshing = true;
            try
            {
                _nameText.text = Vm.ExerciseName;
                _descriptionText.text = Vm.ExerciseDescription;
                _equipmentsText.text = Vm.EquipmentsText;
                UpdateUsingIsTrainings();
            }
            finally
            {
                _isRefreshing = false;
            }
        }

        private void DeleteExercise()
        {
            Vm.DeleteExercise();
            UIController.CloseScreen();
        }

        private void UpdateUsingIsTrainings()
        {
            List<TrainingItem> items = _trainingItemsGroup.Refresh(Vm.UsingInTrainings.Count);
            for (int i = 0; i < Vm.UsingInTrainings.Count; i++)
            {
                items[i].Setup(Vm.UsingInTrainings[i], OnTrainingClicked);
            }
        }

        private void OnTrainingClicked(string trainingId)
        {
            UIController.OpenScreen(ScreenType.ViewTraining, new TrainingIdParameter(trainingId));
        }
    }
}
