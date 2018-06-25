using System.Windows.Input;
using PassFailSample.Helpers;
using Xamarin.Forms;
using PassFailSample.Models;

namespace PassFailSample.ViewModels
{

    public sealed class HomeScreenViewModel : BaseViewModel
    {
        private IBarcodeScanner Scanner;
        private bool NavigationInProcess;
        public ICommand NavigateToScanRequestedScreenCommand { get; private set; }
        public ICommand BypassCommand { get; private set; }

        public HomeScreenViewModel(IdleTimeoutTimer timer, Settings Settings) : base(timer, Settings)
        {
            this.Scanner = DependencyService.Get<IBarcodeScanner>();

            // TODO add option to press trigger to proceed
            this.NavigateToScanRequestedScreenCommand = new Command(p => this.AssessConnection());
            this.BypassCommand = new Command(p => this.CompletePage());

            // HACK not sure how to get this to fire otherwise
            this.Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
            this.NavigationInProcess = false;
        }

        public override void Deinitialize()
        {
            base.Deinitialize();
        }

        private void AssessConnection()
        {
            if (this.Scanner.IsConnected || this.Scanner.TryConnect() || this.Settings.BoolDemoMode)
            {
                this.CompletePage();
            }
            else
            {
                Application.Current.MainPage.DisplayAlert(AppConstants.SCANNER_DISCONNECTED_TITLE, AppConstants.SCANNER_DISCONNECTED_MSG, "Ok");
            }
        }
        private void CompletePage()
        {
            if (!NavigationInProcess) // This prevents the double call to navigation services
            {
                this.NavService.NavigateTo(EnabledScreens.GetNextScreen(this));
                //this.NavService.NavigateTo(typeof(ScanRequestedScreenViewModel));
                this.NavigationInProcess = true;
            }
        }
    }
}
