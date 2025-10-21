using System.Threading.Tasks;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Screens.ViewExercise
{
    public class ViewExerciseScreen : ScreenWithViewModel<ViewExerciseViewModel>, INeedUpdateId
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private TMP_Text _equipmentsText;
        [SerializeField] private Button _editButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _deleteButton;

        public override async Task InitializeAsync(ViewExerciseViewModel viewModel, UiController uiController, object parameter = null)
        {
            await base.InitializeAsync(viewModel, uiController, parameter);

            Vm.ExerciseChanged += MarkDirtyOrRefresh;
            Subscribe(() => Vm.ExerciseChanged -= MarkDirtyOrRefresh);

            _editButton.onClick.AddListener(() => UIController.OpenScreen(ScreenType.CreateExercise, Vm.ExerciseId));
            _backButton.onClick.AddListener(() => UIController.CloseScreen());
            _deleteButton.onClick.AddListener(DeleteExercise);
        }
        
        public void UpdateId(string id)
        {
            Vm.ExerciseId = id;
        }

        protected override void Refresh()
        {
            _isRefreshing = true;
            try
            {
                _nameText.text = Vm.ExerciseName;
                _descriptionText.text = Vm.ExerciseDescription;
                _equipmentsText.text = Vm.EquipmentsText;
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
    }
}