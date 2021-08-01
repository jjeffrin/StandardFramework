using StandardFramework.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardFramework.Utilities
{
    public class AppState : IAppState
    {
        private bool AppLoadingState { get; set; } = false;

        public event Action OnAppStateChange;

        public bool IsAppLoading()
        {
            return this.AppLoadingState;
        }

        public void ToggleAppLoadState(bool? state = null)
        {
            if (state != null)
            {
                this.AppLoadingState = (bool)state;
            }
            else
            {
                this.AppLoadingState = !this.AppLoadingState;
            }
            NotifyAppStateChange();
        }

        public void NotifyAppStateChange()
        {
            OnAppStateChange?.Invoke();
        }
    }
}
