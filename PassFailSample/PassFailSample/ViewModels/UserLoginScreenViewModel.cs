using Java.Lang;
using PassFailSample.Helpers;
using PassFailSample.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PassFailSample.ViewModels
{
    public sealed class UserLoginScreenViewModel : BaseViewModel
    {
        #region Properties

        private IBarcodeScanner Scanner;
        public ICommand EntryCompletedCommand { get; private set; }
        private string _userID;
        public string UserID
        {
            get => this._userID;
            set => this.SetProperty(ref this._userID, value);
        }
        private DataStorage DataStore { get; set; }

        #endregion

        #region Constructor and Init/Deinit

        public UserLoginScreenViewModel(IdleTimeoutTimer timer, Settings settings, DataStorage data) : base(timer, settings)
        {
            this.DataStore = data;
            this.Scanner = DependencyService.Get<IBarcodeScanner>();
            base.Initialize();
            this.EntryCompletedCommand = new Command(p => this.CompleteUserLogin());
        }

        public override void Initialize()
        {
            try
            {
                this.UserID = "";
                base.Initialize();
                MessagingCenter.Subscribe<Page, Throwable>(this, "Exception", this.Scanner_ErrorOccurred, App.Current.MainPage);

                if (this.Scanner.IsConnected || this.Scanner.TryConnect())
                {
                    MessagingCenter.Subscribe<Page, string>(this, "GoodRead", this._Scanner_BarcodeScannedAsync, App.Current.MainPage);
                }
                else if (!this.Settings.BoolDemoMode)
                {
                    Application.Current.MainPage.DisplayAlert("Scanner Not Connected", "The scanner became disconnected", "OK");
                }
            }
            catch (System.Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Exception", ex.Message, "OK");
            }
        }

        public override void Deinitialize()
        {
            this.UserID = "";
            base.Deinitialize();
            MessagingCenter.Unsubscribe<Page, Throwable>(this, "Exception");
            MessagingCenter.Unsubscribe<Page, string>(this, "GoodRead");
        }

        #endregion

        #region Private Methods

        private async void _Scanner_BarcodeScannedAsync(object sender, string e)
        {
            this.UserID = e;
            await Task.Delay(750); // just wait for user display
            this.CompleteUserLogin();
        }

        private void CompleteUserLogin()
        {
            this.DataStore.SetUserID(this.UserID);
            this.NavService.NavigateTo(EnabledScreens.GetNextScreen(this));
        }

        private void Scanner_ErrorOccurred(object sender, Throwable e)
        {
            Application.Current.MainPage.DisplayAlert("Scanner Exception", e.Message, "OK");
        }

        #endregion
    }
}