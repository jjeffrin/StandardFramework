using Microsoft.AspNetCore.Components;
using StandardFramework.Services;
using StandardFramework.Utilities.Interfaces;
using StandardFramework.Utilities.PageHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardFramework.Pages
{
    public partial class Dashboard : StandardScreenBase
    {
        protected void ChangeLoadState()
        {
            this.AppState.ToggleAppLoadState();
        }

        protected void ThrowError()
        {
            this.ActionExecutor.ExecuteAction((AppDbContext dbContext) =>
            {
                throw new ArgumentNullException();
            });
        }
    }
}
