using System.Threading.Tasks;
using Core;
using Screens.Factories.Parameters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Screens.Timer
{
    public class TimerScreen : ScreenWithUpdatableViewModel<TimerViewModel, TimerParameter>
    {
        [SerializeField] private Image _backgroundImage;
        [Space]
        [SerializeField] private TMP_Text _currentExerciseText;
        [Space]
        [SerializeField] private TMP_Text _valueText;
        [SerializeField] private TMP_Text _valueTypeText;
        [Space]
        [SerializeField] private TMP_Text _nextExerciseText;
        [SerializeField] private TMP_Text _currentExerciseIndexText;
        [Space]
        [SerializeField] private Button _previousExerciseButton;
        [SerializeField] private Button _pauseExerciseButton;
        [SerializeField] private Button _nextExerciseButton;
        [Space]
        [SerializeField] private Button _backButton;

        public override async Task InitializeAsync(TimerViewModel viewModel, UiController uiController, object parameter = null)
        {
            await base.InitializeAsync(viewModel, uiController, parameter);

            Vm.ValueUpdated += MarkDirtyOrRefresh;
            
            Subscribe(() => Vm.ValueUpdated -= MarkDirtyOrRefresh);

            _currentExerciseText.text = Vm.CurrentExerciseText;
            _valueText.text = Vm.ValueText;
            _valueTypeText.text = Vm.ValueTypeText;
            _nextExerciseText.text = Vm.NextExerciseText;
            _currentExerciseIndexText.text = Vm.CurrentExerciseIndexText;
            
            _previousExerciseButton.onClick.RemoveAllListeners();
            _previousExerciseButton.onClick.AddListener(() => Vm.OnPreviousExerciseClicked());
            
            _pauseExerciseButton.onClick.RemoveAllListeners();
            _pauseExerciseButton.onClick.AddListener(() => Vm.OnPauseExerciseClicked());
            
            _nextExerciseButton.onClick.RemoveAllListeners();
            _nextExerciseButton.onClick.AddListener(() => Vm.OnNextExerciseClicked());
            
            _backButton.onClick.RemoveAllListeners();
            _backButton.onClick.AddListener(() => UIController.CloseScreen());

            Refresh();
        }

        protected override void Refresh()
        {
            _isRefreshing = true;
            try
            {
                _backgroundImage.color = Vm.BackgroundColor;
                _currentExerciseText.text = Vm.CurrentExerciseText;
                _valueText.text = Vm.ValueText;
                _valueTypeText.text = Vm.ValueTypeText;
                _nextExerciseText.text = Vm.NextExerciseText;
                _currentExerciseIndexText.text = Vm.CurrentExerciseIndexText;
            }
            finally
            {
                _isRefreshing = false;
            }
        }
    }
}
