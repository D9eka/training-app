using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public abstract class ScreenBase : MonoBehaviour
{
    [Header("UI Common Buttons (Optional)")]
    [SerializeField] protected Button _backButton;
    [SerializeField] protected Button _saveButton;
    [SerializeField] protected Button _editButton;

    protected UiController UiController => ServiceLocator.Instance.UiController;

    protected virtual void Awake()
    {
        if (_backButton != null)
            _backButton.onClick.AddListener(OnBackClicked);

        if (_saveButton != null)
            _saveButton.onClick.AddListener(OnSaveClicked);

        if (_editButton != null)
            _editButton.onClick.AddListener(OnEditClicked);
    }

    public virtual async Task InitializeAsync(object parameter = null) 
        => await Task.CompletedTask;

    protected virtual void OnBackClicked() => UiController.CloseScreen();
    protected virtual void OnSaveClicked() { }
    protected virtual void OnEditClicked() { }
}