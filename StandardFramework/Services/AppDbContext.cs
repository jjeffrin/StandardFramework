using Microsoft.EntityFrameworkCore;
using StandardFramework.Models.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardFramework.Services
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        #region Dbset members

        public DbSet<UserConfigModel> UserConfigs { get; set; }
        public DbSet<GlobalConfigModel> GlobalConfigs { get; set; }

        #endregion
    }
}
