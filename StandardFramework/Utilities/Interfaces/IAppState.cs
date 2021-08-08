using StandardFramework.Models;
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
        bool IsDbBusy();
        void SetDbBusy(bool state);
        void ToggleAppLoadState(bool? state = null);
        IEnumerable<NotificationModel> LocalNotifications { get; set; }
    }
}
