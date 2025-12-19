using System;
using System.Collections.Generic;
using Core;
using Screens;
using Screens.Factories.Parameters;
using UnityEngine;
using Zenject;

namespace NavigationTab
{
    public class NavigationTabs : MonoBehaviour
    {
        [Serializable]
        public class NavigationTab
        {
            public ScreenType Type;
            public NavigationButton Button;
        }

        [SerializeField] private NavigationTab[] _tabs;
        [SerializeField] private int _initialTabIndex;

        private UiController _uiController;
        private int _currentTabIndex = -1;
        private bool _isSwitching;
        private readonly HashSet<ScreenType> _tabKeys = new();

        private NavigationTab CurrentTab => _tabs[_currentTabIndex];

        [Inject]
        public void Construct(UiController uiController)
        {
            _uiController = uiController;
        }

        private void Start()
        {
            if (_tabs == null || _tabs.Length == 0)
                throw new InvalidOperationException("Tabs array is empty or null");
            if (_initialTabIndex < 0 || _initialTabIndex >= _tabs.Length)
                throw new InvalidOperationException($"Invalid initialTabIndex: {_initialTabIndex}");

            foreach (NavigationTab tab in _tabs)
            {
                if (tab.Type == ScreenType.Null)
                    throw new InvalidOperationException("Tab Key is null or empty");
                if (tab.Button == null)
                    throw new InvalidOperationException($"Button is null for tab {tab.Type}");
                if (!_tabKeys.Add(tab.Type))
                    throw new InvalidOperationException($"Duplicate tab key: {tab.Type}");
            }

            for (int i = 0; i < _tabs.Length; i++)
            {
                int tabIndex = i;
                _tabs[i].Button.onClick.RemoveAllListeners();
                _tabs[i].Button.onClick.AddListener(() => OpenTab(tabIndex));
            }
            
            OpenTab(_initialTabIndex);
        }

        private void OpenTab(int tabIndex)
        {
            if (_isSwitching || tabIndex < 0 || tabIndex >= _tabs.Length || tabIndex == _currentTabIndex)
                return;

            _isSwitching = true;
            try
            {
                if (_currentTabIndex >= 0 && _currentTabIndex < _tabs.Length)
                    CurrentTab.Button.SetActive(false);

                _currentTabIndex = tabIndex;
                CurrentTab.Button.SetActive(true);

                IScreenParameter param = CreateScreenParameter(CurrentTab.Type);
                _uiController.OpenScreen(CurrentTab.Type, param, true);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to open tab {tabIndex}: {ex.Message}");
            }
            finally
            {
                _isSwitching = false;
            }
        }

        private IScreenParameter CreateScreenParameter(ScreenType currentTabType)
        {
            return currentTabType switch
            {
                _ => null
            };
        }

        private void OnDestroy()
        {
            foreach (NavigationTab tab in _tabs)
            {
                if (tab.Button != null)
                    tab.Button.onClick.RemoveAllListeners();
            }
        }
    }
}