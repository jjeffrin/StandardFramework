using MudBlazor;
using StandardFramework.Models;
using StandardFramework.Services;
using StandardFramework.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace StandardFramework.Utilities
{
    public class ActionExecutor : IActionExecutor
    {
        private readonly AppDbContext context;
        private readonly ISnackbar snackbarService;
        private readonly IAppState appState;
        private readonly IAppConfig appConfig;
        private Stopwatch watch;

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
            this.watch = new Stopwatch();
        }

        //public async Task ExecuteActionWithContext(Action<AppDbContext> action, ActionContextEnum actionContext = ActionContextEnum.Generic, bool showFeedback = false)
        //{
        //    this.appState.ToggleAppLoadState(true);
        //    this.watch.Start();
        //    Exception exceptionInfo = null;
        //    try
        //    {
        //        action?.Invoke(this.context);
        //    }
        //    catch (Exception ex)
        //    {
        //        exceptionInfo = ex;
        //    }
        //    finally
        //    {
        //        await this.ProcessExecutionResult(exceptionInfo);
        //    }
        //}

        public class ActionInfo
        {
            public ActionInfo(string actionInvokeMemberName, string sourceFilePath, int sourceLineNumber)
            {
                this.ActionInvokeTime = DateTime.Now;
                this.ActionInvokeMemberName = actionInvokeMemberName;
                this.SourceFilePath = sourceFilePath;
                this.SourceLineNumber = sourceLineNumber;
            }

            public DateTime ActionInvokeTime { get; set; }
            public string ActionInvokeMemberName { get; set; }
            public string SourceFilePath { get; set; }
            public int SourceLineNumber { get; set; }
        }

        public async Task ExecuteAction(Action<AppDbContext> action, [CallerMemberName]string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            this.watch.Start();
            var actionInfo = new ActionInfo(memberName, sourceFilePath, sourceLineNumber);
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
                await this.ProcessExecutionResult(exceptionInfo, actionInfo);
            }
        }

        private NotificationModel CreateNotificationObject(Exception exceptionInfo, ActionInfo actionInfo)
        {
            var obj = new NotificationModel();
            if (exceptionInfo != null)
            {
                obj.IsException = true;
                obj.ExceptionMessage = exceptionInfo.Message;
                obj.ExceptionSource = exceptionInfo.Source;
                obj.ExceptionStackTrace = exceptionInfo.StackTrace;
                obj.ExceptionHelpLink = exceptionInfo.HelpLink;
            }
            obj.ActionInvokeMemberName = actionInfo.ActionInvokeMemberName;
            obj.ActionInvokeTime = actionInfo.ActionInvokeTime;
            obj.SourceFilePath = actionInfo.SourceFilePath;
            obj.SourceLineNumber = actionInfo.SourceLineNumber;
            return obj;
        }

        private async Task ProcessExecutionResult(Exception exceptionInfo, ActionInfo actionInfo)
        {
            bool saveNotificationToDb = this.appConfig.GetConfigValue(Constants.AppConfigSettings.TRACK_NOTIFICATIONS_IN_DB);
            if (exceptionInfo != null)
            {
                // add fail message
                if (saveNotificationToDb)
                {
                    await this.context.Notifications.AddAsync(CreateNotificationObject(exceptionInfo, actionInfo));
                }
                else
                {
                    // this.appState.LocalNotifications.Append(new NotificationModel() { Content = "Local Failed" });
                }
            }
            else
            {
                // add success message
                if (saveNotificationToDb)
                {
                    await this.context.Notifications.AddAsync(CreateNotificationObject(exceptionInfo, actionInfo));
                }
                else
                {
                    // this.appState.LocalNotifications.Append(new NotificationModel() { Content = "Local Success" });
                }
            }

            this.appState.SetDbBusy(true);
            // if (saveNotificationToDb) await this.context.SaveChangesAsync();
            //this.appState.NotifyNotificationStateChanged();
            await this.context.SaveChangesAsync();
            this.watch.Stop();
            if (exceptionInfo != null)
            {
                this.snackbarService.Add("(" + DateTime.Now.ToLongTimeString() + ") Error: " + exceptionInfo.Message + "(" + Decimal.Divide(this.watch.ElapsedMilliseconds, 1000) + " secs).", Severity.Error);
            }
            else
            {
                this.snackbarService.Add("(" + DateTime.Now.ToLongTimeString() + ") Action is completed (" + Decimal.Divide(this.watch.ElapsedMilliseconds, 1000) + " secs).", Severity.Success);
            }
            this.watch.Reset();
            this.appState.SetDbBusy(false);
            this.appState.ToggleAppLoadState(false);
            this.appState.NotifyAppStateChange();
        }
    }
}
