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
            public Button Button;
            public GameObject Screen;
        }

        [SerializeField] private NavigationTab[] _tabs;
        [SerializeField] private int _initialTabIndex;

        private UiController _uiController;
        private int _currentTabIndex;
        private NavigationTab _currentTab => _tabs[_currentTabIndex];

        private void Awake()
        {
            _uiController = ServiceLocator.Instance.UiController;
            
            for (int i = 0; i < _tabs.Length; i++)
            {
                int tabIndex = i;
                _tabs[i].Button.onClick.AddListener(() => OpenTab(tabIndex));
                _tabs[i].Screen.SetActive(false);
            }
            
            OpenTab(_initialTabIndex);
        }

        private void OpenTab(int tabIndex)
        {
            _currentTab.Button.interactable = true;
            _currentTabIndex = tabIndex;
            _uiController.OpenScreen(_currentTab.Screen);
            _currentTab.Button.interactable = false;
        }
    }
}