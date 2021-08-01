using StandardFramework.Models;
using StandardFramework.Services;
using StandardFramework.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardFramework.Utilities
{
    public class AppMetrics : IAppMetrics
    {
        private readonly AppDbContext context;
        public AppMetrics(AppDbContext context)
        {
            this.context = context;
        }

        public async Task UpdateScreenCount(Type screenType)
        {
            bool recordAlreadyPresent = this.context.ScreenMetrics.Any(x => x.ScreenName == screenType.Name);
            if (recordAlreadyPresent)
            {
                this.context.ScreenMetrics.First(x => x.ScreenName == screenType.Name).ScreenViewCount += 1;
            }
            else
            {
                await this.context.ScreenMetrics.AddAsync(new ScreenMetricModel() { ScreenName = screenType.Name, ScreenViewCount = 1 });
            }
            await this.context.SaveChangesAsync();
        }
    }
}
