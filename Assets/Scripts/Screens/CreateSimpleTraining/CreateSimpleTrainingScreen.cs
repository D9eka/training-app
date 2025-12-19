using System.Threading.Tasks;
using Core;
using Screens.Factories.Parameters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Screens.CreateSimpleTraining
{
    public class CreateSimpleTrainingScreen : ScreenWithViewModel<CreateSimpleTrainingViewModel>
    {
        [SerializeField] private TMP_InputField _preparingSecondsInputField;
        [SerializeField] private TMP_InputField _approachesDurationSecondsInputField;
        [SerializeField] private TMP_InputField _restAfterApproachSecondsInputField;
        [SerializeField] private TMP_InputField _approachesInputField;
        [SerializeField] private TMP_InputField _setsInputField;
        [SerializeField] private TMP_InputField _restAfterSetSecondsInputField;
        [Space]
        [SerializeField] private Button _startTrainingButton;
        [SerializeField] private Button _backButton;
        
        [Inject]
        public void Construct(CreateSimpleTrainingViewModel viewModel, UiController uiController)
        {
            InitializeAsync(viewModel, uiController);
        }

        public override async Task InitializeAsync(CreateSimpleTrainingViewModel viewModel, UiController uiController, 
            object parameter = null)
        {
            await base.InitializeAsync(viewModel, uiController, parameter);

            Vm.DataUpdated += MarkDirtyOrRefresh;
            
            _preparingSecondsInputField.contentType = TMP_InputField.ContentType.IntegerNumber;
            _preparingSecondsInputField.onEndEdit.RemoveAllListeners();
            _preparingSecondsInputField.onValueChanged.AddListener(v => Vm.PreparingSeconds = ParseInt(v));
            
            _approachesDurationSecondsInputField.contentType = TMP_InputField.ContentType.IntegerNumber;
            _approachesDurationSecondsInputField.onEndEdit.RemoveAllListeners();
            _approachesDurationSecondsInputField.onEndEdit.AddListener(v => Vm.ApproachesDurationSeconds = ParseInt(v));
            
            _restAfterApproachSecondsInputField.contentType = TMP_InputField.ContentType.IntegerNumber;
            _restAfterApproachSecondsInputField.onEndEdit.RemoveAllListeners();
            _restAfterApproachSecondsInputField.onEndEdit.AddListener(v => Vm.RestAfterApproachSeconds = ParseInt(v));
            
            _approachesInputField.contentType = TMP_InputField.ContentType.IntegerNumber;
            _approachesInputField.onEndEdit.RemoveAllListeners();
            _approachesInputField.onEndEdit.AddListener(v => Vm.Approaches = ParseInt(v));
            
            _setsInputField.contentType = TMP_InputField.ContentType.IntegerNumber;
            _setsInputField.onEndEdit.RemoveAllListeners();
            _setsInputField.onEndEdit.AddListener(v => Vm.Sets = ParseInt(v));
            
            _restAfterSetSecondsInputField.contentType = TMP_InputField.ContentType.IntegerNumber;
            _restAfterSetSecondsInputField.onEndEdit.RemoveAllListeners();
            _restAfterSetSecondsInputField.onEndEdit.AddListener(v => Vm.RestAfterSetSeconds = ParseInt(v));

            _startTrainingButton.onClick.RemoveAllListeners();
            _startTrainingButton.onClick.AddListener(OnStart);

            _backButton.onClick.RemoveAllListeners();
            _backButton.onClick.AddListener(() => UIController.CloseScreen());
            
            Refresh();
        }

        protected override void Refresh()
        {
            _isRefreshing = true;
            try
            {
                _preparingSecondsInputField.text = Vm.PreparingSeconds.ToString();
                _approachesDurationSecondsInputField.text = Vm.ApproachesDurationSeconds.ToString();
                _restAfterApproachSecondsInputField.text = Vm.RestAfterApproachSeconds.ToString();
                _approachesInputField.text = Vm.Approaches.ToString();
                _setsInputField.text = Vm.Sets.ToString();
                _restAfterSetSecondsInputField.text = Vm.RestAfterSetSeconds.ToString();
            }
            finally
            {
                _isRefreshing = false;
            }
        }

        private void OnStart()
        {
            UIController.OpenScreen(ScreenType.Timer, new TimerParameter(Vm.GetSimpleTraining()));
            Vm.Clear();
        }

        private int ParseInt(string value)
        {
            return int.TryParse(value, out int result) ? result : 0;
        }
    }
}