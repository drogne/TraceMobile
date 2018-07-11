using System;
using System.Windows.Input;
using PassFailSample.Helpers;
using PassFailSample.Models;
using Xamarin.Forms;

namespace PassFailSample.ViewModels
{
    public class OrderViewModel : BaseViewModel
    {
        #region Properties
        private string _OrderNumber;

        public string OrderNumber
        {
            get => this._OrderNumber;
            set
            {
                this.SetProperty(ref this._OrderNumber, value);
                this.FirePropertyChanged("CanProceed");
            }
        }

        
        public bool CanProceed
        {
            get => this.OrderNumber.Length == 6;    
        }

        private IBarcodeScanner Scanner { get; set; }

        private Session Session { get; }

        public ICommand ProceedCommand { get; private set; }

        #endregion

        #region Constructor and Init/Deinit

        public OrderViewModel(IdleTimeoutTimer timer, Settings Settings, Session session) : base(timer, Settings)
        {
            this.Session = session;
            this.OrderNumber = string.Empty;
            this.Scanner = DependencyService.Get<IBarcodeScanner>();
            this.ProceedCommand = new Command(() => this.CompleteScreen());
        }

        public override void Initialize()
        {
            this.Session.OrderNumber = string.Empty;
            this.OrderNumber = string.Empty;
            base.Initialize();
            MessagingCenter.Subscribe<Page, string>(this, "GoodRead", this._Scanner_BarcodeScanned, App.Current.MainPage);
        }

        public override void Deinitialize()
        {
            this.OrderNumber = String.Empty;
            base.Deinitialize();
            MessagingCenter.Unsubscribe<Page, string>(this, "GoodRead");
        }

        #endregion

        #region Methods

        private void CompleteScreen()
        {
            this.Session.OrderNumber = this.OrderNumber;
            this.NavService.NavigateTo(EnabledScreens.GetNextScreen(this));
        }

        private async void _Scanner_BarcodeScanned(object sender, string e)
        {
            this.OrderNumber = e;
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