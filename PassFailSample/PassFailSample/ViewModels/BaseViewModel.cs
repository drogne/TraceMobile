﻿using System;
using System.Threading;
using PassFailSample.Models;
using PassFailSample.Utilities.Navigation;
using PassFailSample.Utilities;
using PassFailSample.IoC;
using Autofac;
using PassFailSample.Helpers;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PassFailSample.ViewModels
{
    public abstract class BaseViewModel : NotifyPropertyChanged
    {
        #region Properties

        private static INavigationService _NavService;
        protected INavigationService NavService
        {
            get
            {
                if (BaseViewModel._NavService == null)
                {
                    BaseViewModel._NavService = IoCContainer.Container.Resolve<NavigationService>();
                }
                return BaseViewModel._NavService;
            }
        }
        public Settings Settings { get; private set; }

        protected const int TIMEOUT_TIME = 5000;
        protected IdleTimeoutTimer TimeoutTimer { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Constructor and Init/Deinit

        protected BaseViewModel(IdleTimeoutTimer timer, Settings Settings)
        {
            this.TimeoutTimer = timer;
            this.Settings = Settings;
        }

        public virtual void Initialize()
        {
            // Standard Initailization code goes here. All Children should call this base method.
            if (!this.TimeoutTimer.IsInitialized)
            {
                this.TimeoutTimer.Initialize(this.TimeoutAction, Timeout.Infinite);
            }

            if (this.TimeoutTimer.IsInitialized && this.GetType() != typeof(HomeScreenViewModel) && this.GetType() != typeof(MainPageMasterViewModel))
            {
                this.TimeoutTimer.ResetTimer(TIMEOUT_TIME);
            }
            else
            {
                this.TimeoutTimer.ResetTimer(Timeout.Infinite);
            }
        }

        public virtual void Deinitialize()
        {
            // Standard Deinitailization code goes here. All Children should call this base method.
        }

        #endregion

        #region Methods

        public void TimeoutAction(object state)
        {
            this.TimeoutTimer.ResetTimer(Timeout.Infinite);

            // TODO get this back in the code!
            //this.NavService.PopToRootAsync(true);
        }

        #endregion
    }
}