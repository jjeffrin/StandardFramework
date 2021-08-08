using StandardFramework.Models.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace StandardFramework.Models.Config
{
    public class GlobalConfigModel : UserTrackable
    {
        public GlobalConfigModel()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        [Key]
        public string Name { get; set; }
        public bool State { get; set; }
    }
}
