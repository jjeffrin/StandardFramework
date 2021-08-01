using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardFramework.Utilities.Interfaces
{
    public interface IAppState
    {
        event Action OnAppStateChange;
        void NotifyAppStateChange();
        bool IsAppLoading();
        void ToggleAppLoadState(bool? state = null);
    }
}
