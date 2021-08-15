using Microsoft.AspNetCore.Components;
using MudBlazor;
using StandardFramework.Models.Config;
using StandardFramework.Services;
using StandardFramework.Utilities.PageHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardFramework.Pages.FeatureFlags
{
    public partial class UserConfiguration : StandardScreenBase
    {
        #region Dependencies

        [Inject]
        public AppDbContext DbContext { get; set; }

        #endregion

        #region Members

        public IEnumerable<UserConfigModel> UserConfigs { get; set; } = Enumerable.Empty<UserConfigModel>();

        #endregion

        protected override void OnInitialized()
        {
            base.OnInitialized();
            this.UserConfigs = this.DbContext.UserConfigs;
        }

        protected async Task DeleteConfig(string configName)
        {
            await this.ActionExecutor.ExecuteAction((AppDbContext dbContext) =>
            {
                var recordToDel = dbContext.UserConfigs.FirstOrDefault(x => x.Name == configName);
                dbContext.UserConfigs.Remove(recordToDel);
                // await this.DbContext.SaveChangesAsync();
            });
        }

        protected void OpenAddNewConfigModal()
        {
            var parameter = new DialogParameters() { ["SelectedConfig"] = null };
            parameter.Add("UpdateMode", false);            
            parameter.Add("IsDisableConfigField", false);
            this.DialogService.Show<AddUserConfiguration>("Add new config", parameter);
            this.UserConfigs = this.DbContext.UserConfigs;
        }

        protected void OpenUpdateConfigModal(UserConfigModel model)
        {
            var parameter = new DialogParameters();
            parameter.Add("SelectedConfig", model);
            parameter.Add("UpdateMode", true);
            parameter.Add("IsDisableConfigField", true);
            this.DialogService.Show<AddUserConfiguration>("Update config", parameter);
            this.UserConfigs = this.DbContext.UserConfigs;
        }
    }
}
