using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Screens
{
    public class Screen : MonoBehaviour
    {
        [SerializeField] protected bool _showNavBar;
        [SerializeField] protected bool _showPreviousScreen;

        public bool ShowNavBar => _showNavBar;
        public bool ShowPreviousScreen => _showPreviousScreen;
        public bool IsInitialized { get; private set; }

        protected readonly List<Action> _unsubscribers = new();

        protected virtual void OnDestroy()
        {
            foreach (var unsub in _unsubscribers)
            {
                try { unsub(); } catch (Exception ex) { Debug.LogWarning($"Unsubscribe failed: {ex.Message}"); }
            }
            _unsubscribers.Clear();
        }

        public virtual async Task InitializeAsync(object parameter = null)
        {
            IsInitialized = true;
            await Task.CompletedTask;
        }

        public virtual Task OnShowAsync() => Task.CompletedTask;
        public virtual Task OnHideAsync() => Task.CompletedTask;

        protected void Subscribe(Action unsubscribeAction)
        {
            if (unsubscribeAction == null) return;
            _unsubscribers.Add(unsubscribeAction);
        }
    }
}