using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Screens.Factories.Parameters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Views.Components;

namespace Screens.CreateBlock
{
    public class CreateTrainingBlockScreen : ScreenWithUpdatableViewModel<CreateTrainingBlockViewModel, CreateTrainingBlockParameter>
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

        public override async Task InitializeAsync(CreateTrainingBlockViewModel viewModel, UiController uiController, 
            object parameter = null)
        {
            await base.InitializeAsync(viewModel, uiController, parameter);

            Vm.EditModeChanged += OnEditModeChanged;
            Vm.BlockChanged += MarkDirtyOrRefresh;

            Subscribe(() => Vm.EditModeChanged -= OnEditModeChanged);
            Subscribe(() => Vm.BlockChanged -= MarkDirtyOrRefresh);
            
            _approachesInputField.onValueChanged.RemoveAllListeners();
            _approachesInputField.onValueChanged.AddListener(v => Vm.Approaches = int.Parse(v));
            
            _setsInputField.onValueChanged.RemoveAllListeners();
            _setsInputField.onValueChanged.AddListener(v => Vm.Sets = int.Parse(v));
            
            _restAfterApproachSecondsInputField.onValueChanged.RemoveAllListeners();
            _restAfterApproachSecondsInputField.onValueChanged.AddListener(v => 
                Vm.RestAfterApproachTimeSpan = new TimeSpan(0,0,int.Parse(v)));
            
            _restAfterSetSecondsInputField.onValueChanged.RemoveAllListeners();
            _restAfterSetSecondsInputField.onValueChanged.AddListener(v => 
                Vm.RestAfterSetTimeSpan = new TimeSpan(0,0,int.Parse(v)));
            
            _restAfterBlockSecondsInputField.onValueChanged.RemoveAllListeners();
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

        protected override void Refresh()
        {
            _isRefreshing = true;
            try
            {
                OnCanSaveChanged(Vm.CanSave);
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
                    item.Setup(ex, OnEditExerciseClicked, Vm.RemoveExercise, 
                        Vm.UpdateRepetition, Vm.UpdateDuration, Vm.UpdateEquipmentWeight);
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
            _header.text = editMode ? "Изменить блок" : "Создать блок";
            _createButtonText.text = editMode ? "Изменить" : "Создать";
        }

        private void OnCanSaveChanged(bool canSave) =>
            _createButton.interactable = canSave;

        private void OnEditExerciseClicked(string exerciseInBlockId)
        {
            int exerciseIndex = Vm.GetExerciseIndex(exerciseInBlockId);
            Vm.RemoveExercise(exerciseInBlockId);
            UIController.OpenScreen(ScreenType.SelectExercise, new SelectExerciseParameter(Vm.BlockId, exerciseIndex));
        }

        private void OnAddExercise()
        {
            Vm.Save();
            UIController.OpenScreen(ScreenType.SelectExercise, new SelectExerciseParameter(Vm.BlockId));
            //BUG: Сбрасывается вес у существующих упражнений после добавления нового
        }
    }
}
