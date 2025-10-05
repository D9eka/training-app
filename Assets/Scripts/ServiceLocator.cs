using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    [SerializeField] private UiController _uiController;
    [SerializeField] private ViewExercisesScreen _viewExercisesScreen;
    [SerializeField] private ViewExerciseScreen _viewExerciseScreen;
    [SerializeField] private CreateExerciseScreen _createExerciseScreen;
    [SerializeField] private CreateEquipmentScreen _createEquipmentScreen;

    public UiController UiController => _uiController;
    public ViewExercisesScreen ViewExercisesScreen => _viewExercisesScreen;
    public ViewExerciseScreen ViewExerciseScreen => _viewExerciseScreen;
    public CreateExerciseScreen CreateExerciseScreen => _createExerciseScreen;
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
        var param = _screenParameter;
        _screenParameter = null; // Очищаем после получения
        return param;
    }

    private void Awake()
    {
        DataService = new DataService();
        Instance = this;
    }
}