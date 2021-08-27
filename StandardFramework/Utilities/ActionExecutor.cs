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
        private Guid lastAddedNotifId;

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
            this.appState.ToggleAppLoadState(true);
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
            this.lastAddedNotifId = obj.Id;
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
            obj.UpdateDate = DateTime.Now;
            obj.UpdatedUser = "DEV";
            return obj;
        }

        private async Task ProcessExecutionResult(Exception exceptionInfo, ActionInfo actionInfo)
        {
            // 1. Get the config value to see if logs are to be tracked in DB
            bool saveNotificationToDb = this.appConfig.GetConfigValue(Constants.AppConfigSettings.TRACK_NOTIFICATIONS_IN_DB);
            // 2. Check if the config is true. If true, then save off the log to DB
            if (saveNotificationToDb)
            {
                await this.context.Notifications.AddAsync(CreateNotificationObject(exceptionInfo, actionInfo));
                this.appState.SetDbBusy(true);
                await this.context.SaveChangesAsync();
            }
            // 3. Stop the watch because all trackable actions are done!
            this.watch.Stop();
            // 4. Show a snackbar to the user, indicating the result of the action
            if (exceptionInfo != null)
            {
                this.snackbarService.Add("(" + DateTime.Now.ToLongTimeString() + ") Error: " + exceptionInfo.Message + "(" + Decimal.Divide(this.watch.ElapsedMilliseconds, 1000) + " secs).", Severity.Error);
            }
            else
            {
                this.snackbarService.Add("(" + DateTime.Now.ToLongTimeString() + ") Action is completed (" + Decimal.Divide(this.watch.ElapsedMilliseconds, 1000) + " secs).", Severity.Success);
            }
            // 5. Query log record thats just been saved off to DB
            var lastAddedNotifRecord = this.context.Notifications.Single(x => x.Id == this.lastAddedNotifId);
            // 6. Update the log record with ElapsedTime
            if (lastAddedNotifRecord != null)
            {
                lastAddedNotifRecord.ElapsedTime = Decimal.Divide(this.watch.ElapsedMilliseconds, 1000);
                this.context.Notifications.Update(lastAddedNotifRecord);
            }
            // 7. Again save off the log updates to DB
            await this.context.SaveChangesAsync();
            // 8. Reset the watch and get ready for the next action!
            this.watch.Reset();
            // 9. Also, since all DB activities are done for the currect action, set the DbBusy flag to false
            this.appState.SetDbBusy(false);
            this.appState.ToggleAppLoadState(false);
            this.appState.NotifyAppStateChange();
        }
    }
}
