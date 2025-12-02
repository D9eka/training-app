using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NavigationTab
{
    public class NavigationButton : MonoBehaviour
    {
        [SerializeField] private Image _activeIndicator;
        [SerializeField] private Image _icon;
        [Space] 
        [SerializeField] private Sprite _inactiveIcon;
        
        private Button _button;
        private Color _activeIndicatorColor;
        private Color _inactiveIndicatorColor;
        private Sprite _activeIcon;

        public UnityEvent onClick => _button.onClick;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            _activeIndicatorColor = _activeIndicator.color;
            _inactiveIndicatorColor = transform.parent.GetComponent<Image>().color;
            _activeIcon = _icon.sprite;
            SetActive(false);
        }

        public void SetActive(bool active)
        {
            _activeIndicator.color = active ? _activeIndicatorColor : _inactiveIndicatorColor;
            _icon.sprite = active ? _activeIcon : _inactiveIcon;
            _button.interactable = !active;
            Debug.Log($"{name} is {active}");
        }
    }
}