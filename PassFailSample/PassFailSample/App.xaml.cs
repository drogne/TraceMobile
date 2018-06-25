using System;
using Autofac;
using PassFailSample.IoC;
using PassFailSample.Models;
using PassFailSample.Views;
using PassFailSample.Helpers;
using Xamarin.Forms;

namespace PassFailSample
{
    
    public partial class App : Application
    {
        private IBarcodeScanner Scanner { get; set; }
        private DataStorage DataStore { get; set; }
        public const string userName = "Congex Sample App";

        public App()
        {
            InitializeComponent();

            IoCContainer.Initialize();
            using (var scope = IoCContainer.Container.BeginLifetimeScope())
            {
                var master = scope.Resolve<MainPageMaster>();
                MainPage = new MasterDetailPage()
                {
                    Master = scope.Resolve<MainPageMaster>(),
                    // Wrap the Details Page with a Navigation Page
                    Detail = new NavigationPage(scope.Resolve<HomeScreen>())
                };

                this.DataStore = scope.Resolve<DataStorage>();
                this.DataStore.Initialize();
            }
            this.Scanner = DependencyService.Get<IBarcodeScanner>();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            try
            {
                this.AttemptConnect();
            }
            catch (Exception)
            {
                // If we did not properly connect, then we will have a null reference for the scanner object. Ignore this error since we are already disconnected. 
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            // TODO implement logic to determine if we are connected, and only if we are connected do the below.
            try
            {
                Scanner?.Disconnect();
            }
            catch (Exception)
            {
                // If we did not properly connect, then we will have a null reference for the scanner object. Ignore this error since we are already disconnected. 
            }
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            // TODO implement logic to determine if we are connected, and only if we are NOT connected do the below.
            this.AttemptConnect();
            this.DataStore.Initialize();
            // TODO - do we want to do this? do we need to go back to the main page??
        }

        protected void AttemptConnect()
        {
            if (!this.Scanner.TryConnect())
            {
                Application.Current.MainPage.DisplayAlert(AppConstants.SCANNER_DISCONNECTED_TITLE, AppConstants.SCANNER_DISCONNECTED_MSG, "OK");
            }
        }
    }
}
