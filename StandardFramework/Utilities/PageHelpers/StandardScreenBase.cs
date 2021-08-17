﻿using Microsoft.AspNetCore.Components;
using MudBlazor;
using StandardFramework.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardFramework.Utilities.PageHelpers
{
    public class StandardScreenBase : ComponentBase, IDisposable
    {
        [Inject]
        public IAppState AppState { get; set; }  
        [Inject]
        public ISnackbar SnackbarService { get; set; }
        [Inject]
        public IDialogService DialogService { get; set; }
        [Inject]
        public IActionExecutor ActionExecutor { get; set; }

        public StandardScreenBase()
        {
            
        }

        protected override void OnInitialized()
        {
            this.AppState.ToggleAppLoadState(true);
            this.AppState.OnAppStateChange += StateHasChanged;
        }
        
        protected override bool ShouldRender()
        {
            if (this.AppState.IsDbBusy()) 
            {
                return false;
            }
            return base.ShouldRender();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                this.AppState.ToggleAppLoadState(false);
            }
            base.OnAfterRender(firstRender);
        }

        public void Dispose()
        {
            this.AppState.OnAppStateChange -= StateHasChanged;
        }
    }
}
