using System.Threading.Tasks;

namespace Screens
{
    public abstract class ReactiveScreen : Screen
    {
        protected bool isDirty;
        protected bool _isRefreshing;

        protected abstract void Refresh();

        public override async Task InitializeAsync(object parameter = null)
        {
            await base.InitializeAsync(parameter);
            isDirty = true;
        }

        public override async Task OnShowAsync()
        {
            if (isDirty && !_isRefreshing)
            {
                try
                {
                    Refresh();
                }
                finally
                {
                    isDirty = false;
                }
            }

            await Task.CompletedTask;
        }

        protected void MarkDirtyOrRefresh()
        {
            isDirty = true;
            if (gameObject.activeInHierarchy && !_isRefreshing)
                Refresh();
        }
    }
}