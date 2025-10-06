using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using UnityEngine;
using UnityEngine.UI;
using Views.Components.Exercise;

namespace Screens.ViewExercises
{
    public class ViewExercisesScreen : ScreenBase
    {
        [SerializeField] private Button _addNewExerciseButton;
        [SerializeField] private Transform _exerciseListParent;
        [SerializeField] private ExerciseItem _exerciseItemPrefab;

        private ViewExercisesViewModel _vm;
        private List<ExerciseItem> _spawnedItems = new List<ExerciseItem>();

        public override async Task InitializeAsync(object parameter = null)
        {
            _addNewExerciseButton.onClick.AddListener(() => UiController.OpenScreen(
                ServiceLocator.Instance.CreateExerciseScreen.gameObject, true, false));
        
            _vm = new ViewExercisesViewModel(ServiceLocator.Instance.DataService);
            _vm.ExercisesChanged += RefreshList;
            ServiceLocator.Instance.DataService.DataChanged += OnDataChanged;

            RefreshList();
            await Task.CompletedTask;
        }

        private void OnEnable()
        {
            if (_vm == null) return;
            _vm.ExercisesChanged += RefreshList;
            ServiceLocator.Instance.DataService.DataChanged += OnDataChanged;
            RefreshList();
        }

        private void OnDisable()
        {
            _vm.ExercisesChanged -= RefreshList;
            ServiceLocator.Instance.DataService.DataChanged -= OnDataChanged;
        }

        private void OnDataChanged()
        {
            _vm.Load();
            RefreshList();
        }

        private void RefreshList()
        {
            foreach (ExerciseItem item in _spawnedItems)
            {
                Destroy(item.gameObject);
            }
            _spawnedItems.Clear();

            foreach (Exercise ex in _vm.Exercises)
            {
                ExerciseItem item = Instantiate(_exerciseItemPrefab, _exerciseListParent);
                item.Setup(ex, OnExerciseClicked);
                _spawnedItems.Add(item);
            }
        }

        private void OnExerciseClicked(Exercise ex)
        {
            ServiceLocator.Instance.SetScreenParameter(ex);
            UiController.OpenScreen(
                ServiceLocator.Instance.ViewExerciseScreen.gameObject,
                true,
                true
            );
        }
    }
}