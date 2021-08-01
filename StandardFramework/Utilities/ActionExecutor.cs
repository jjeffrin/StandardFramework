using StandardFramework.Models;
using StandardFramework.Services;
using StandardFramework.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardFramework.Utilities
{
    public class ActionExecutor : IActionExecutor
    {
        private readonly AppDbContext context;
        private readonly IAppState appState;
        private readonly IAppConfig appConfig;

        public Dictionary<string, bool> Configs { get; set; }

        // inject the app DB context via constructor injection
        public ActionExecutor(IAppState appstate, IAppConfig appConfig, AppDbContext context)
        {
            this.appState = appstate;
            this.appConfig = appConfig;
            this.context = context;
        }

        public async Task ExecuteAction(Action action)
        {
            Exception exceptionInfo = null;
            try
            {
                action?.Invoke();
            }
            catch (Exception ex)
            {
                exceptionInfo = ex;
            }
            finally
            {
                await this.ProcessExecutionResult(exceptionInfo);
            }
        }

        private async Task ProcessExecutionResult(Exception exceptionInfo)
        {
            bool saveNotificationToDb = this.appConfig.GetConfigValue(Constants.AppConfigSettings.TRACK_NOTIFICATIONS_IN_DB);
            if (exceptionInfo != null)
            {
                // add fail message
                if (saveNotificationToDb)
                {
                    await this.context.Notifications.AddAsync(new NotificationModel() { Content = "Failed" });
                }
                else
                {
                    this.appState.LocalNotifications.Append(new NotificationModel() { Content = "Local Failed" });
                }
            }
            else
            {
                // add success message
                if (saveNotificationToDb)
                {
                    await this.context.Notifications.AddAsync(new NotificationModel() { Content = "Success" });
                }
                else
                {
                    this.appState.LocalNotifications.Append(new NotificationModel() { Content = "Local Success" });
                }
            }

            if (saveNotificationToDb) await this.context.SaveChangesAsync();
            //this.appState.NotifyNotificationStateChanged();
            this.appState.NotifyAppStateChange();
        }
    }
}
