using StandardFramework.Services;
using StandardFramework.Utilities.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace StandardFramework.Utilities
{
    public class AppConfig : IAppConfig
    {
        #region Members

        private readonly AppDbContext context;
        private Dictionary<string, bool> Configs { get; set; }

        #endregion

        public AppConfig(AppDbContext context)
        {
            this.context = context;
            this.Configs = new Dictionary<string, bool>();
            this.PrepareConfigDictionary();
        }

        /// <summary>
        /// Firstly, loop through the global configs, if present, then check if the global config has a user config value. 
        /// In such case, the global config state is set as user config state.
        /// </summary>
        private void PrepareConfigDictionary()
        {
            if (this.context.UserConfigs.Count() > 0)
            {
                this.context.UserConfigs
                .ToList()
                .ForEach(uConfig =>
                {
                    this.Configs.Add(uConfig.Name, uConfig.State);
                });
            }

            if (this.context.GlobalConfigs.Count() > 0)
            {
                this.context.GlobalConfigs
                .ToList()
                .ForEach(gConfig =>
                {
                    if (!this.Configs.ContainsKey(gConfig.Name))
                    {
                        this.Configs.Add(gConfig.Name, gConfig.State);
                    }
                });
            }
        }

        /// <summary>
        /// Returns the config value set by the user. If there is no user config present, then global config value is returned. If not present in both, then false is returned by default.
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public bool GetConfigValue(string config)
        {
            if (this.Configs.ContainsKey(config)) return this.Configs[config];
            return false;
        }
    }
}
