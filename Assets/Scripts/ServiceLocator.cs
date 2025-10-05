using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    [SerializeField] private UiController _uiController;

    public UiController UiController => _uiController;
        
    public DataService DataService { get; private set; }

    public static ServiceLocator Instance { get; private set; }

    private void Awake()
    {
        DataService = new DataService();
        Instance = this;
    }
}