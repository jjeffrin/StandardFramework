using MudBlazor;
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
        private readonly ISnackbar snackbarService;
        private readonly IAppState appState;
        private readonly IAppConfig appConfig;

        public enum ActionContextEnum
        {
            Update,
            Insert,
            Create,
            Read,
            Generic
        }

        public Dictionary<string, bool> Configs { get; set; }

        // inject the app DB context via constructor injection
        public ActionExecutor(IAppState appstate, IAppConfig appConfig, AppDbContext context, ISnackbar snackbarService)
        {
            this.appState = appstate;
            this.appConfig = appConfig;
            this.context = context;
            this.snackbarService = snackbarService;
        }

        public async Task ExecuteActionWithContext(Action<AppDbContext> action, ActionContextEnum actionContext = ActionContextEnum.Generic, bool showFeedback = false)
        {
            Exception exceptionInfo = null;
            try
            {
                action?.Invoke(this.context);
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

        public async Task ExecuteAction(Action<AppDbContext> action)
        {
            //this.appState.ToggleAppLoadState(true);
            Exception exceptionInfo = null;
            try
            {
                action?.Invoke(this.context);
            }
            catch (Exception ex)
            {
                exceptionInfo = ex;
            }
            finally
            {
                await this.ProcessExecutionResult(exceptionInfo);
            }
            //this.appState.ToggleAppLoadState(false);
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

            this.appState.SetDbBusy(true);
            // if (saveNotificationToDb) await this.context.SaveChangesAsync();
            //this.appState.NotifyNotificationStateChanged();
            await this.context.SaveChangesAsync();
            this.snackbarService.Add("The action is completed. Time of completion: " + DateTime.Now.ToShortTimeString(), Severity.Success);
            this.appState.SetDbBusy(false);
            this.appState.NotifyAppStateChange();
        }
    }
}
