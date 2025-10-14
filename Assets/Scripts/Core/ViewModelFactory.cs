#nullable enable
using System;
using Data;
using Screens.CreateEquipment;
using Screens.CreateExercise;
using Screens.ViewExercise;
using Screens.ViewExercises;

namespace Core
{
    /// <summary>
    /// Фабрика для создания ViewModel'ов. Поддерживает кастомные создатели.
    /// </summary>
    public class ViewModelFactory
    {
        private readonly IDataService _dataService;

        public ViewModelFactory(IDataService ds)
        {
            _dataService = ds ?? throw new ArgumentNullException(nameof(ds));
        }

        /// <summary>
        /// Создаёт ViewModel указанного типа.
        /// </summary>
        public T? Create<T>(object parameter = null) where T : class
        {
            if (typeof(T) == typeof(ViewExercisesViewModel))
                return new ViewExercisesViewModel(_dataService) as T;
            if (typeof(T) == typeof(ViewExerciseViewModel))
            {
                if (parameter is not string id || string.IsNullOrEmpty(id))
                    throw new ArgumentException("ViewExerciseViewModel requires a non-empty string ID", nameof(parameter));
                return new ViewExerciseViewModel(_dataService, id) as T;
            }
            if (typeof(T) == typeof(CreateExerciseViewModel))
            {
                return new CreateExerciseViewModel(_dataService, parameter.ToString()) as T;
            }
            if (typeof(T) == typeof(CreateEquipmentViewModel))
                return new CreateEquipmentViewModel(_dataService) as T;
            throw new InvalidOperationException($"Unknown ViewModel type {typeof(T).Name}");
        }
    }
}