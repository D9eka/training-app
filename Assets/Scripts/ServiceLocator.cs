using Data;
using Screens.CreateEquipment;
using Screens.CreateExercise;
using Screens.EditExercise;
using Screens.ViewExercise;
using Screens.ViewExercises;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    [SerializeField] private UiController _uiController;
    [SerializeField] private ViewExercisesScreen _viewExercisesScreen;
    [SerializeField] private ViewExerciseScreen _viewExerciseScreen;
    [SerializeField] private CreateExerciseScreen _createExerciseScreen;
    [SerializeField] private EditExerciseScreen _editExerciseScreen;
    [SerializeField] private CreateEquipmentScreen _createEquipmentScreen;

    public UiController UiController => _uiController;
    public ViewExercisesScreen ViewExercisesScreen => _viewExercisesScreen;
    public ViewExerciseScreen ViewExerciseScreen => _viewExerciseScreen;
    public CreateExerciseScreen CreateExerciseScreen => _createExerciseScreen;
    public EditExerciseScreen EditExerciseScreen => _editExerciseScreen;
    public CreateEquipmentScreen CreateEquipmentScreen => _createEquipmentScreen;
    public DataService DataService { get; private set; }

    public static ServiceLocator Instance { get; private set; }

    // Временное хранилище для параметров экрана
    private object _screenParameter;

    public void SetScreenParameter(object parameter)
    {
        _screenParameter = parameter;
    }

    public object GetScreenParameter()
    {
        object param = _screenParameter;
        _screenParameter = null; // Очищаем после получения
        return param;
    }

    private void Awake()
    {
        DataService = new DataService();
        Instance = this;
    }
}