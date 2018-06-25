using System.Windows.Input;
using Xamarin.Forms;
using PassFailSample.Helpers;
using PassFailSample.Models;
using PassFailSample.IoC;
using System.Collections.Generic;
using System.Linq;
using PassFailSample.Utilities;
using System.Threading.Tasks;

namespace PassFailSample.ViewModels
{
    public sealed class FailureFeedbackScreenViewModel : BaseViewModel
    {
        #region Properties


        // The overall list that will keep track of which view models can be navigated
        // to and displayed in the "master" portion of master/detail
        public List<ButtonItem> AvailableButtons { get; set; }
        private DataStorage DataStore { get; set; }
        public ICommand FailureFeedbackSelectCommand { get; private set; }
        const string OtherLabel = "OTHER";
        private MainPageMasterViewModel MainViewModel { get; }

        #endregion

        #region Constructor and Init/Deinit
        public FailureFeedbackScreenViewModel(DataStorage data, IdleTimeoutTimer timer, Settings settings, MainPageMasterViewModel mainViewModel) : base(timer, settings)
        {
            this.MainViewModel = mainViewModel;
            this.DataStore = data;
            this.FailureFeedbackSelectCommand = new Command(async p =>
            {
                if (p is string)
                {
                    var failureReason = p as string;
                    if (failureReason == OtherLabel)
                    {
                        // TODO Change this once adding multiple text entry screens
                        await this.NavService.NavigateTo(EnabledScreens.GetNextScreen(this)).ConfigureAwait(false);
                    }
                    else
                    {
                        await this.GetFailureReasonAsync(failureReason).ConfigureAwait(false);
                    }
                }
            });

            var ButtonList = this.Settings.ListFailureReasons.Trim().Split(';');
            this.AvailableButtons = new List<ButtonItem>();
            foreach (string element in ButtonList)
            {
                this.AvailableButtons.Add(new ButtonItem(element, this.FailureFeedbackSelectCommand, Color.FromHex(Settings.ColorStandardButton)));
            }
            if (Settings.BoolCustomInputScreenEnabled)
            {
                //this.AvailableButtons.Add(new ButtonItem(OtherLabel, this.FailureFeedbackSelectCommand, Color.FromHex(Settings.ColorStandardButton)));
            }

        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Deinitialize()
        {
            base.Deinitialize();
        }
        #endregion

        #region Methods
        private async Task GetFailureReasonAsync(string failReason)
        {
            this.DataStore.SetStatusReason(failReason);
            await this.DataStore.RecordBarcode();
            // TODO Change this once adding multiple text entry screens
            if (this.MainViewModel.YesNoQuestionList.Any())
            {
                await this.NavService.NavigateTo(typeof(YesNoQuestionScreenViewModel));
            }
            else
            {
                await this.NavService.NavigateTo(typeof(ScanRequestedScreenViewModel));
            }
        }
        #endregion
    }
}