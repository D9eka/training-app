using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Core;
using Screens.Factories.Parameters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Views.Components;

namespace Screens.CreateTraining
{
    public class CreateTrainingScreen : ScreenWithUpdatableViewModel<CreateTrainingViewModel, TrainingIdParameter>
    {
        [SerializeField] private TMP_Text _header;
        [SerializeField] private TMP_InputField _nameInput;
        [SerializeField] private TMP_InputField _descInput;
        [SerializeField] private TMP_InputField _prepTimeInput;
        [SerializeField] private Transform _blockListParent;
        [SerializeField] private TrainingBlockItem _trainingBlockPrefab;
        [SerializeField] private Button _createButton;
        [SerializeField] private TMP_Text _createButtonText;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _addBlockButton;

        private readonly List<GameObject> _spawnedItems = new();

        public override async Task InitializeAsync(CreateTrainingViewModel viewModel, UiController uiController, object parameter = null)
        {
            await base.InitializeAsync(viewModel, uiController, parameter);

            Vm.EditModeChanged += OnEditModeChanged;
            Vm.CanSaveChanged += OnCanSaveChanged;
            Vm.TrainingChanged += MarkDirtyOrRefresh;

            Subscribe(() => Vm.EditModeChanged -= OnEditModeChanged);
            Subscribe(() => Vm.CanSaveChanged -= OnCanSaveChanged);
            Subscribe(() => Vm.TrainingChanged -= MarkDirtyOrRefresh);

            _nameInput.onValueChanged.RemoveAllListeners();
            _nameInput.onValueChanged.AddListener(v => Vm.Name = v);
            
            _descInput.onValueChanged.RemoveAllListeners();
            _descInput.onValueChanged.AddListener(v => Vm.Description = v);
            
            _prepTimeInput.onValueChanged.RemoveAllListeners();
            _prepTimeInput.onValueChanged.AddListener(v => Vm.PrepTimeSeconds = int.Parse(v));

            _createButton.onClick.RemoveAllListeners();
            _createButton.onClick.AddListener(OnCreate);
            
            _backButton.onClick.RemoveAllListeners();
            _backButton.onClick.AddListener(() => UIController.CloseScreen());
            
            _addBlockButton.onClick.RemoveAllListeners();
            _addBlockButton.onClick.AddListener(OnAddBlockClicked);

            OnEditModeChanged(Vm.IsEditMode);
            OnCanSaveChanged(Vm.CanSave);

            Refresh();
        }

        protected override void Refresh()
        {
            _isRefreshing = true;
            try
            {
                _nameInput.text = Vm.Name;
                _descInput.text = Vm.Description;
                _prepTimeInput.text = Vm.PrepTimeSeconds.ToString(CultureInfo.CurrentCulture);
                
                foreach (Transform child in _blockListParent)
                    if (child.TryGetComponent(out TrainingBlockItem _))
                        SimplePool.Return(child.gameObject, _trainingBlockPrefab.gameObject);

                foreach (var block in Vm.GetTrainingBlocks())
                {
                    var go = SimplePool.Get(_trainingBlockPrefab.gameObject, _blockListParent);
                    var item = go.GetComponent<TrainingBlockItem>();
                    item.Setup(block, Vm.IsEditMode, OnBlockClicked,OnBlockClicked, Vm.RemoveBlock);
                    _spawnedItems.Add(go);
                }
            }
            finally
            {
                _isRefreshing = false;
            }
        }

        private void OnCreate()
        {
            Vm.OnCreate();
            UIController.CloseScreen();
        }

        private void OnEditModeChanged(bool editMode)
        {
            _header.text = editMode ? "Изменить тренировку" : "Создать тренировку";
            _createButtonText.text = editMode ? "Изменить" : "Создать";
        }

        private void OnCanSaveChanged(bool canSave)
        {
            _createButton.interactable = canSave;
            _addBlockButton.interactable = canSave;
        }

        private void OnAddBlockClicked()
        {
            string trainingId = Vm.TrainingId;
            Vm.Save();
            UIController.OpenScreen(ScreenType.CreateBlock, new CreateTrainingBlockParameter(trainingId));
        }

        private void OnBlockClicked(string blockId)
        {
            string trainingId = Vm.TrainingId;
            Vm.Save();
            UIController.OpenScreen(ScreenType.CreateBlock, new CreateTrainingBlockParameter(trainingId, blockId));
        }
    }
}
