using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    [SerializeField] private UiController _uiController;

    public UiController UiController => _uiController;
        
    public static ServiceLocator Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}