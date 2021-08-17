using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StandardFramework.Models
{
    public class NotificationModel
    {
        public NotificationModel()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        public string ExceptionSource { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionStackTrace { get; set; }
        public string ExceptionHelpLink { get; set; }
        public DateTime ActionInvokeTime { get; set; }
        public string ActionInvokeMemberName { get; set; }
        public string SourceFilePath { get; set; }
        public int SourceLineNumber { get; set; }
        public string ActionType { get; set; }
        public bool IsException { get; set; }
    }
}
