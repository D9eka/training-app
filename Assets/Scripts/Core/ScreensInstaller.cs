using System;
using System.Collections.Generic;
using Screens;
using UnityEngine;
using Screen = Screens.Screen;

namespace Core
{
    public class ScreensInstaller : MonoBehaviour
    {
        [Serializable]
        private class ScreenEntry
        {
            public ScreenType Type;
            public Screen Screen;
        }
        [SerializeField] private List<ScreenEntry> _screens = new();

        public void Install(ServiceLocator serviceLocator)
        {
            foreach (ScreenEntry entry in _screens)
            {
                if (entry.Type == ScreenType.Null || entry.Screen == null)
                {
                    Debug.LogError($"Invalid screen entry: Key={entry.Type}, Screen={entry.Screen}");
                    continue;
                }
                serviceLocator.RegisterNamed(entry.Type.ToString(), entry.Screen.gameObject);
                entry.Screen.gameObject.SetActive(false);
            }
        }
    }
}