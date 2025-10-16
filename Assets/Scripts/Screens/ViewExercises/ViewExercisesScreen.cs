using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Data;
using Models;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Views.Components;

namespace Screens.ViewExercises
{
    public class ViewExercisesScreen : ReactiveScreen
    {
        [SerializeField] private Transform _contentParent;
        [SerializeField] private ExerciseItem _exerciseItemPrefab;
        [SerializeField] private Button _createButton;

        private ViewExercisesViewModel _vm;
        private IDataService _dataService;
        private int _lastExercisesHash;

        public override async Task InitializeAsync(object parameter = null)
        {
            _dataService = DiContainer.Instance.Resolve<IDataService>() ?? throw new InvalidOperationException("IDataService not resolved");
            ViewModelFactory factory = DiContainer.Instance.Resolve<ViewModelFactory>() ?? throw new InvalidOperationException("ViewModelFactory not resolved");
            _vm = factory.Create<ViewExercisesViewModel>();

            Subscribe(() => _vm.ExercisesChanged -= MarkDirtyOrRefresh);
            _vm.ExercisesChanged += MarkDirtyOrRefresh;

            _createButton.onClick.RemoveAllListeners();
            _createButton.onClick.AddListener(() =>
                DiContainer.Instance.Resolve<UiController>().OpenScreen(ScreenType.CreateExercise));

            Refresh();
            await base.InitializeAsync(parameter);
        }

        protected override void Refresh()
        {
            _isRefreshing = true;
            try
            {
                if (_initialized && !isDirty) return;

                foreach (Transform t in _contentParent)
                {
                    SimplePool.Return(t.gameObject, _exerciseItemPrefab.gameObject);
                }

                foreach (Exercise ex in _vm.Exercises)
                {
                    GameObject go = SimplePool.Get(_exerciseItemPrefab.gameObject, _contentParent);
                    ExerciseItem item = go.GetComponent<ExerciseItem>();
                    List<(string Id, string Name, int Quantity)> exEquipment = GetEquipmentViewData(ex);
                    item.Setup(ex, exEquipment, OnExerciseClicked);
                }
            }
            finally
            {
                _isRefreshing = false;
            }
        }

        private List<(string Id, string Name, int Quantity)> GetEquipmentViewData(Exercise ex)
        {
            return ex.RequiredEquipment.Select(r =>
                (Id: r.EquipmentId, _dataService.GetEquipmentById(r.EquipmentId)?.Name, r.Quantity)).ToList();
        }

        private void OnExerciseClicked(Exercise ex) =>
            DiContainer.Instance.Resolve<UiController>().OpenScreen(ScreenType.ViewExercise, ex.Id);
    }
}