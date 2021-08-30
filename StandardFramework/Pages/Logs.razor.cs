using MudBlazor;
using StandardFramework.Models;
using StandardFramework.Utilities.PageHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardFramework.Pages
{
    public partial class Logs : StandardScreenBase
    {
        public List<NotificationModel> Notifications { get; set; }
        public bool ShowErrors { get; set; } = false;
        public bool ShowSlowProcesses { get; set; } = false;
        public DateTime? LogsFromDate { get; set; } = DateTime.Today;
        public DateTime? LogsToDate { get; set; } = DateTime.Today;

        protected override void OnInitialized()
        {            
            base.OnInitialized();
        }  

        protected List<NotificationModel> GetAllNotificationsBasedOnFilter()
        {
            if (ShowErrors && ShowSlowProcesses)
            {
                return this.DbContext.Notifications
                    .Where(x => (x.UpdateDate.Date >= LogsFromDate && x.UpdateDate.Date <= LogsToDate) && (x.IsException || x.ElapsedTime > 5))
                    .OrderByDescending(x => x.UpdateDate)
                    .ToList();
            }
            else if (ShowErrors)
            {
                return this.DbContext.Notifications
                    .Where(x => (x.UpdateDate.Date >= LogsFromDate && x.UpdateDate.Date <= LogsToDate) && x.IsException)
                    .OrderByDescending(x => x.UpdateDate)
                    .ToList();
            }
            else if (ShowSlowProcesses)
            {
                return this.DbContext.Notifications
                    .Where(x => (x.UpdateDate.Date >= LogsFromDate && x.UpdateDate.Date <= LogsToDate) && x.ElapsedTime > 5)
                    .OrderByDescending(x => x.UpdateDate)
                    .ToList();
            }
            else
            {
                return this.DbContext.Notifications
                    .Where(x => x.UpdateDate.Date >= LogsFromDate && x.UpdateDate.Date <= LogsToDate)
                    .OrderByDescending(x => x.UpdateDate)
                    .ToList();
            }
        }

        protected Color GetBadgeColor(decimal elapsedTime, bool isException)
        {
            if (isException)
            {
                return Color.Error;
            }
            else
            {
                if (elapsedTime < 5)
                {
                    return Color.Success;
                }
                else
                {
                    return Color.Warning;
                }
            }
        } 
        protected Color GetElapsedTimeColor(decimal elapsedTime)
        {
            if (elapsedTime < 5)
            {
                return Color.Success;
            }
            else
            {
                return Color.Warning;
            }
        }
        protected string GetPillContent(bool isException)
        {
            if (isException) return "Error";
            return "Success";
        }
    }
}
