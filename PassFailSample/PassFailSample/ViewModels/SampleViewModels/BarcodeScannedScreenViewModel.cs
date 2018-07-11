using System.Windows.Input;
using PassFailSample.Helpers;
using PassFailSample.Models;
using Xamarin.Forms;
using PassFailSample.IoC;
using Autofac;
using PassFailSample.Utilities.Navigation;
using PassFailSample.Utilities;
using System.Collections.Generic;

namespace PassFailSample.ViewModels
{
    public sealed class BarcodeScannedScreenViewModel : BaseViewModel
    {
        #region Properties

        private string _scannedBarcode;
        public string ScannedBarcode
        {
            get => this._scannedBarcode;
            set => this.SetProperty(ref this._scannedBarcode, value);
        }

        public List<ButtonItem> AvailableButtons { get; set; }
        public ICommand EntryCompleteCommand { get; private set; }
        private DataStorage DataStore { get; set; }
        private MainPageMasterViewModel MainViewModel { get; set; }

        #endregion

        #region Constructor and Init/Deinit

        public BarcodeScannedScreenViewModel(IdleTimeoutTimer timer, Settings settings, DataStorage data, MainPageMasterViewModel mainViewModel) : base(timer, settings)
        {
            this.DataStore = data;
            this.MainViewModel = mainViewModel;
            this.EntryCompleteCommand = new Command(p =>
            {
                if (p is string temp)
                {
                    this.CompletePage(temp != this.Settings.TextFailButton, temp);
                }
            });

            var ButtonList = this.Settings.ListAdditionalStatusOptions.Trim().Split(';');
            this.AvailableButtons = new List<ButtonItem>
            {
                new ButtonItem(this.Settings.TextPassButton, this.EntryCompleteCommand, Color.FromHex(Settings.ColorPass)),
                new ButtonItem(this.Settings.TextFailButton, this.EntryCompleteCommand, Color.FromHex(Settings.ColorFail))
            };
            foreach (string element in ButtonList)
            {
                if (element != "")
                {
                    this.AvailableButtons.Add(new ButtonItem(element, this.EntryCompleteCommand, Color.FromHex(Settings.ColorStandardButton)));
                }
            }
        }
        
        public override void Initialize()
        {
            base.Initialize();
            this.ScannedBarcode = string.IsNullOrEmpty(this.DataStore.Barcode) ? "Nothing was Scanned" : $"Was this a {this.DataStore.Barcode}?";
        }

        public override void Deinitialize()
        {
            base.Deinitialize();
        }

        #endregion

        #region Methods

        private async void CompletePage(bool stationPass, string status)
        {

            this.DataStore.SetStatus(status);
            if (stationPass)
            {
                await this.DataStore.RecordBarcode();
                //this.MainViewModel.YesNoQuestionList.Add("Were all checks completed as prescribed?");
                //this.MainViewModel.YesNoQuestionList.Add("Did all checks perform to spec?");
                //this.MainViewModel.YesNoQuestionList.Add("Is the code date acceptable for shipment?");
                await this.NavService.NavigateTo(typeof(YesNoQuestionScreenViewModel)); // TODO change this after adding more input screens
            }
            else
            {
                // If this is the last page in the sequence, then record the barcode
                if (EnabledScreens.IsLastScreen(this))
                {
                    await this.DataStore.RecordBarcode();
                }
                await this.NavService.NavigateTo(typeof(ScanRequestedScreenViewModel));
            }
        }

        #endregion

    }
}