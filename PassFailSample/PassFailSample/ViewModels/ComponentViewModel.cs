﻿using PassFailSample.Helpers;
using PassFailSample.Models;
using PassFailSample.TraceModis;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace PassFailSample.ViewModels
{
    public class ComponentViewModel : BaseViewModel
    {
        #region Properties

        private string _ComponentNumber;

        public string ComponentNumber
        {
            get => this._ComponentNumber;
            set
            {
                this.SetProperty(ref this._ComponentNumber, value);
                this.FirePropertyChanged("CanProceed");
            }
        }

        public bool CanProceed
        {
            get => this.ComponentNumber.Length > 6;
        }

        private IBarcodeScanner Scanner { get; set; }

        public Session Session { get; }

        public ICommand ProceedCommand { get; private set; }

        #endregion

        #region Constructor and Init/Deinit

        public ComponentViewModel(IdleTimeoutTimer timer, Settings Settings, Session session) : base(timer, Settings)
        {
            this.Session = session;
            this.ComponentNumber = string.Empty;
            this.Scanner = DependencyService.Get<IBarcodeScanner>();
            this.ProceedCommand = new Command(() => this.CompleteScreen());
        }

        public override void Initialize()
        {
            this.ComponentNumber = string.Empty;
            base.Initialize();
            MessagingCenter.Subscribe<Page, string>(this, "GoodRead", this._Scanner_BarcodeScanned, App.Current.MainPage);
        }

        public override void Deinitialize()
        {
            this.ComponentNumber = string.Empty;
            base.Deinitialize();
            MessagingCenter.Unsubscribe<Page, string>(this, "GoodRead");
        }

        #endregion

        #region Methods

        private void CompleteScreen()
        {
            ObservableCollection<string> Pars = new ObservableCollection<string>() { "", "", "Test-Cognex", "" };
            Service1Client client = new Service1Client();
            client.ProcessBarCodeAsync(this.Session.OrderNumber, ComponentNumber, Pars);
            client.ProcessBarCodeCompleted += Client_ProcessBarCodeCompleted;

            //this.Session.ComponentNumber = this.ComponentNumber;
            //this.Session.Result = "Pass";
            //this.Session.ResultDescription = "Passed";
            //this.NavService.NavigateTo(EnabledScreens.GetNextScreen(this));
        }

        private void Client_ProcessBarCodeCompleted(object sender, ProcessBarCodeCompletedEventArgs e)
        {
            ObservableCollection<string> vals = e.Result;

            this.Session.ComponentNumber = this.ComponentNumber;
            this.Session.Result = vals[0].ToString();
            this.Session.ResultDescription = vals[1].ToString();
            this.NavService.NavigateTo(EnabledScreens.GetNextScreen(this));
        }

        private void _Scanner_BarcodeScanned(object sender, string e)
        {
            this.ComponentNumber = e;
            if (this.CanProceed)
            {
                this.CompleteScreen();
            }
            else
            {
                // TODO: Add logic to disable OK button and only enable when length == 6
            }
        }

        #endregion
    }
}