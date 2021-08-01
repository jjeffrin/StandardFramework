using System;
using System.ComponentModel.DataAnnotations;

namespace StandardFramework.Models
{
    public class ScreenMetricModel
    {
        public ScreenMetricModel()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        public string ScreenName { get; set; }
        public double ScreenViewCount { get; set; }
    }
}
