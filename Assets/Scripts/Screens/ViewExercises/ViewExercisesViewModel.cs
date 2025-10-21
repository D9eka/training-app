using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Models;
using Screens.ViewModels;

namespace Screens.ViewExercises
{
    public class ViewExercisesViewModel : IViewModel
    {
        private readonly IDataService<Exercise> _exerciseDataService;
        private readonly IDataService<Equipment> _equipmentDataService;

        private IReadOnlyList<ExerciseViewData> _exercisesView;

        public IReadOnlyList<ExerciseViewData> Exercises => _exercisesView;
        public event Action ExercisesChanged;

        public ViewExercisesViewModel(IDataService<Exercise> exerciseDataService, IDataService<Equipment> equipmentDataService)
        {
            _exerciseDataService = exerciseDataService ?? throw new ArgumentNullException(nameof(exerciseDataService));
            _equipmentDataService = equipmentDataService ?? throw new ArgumentNullException(nameof(equipmentDataService));

            Load(_exerciseDataService.Cache);
            _exerciseDataService.DataUpdated += Load;
        }

        private void Load(IReadOnlyList<Exercise> allExercises)
        {
            _exercisesView = allExercises
                .Select(ex => new ExerciseViewData
                {
                    Id = ex.Id,
                    Name = ex.Name,
                    Equipments = ex.RequiredEquipment.Select(req =>
                        (req.Equipment?.Name ?? "???", req.Quantity)
                    ).ToList()
                })
                .ToList();

            ExercisesChanged?.Invoke();
        }
    }
}