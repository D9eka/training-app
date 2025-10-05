using System;
using UnityEngine;
using UnityEngine.UI;

namespace NavigationTab
{
    public class NavigationTabs : MonoBehaviour
    {
        [Serializable]
        public class NavigationTab
        {
            public Button _button;
            public GameObject _screen;
        }

        [SerializeField] private NavigationTab[] _tabs;
        [SerializeField] private int _initialTabIndex;

        private UiController _uiController;
        private int _currentTabIndex;
        private NavigationTab CurrentTab => _tabs[_currentTabIndex];

        private void Awake()
        {
            _uiController = ServiceLocator.Instance.UiController;
            
            for (int i = 0; i < _tabs.Length; i++)
            {
                int tabIndex = i;
                _tabs[i]._button.onClick.AddListener(() => OpenTab(tabIndex));
                _tabs[i]._screen.SetActive(false);
            }
            
            OpenTab(_initialTabIndex);
        }

        private void OpenTab(int tabIndex)
        {
            CurrentTab._button.interactable = true;
            _currentTabIndex = tabIndex;
            _uiController.OpenScreen(CurrentTab._screen);
            CurrentTab._button.interactable = false;
        }
    }
}