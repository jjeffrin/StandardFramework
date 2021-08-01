using StandardFramework.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace StandardFramework.Models.Config
{
    public class GlobalConfigModel : UserTrackable
    {
        [Key]
        public string Name { get; set; }
        public bool State { get; set; }
    }
}
