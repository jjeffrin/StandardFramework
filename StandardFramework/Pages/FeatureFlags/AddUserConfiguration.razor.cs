using Microsoft.AspNetCore.Components;
using MudBlazor;
using StandardFramework.Models.Config;
using StandardFramework.Services;
using StandardFramework.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardFramework.Pages.FeatureFlags
{
    public partial class AddUserConfiguration : ComponentBase
    {
        [Parameter]
        public UserConfigModel SelectedConfig { get; set; }

        [CascadingParameter] 
        MudDialogInstance UserConfigModal { get; set; }
        [Parameter]
        public bool UpdateMode { get; set; } = true;
        [Inject]
        public AppDbContext DbContext { get; set; }
        [Inject]
        public IActionExecutor ActionExecutor { get; set; }

        protected override void OnParametersSet()
        {
            if (this.SelectedConfig == null)
            {
                this.SelectedConfig = new UserConfigModel();
            }
        }

        protected string GetUpdateBtnText()
        {
            if (this.UpdateMode)
            {
                return "Update config";
            }
            else
            {
                return "Add config";
            }
        }

        protected void Cancel()
        {
            UserConfigModal.Cancel();
        }

        protected async Task ProcessUserConfig()
        {
            await this.ActionExecutor.ExecuteAction(async (AppDbContext dbContext) =>
            {
                if (this.UpdateMode)
                {
                    dbContext.UserConfigs.Update(this.SelectedConfig);
                }
                else
                {
                    await dbContext.UserConfigs.AddAsync(this.SelectedConfig);
                }
            });
            
            UserConfigModal.Close();
        }
    }
}
