using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Data;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Screens.ViewExercise
{
    public class ViewExerciseScreen : ReactiveScreen
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private TMP_Text _equipmentsText;
        [SerializeField] private Button _editButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _deleteButton;

        private ViewExerciseViewModel _vm;
        private IDataService _dataService;
        private UiController _uiController;
        private string _exerciseId;

        public override async Task InitializeAsync(object parameter = null)
        {
            _exerciseId = parameter as string ?? throw new ArgumentException("Exercise ID required");
            _dataService = DiContainer.Instance.Resolve<IDataService>() ?? throw new InvalidOperationException("IDataService not resolved");
            _uiController =  DiContainer.Instance.Resolve<UiController>() ?? throw new InvalidOperationException("UiController not resolved");
            ViewModelFactory factory = DiContainer.Instance.Resolve<ViewModelFactory>() ?? throw new InvalidOperationException("ViewModelFactory not resolved");
            _vm = factory.Create<ViewExerciseViewModel>(_exerciseId);

            Subscribe(() => _vm.ExerciseChanged -= MarkDirtyOrRefresh);
            _vm.ExerciseChanged += MarkDirtyOrRefresh;

            _editButton.onClick.RemoveAllListeners();
            _editButton.onClick.AddListener(() =>
                _uiController.OpenScreen(ScreenType.CreateExercise, _exerciseId));

            _backButton.onClick.RemoveAllListeners();
            _backButton.onClick.AddListener(() => _uiController.CloseScreen());
            
            _deleteButton.onClick.RemoveAllListeners();
            _deleteButton.onClick.AddListener(DeleteExercise);

            Refresh();
            await base.InitializeAsync(parameter);
        }

        protected override void Refresh()
        {
            _isRefreshing = true;
            try
            {
                if (_initialized && !isDirty) return;
                
                Exercise ex = _vm.CurrentExercise;
                if (ex == null)
                {
                    _nameText.text = "Упражнение не найдено";
                    _descriptionText.text = "";
                    _equipmentsText.text = "";
                    return;
                }

                _nameText.text = ex.Name;
                _descriptionText.text = $"Описание: {ex.Description}";

                if (ex.RequiredEquipment == null || ex.RequiredEquipment.Count == 0)
                {
                    _equipmentsText.text = "Без оборудования";
                    return;
                }

                List<(string Id, string Name, int Quantity)> equipmentData = ex.RequiredEquipment.Select(r =>
                {
                    Equipment eq = _dataService.GetEquipmentById(r.EquipmentId);
                    return (Id: r.EquipmentId, Name: eq?.Name ?? "??", r.Quantity);
                }).ToList();

                _equipmentsText.text = $"Нужно: {string.Join(", ", equipmentData.Select(r => $"{r.Name} x{r.Quantity}"))}";
            }
            finally
            {
                _isRefreshing = false;
            }
        }

        private void DeleteExercise()
        {
            _vm.DeleteExercise();
            _uiController.CloseScreen();
        }
    }
}