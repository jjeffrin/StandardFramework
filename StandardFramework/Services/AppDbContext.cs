using Microsoft.EntityFrameworkCore;
using StandardFramework.Models;
using StandardFramework.Models.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardFramework.Services
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        #region Dbset members

        public DbSet<UserConfigModel> UserConfigs { get; set; }
        public DbSet<GlobalConfigModel> GlobalConfigs { get; set; }
        public DbSet<NotificationModel> Notifications { get; set; }
        public DbSet<ScreenMetricModel> ScreenMetrics { get; set; }

        #endregion
    }
}
