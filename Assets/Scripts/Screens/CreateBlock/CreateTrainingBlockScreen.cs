using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Views.Components;

namespace Screens.CreateBlock
{
    public class CreateTrainingBlockScreen : ScreenWithViewModel<CreateTrainingBlockViewModel>, INeedUpdateId
    {
        [SerializeField] private TMP_Text _header;
        [SerializeField] private Transform _exerciseInBlockListParent;
        [SerializeField] private ExerciseInBlockItem _exerciseInBlockPrefab;
        [Space]
        [SerializeField] private TMP_InputField _approachesInputField;
        [SerializeField] private TMP_InputField _setsInputField;
        [SerializeField] private TMP_InputField _restAfterApproachSecondsInputField;
        [SerializeField] private TMP_InputField _restAfterSetSecondsInputField;
        [SerializeField] private TMP_InputField _restAfterBlockSecondsInputField;
        [Space]
        [SerializeField] private Button _createButton;
        [SerializeField] private TMP_Text _createButtonText;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _addExerciseInBlockButton;

        private readonly List<GameObject> _spawnedItems = new();

        public override async Task InitializeAsync(CreateTrainingBlockViewModel viewModel, UiController uiController, object parameter = null)
        {
            await base.InitializeAsync(viewModel, uiController, parameter);

            Vm.EditModeChanged += OnEditModeChanged;
            Vm.CanSaveChanged += OnCanSaveChanged;
            Vm.BlockChanged += MarkDirtyOrRefresh;

            Subscribe(() => Vm.EditModeChanged -= OnEditModeChanged);
            Subscribe(() => Vm.CanSaveChanged -= OnCanSaveChanged);
            Subscribe(() => Vm.BlockChanged -= MarkDirtyOrRefresh);
            
            
            _approachesInputField.onValueChanged.AddListener(v => Vm.Approaches = int.Parse(v));
            _setsInputField.onValueChanged.AddListener(v => Vm.Sets = int.Parse(v));
            _restAfterApproachSecondsInputField.onValueChanged.AddListener(v => 
                Vm.RestAfterApproachTimeSpan = new TimeSpan(0,0,int.Parse(v)));
            _restAfterSetSecondsInputField.onValueChanged.AddListener(v => 
                Vm.RestAfterSetTimeSpan = new TimeSpan(0,0,int.Parse(v)));
            _restAfterBlockSecondsInputField.onValueChanged.AddListener(v => 
                Vm.RestAfterBlockTimeSpan = new TimeSpan(0,0,int.Parse(v)));

            _createButton.onClick.RemoveAllListeners();
            _createButton.onClick.AddListener(OnCreate);
            
            _backButton.onClick.RemoveAllListeners();
            _backButton.onClick.AddListener(() => UIController.CloseScreen());
            
            _addExerciseInBlockButton.onClick.RemoveAllListeners();
            _addExerciseInBlockButton.onClick.AddListener(OnAddExercise);

            OnEditModeChanged(Vm.IsEditMode);
            OnCanSaveChanged(Vm.CanSave);

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
                _approachesInputField.text = Vm.Approaches.ToString();
                _setsInputField.text = Vm.Sets.ToString();
                _restAfterApproachSecondsInputField.text = Vm.RestAfterApproachTimeSpan.TotalSeconds.ToString();
                _restAfterSetSecondsInputField.text = Vm.RestAfterSetTimeSpan.TotalSeconds.ToString();
                _restAfterBlockSecondsInputField.text = Vm.RestAfterBlockTimeSpan.TotalSeconds.ToString();
                
                foreach (Transform child in _exerciseInBlockListParent)
                    if (child.TryGetComponent(out ExerciseInBlockItem _))
                        SimplePool.Return(child.gameObject, _exerciseInBlockPrefab.gameObject);

                foreach (var ex in Vm.GetExercises())
                {
                    var go = SimplePool.Get(_exerciseInBlockPrefab.gameObject, _exerciseInBlockListParent);
                    var item = go.GetComponent<ExerciseInBlockItem>();
                    item.Setup(ex, OnEditExerciseClicked, Vm.RemoveExercise, Vm.UpdateRepetition, Vm.UpdateDuration, Vm.UpdateEquipmentWeight);
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
            Vm.Save();
            UIController.CloseScreen();
        }

        private void OnEditModeChanged(bool editMode)
        {
            _header.text = editMode ? "Изменить блок" : "Создать блок";
            _createButtonText.text = editMode ? "Изменить" : "Создать";
        }

        private void OnCanSaveChanged(bool canSave) =>
            _createButton.interactable = canSave;

        private void OnEditExerciseClicked(string exerciseInBlockId)
        {
            Vm.RemoveExercise(exerciseInBlockId);
            UIController.OpenScreen(ScreenType.SelectExercise, Vm.BlockId);
        }

        private void OnAddExercise()
        {
            Vm.Save();
            UIController.OpenScreen(ScreenType.SelectExercise, Vm.BlockId);
            //BUG: Сбрасывается вес у существующих упражнений после добавления нового
        }
    }
}
