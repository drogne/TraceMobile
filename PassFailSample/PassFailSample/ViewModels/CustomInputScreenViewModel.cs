using System;
using System.Linq;
using System.Windows.Input;
using PassFailSample.Helpers;
using PassFailSample.Models;
using Xamarin.Forms;
using PassFailSample.Utilities.Navigation;

namespace PassFailSample.ViewModels
{
    public sealed class CustomInputScreenViewModel : BaseViewModel
    {
        #region Properties

        public ICommand EntryCompletedCommand { get; private set; }
        private DataStorage DataStore { get; set; }

        private string _failureReasonString;
        public string FailureReasonString
        {
            get => this._failureReasonString;
            set => this.SetProperty(ref this._failureReasonString, value);
        }

        private MainPageMasterViewModel MainViewModel { get; }
        #endregion

        #region Constructor and Init/Deinit

        public CustomInputScreenViewModel(DataStorage data, IdleTimeoutTimer timer, Settings settings, MainPageMasterViewModel mainViewModel) : base(timer, settings)
        {
            this.DataStore = data;
            this.MainViewModel = mainViewModel;
            this.EntryCompletedCommand = new Command(async p =>
            {
                // TODO Change this once adding multiple text entry screens
                this.DataStore.SetStatusReason(this.FailureReasonString);
                await this.DataStore.RecordBarcode();
                if (this.MainViewModel.YesNoQuestionList.Any())
                {
                    await this.NavService.NavigateTo(typeof(YesNoQuestionScreenViewModel));
                }
                else
                {
                    await this.NavService.NavigateTo(typeof(ScanRequestedScreenViewModel));
                }
            });
        }

        public override void Initialize()
        {
            base.Initialize();
            this.FailureReasonString = string.Empty;
        }

        public override void Deinitialize()
        {
            base.Deinitialize();
        }

        #endregion
    }
}