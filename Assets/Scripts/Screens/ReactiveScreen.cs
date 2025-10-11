using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Screens
{
    public abstract class ReactiveScreen : Screen
    {
        protected bool isDirty;
        protected bool _initialized;
        protected bool _isRefreshing;

        protected virtual void OnEnable()
        {
            if (_initialized && isDirty && !_isRefreshing)
            {
                Refresh();
                isDirty = false;
            }
        }

        protected void MarkDirtyOrRefresh()
        {
            isDirty = true;
            if (!_initialized) return; 
            if (gameObject.activeInHierarchy && !_isRefreshing) Refresh();
        }

        protected abstract void Refresh();

        public override async Task InitializeAsync(object parameter = null)
        {
            try
            {
                _initialized = true;
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Initialize failed: {ex.Message}");
            }
        }
    }
}