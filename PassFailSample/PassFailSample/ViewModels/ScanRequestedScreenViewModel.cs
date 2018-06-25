using Autofac;
using Java.Lang;
using PassFailSample.Helpers;
using PassFailSample.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PassFailSample.ViewModels
{
    public sealed class ScanRequestedScreenViewModel : BaseViewModel
    {
        #region Properties

        private bool NavigationInProcess;
        private DataStorage DataStore { get; set; }
        private IBarcodeScanner Scanner { get; set; }
        public ICommand NavigateToBarcodeScannedScreenCommand { get; private set; }
        private MainPageMasterViewModel MainViewModel { get; }

        // QUESTION: Why do i need a field and a property to just have a property that get IChangeNotification?
        private ImageSource _BarcodeImage;
        public ImageSource BarcodeImage
        {
            get => this._BarcodeImage;
            set => this.SetProperty(ref this._BarcodeImage, value);
        }

        #endregion

        #region Constructor and Init/Deinit

        public ScanRequestedScreenViewModel(DataStorage data, IdleTimeoutTimer timer, Settings settings, MainPageMasterViewModel mainViewModel) : base(timer, settings)
        {
            this.MainViewModel = mainViewModel;
            this.DataStore = data;
            // For simulation only as a way to proceed through the workflow without scanning a code
            this.NavigateToBarcodeScannedScreenCommand = new Command(async p =>
            {
                if (!NavigationInProcess && Settings.BoolDemoMode) // This prevents the double call to navigation services
                {
                    this.DataStore.SetBarcode(string.Empty); // Set the barcode to an empty string, since nothing was scanned
                    await this.CompletePageAsync();
                }
            });
            this.Scanner = DependencyService.Get<IBarcodeScanner>();
        }
        public override void Initialize()
        {
            base.Initialize();
            this.NavigationInProcess = false;
            this.MainViewModel.YesNoQuestionList.Clear();
            MessagingCenter.Subscribe<Page, Throwable>(this, "Exception", this.Scanner_ErrorOccurred, App.Current.MainPage);

            try
            {
                if (this.Scanner.IsConnected || this.Scanner.TryConnect())
                {
                    MessagingCenter.Subscribe<Page, string>(this, "GoodRead", this._Scanner_BarcodeScanned, App.Current.MainPage);
                    MessagingCenter.Subscribe<Page, MemoryStream>(this, "ImageReceived", this._Image_Arrived, App.Current.MainPage);
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
            base.Deinitialize();
            MessagingCenter.Unsubscribe<Page, Throwable>(this, "Exception");
            MessagingCenter.Unsubscribe<Page, string>(this, "GoodRead");
            MessagingCenter.Unsubscribe<Page, MemoryStream>(this, "ImageReceived");
        }

        #endregion

        #region Methods

        private async void _Scanner_BarcodeScanned(object sender, string e)
        {
            this.DataStore.SetBarcode(e);
            await this.CompletePageAsync();
        }

        private void _Image_Arrived(object sender, MemoryStream stream)
        {
            stream.Position = 0;
            this.BarcodeImage = Xamarin.Forms.ImageSource.FromStream(() => stream);
            // TODO do I need to release the stream? or close it?
        }

        private void Scanner_ErrorOccurred(object sender, Throwable e)
        {
            Application.Current.MainPage.DisplayAlert("Scanner Exception", e.Message, "OK");
        }

        private async Task CompletePageAsync()
        {
            // If this is the last page in the sequence, then record the barcode
            if (EnabledScreens.IsLastScreen(this))
            {
                await this.DataStore.RecordBarcode();
            }
            await this.NavService.NavigateTo(EnabledScreens.GetNextScreen(this));
            this.NavigationInProcess = true;
        }

        #endregion
    }
}