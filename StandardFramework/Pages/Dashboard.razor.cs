using Microsoft.AspNetCore.Components;
using StandardFramework.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardFramework.Pages
{
    public partial class Dashboard : ComponentBase
    {
        [Inject]
        public IAppState AppState { get; set; }

        protected void ChangeLoadState()
        {
            this.AppState.ToggleAppLoadState();
        }
    }
}
