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
    public sealed class YesNoQuestionScreenViewModel : BaseViewModel
    {
        const string OtherLabel = "OTHER";
        #region Properties
        // The overall list that will keep track of which view models can be navigated
        // to and displayed in the "master" portion of master/detail
        public List<ButtonItem> AvailableButtons { get; set; }
        private DataStorage DataStore { get; set; }
        private MainPageMasterViewModel MainViewModel { get; }

        private string _YesNoQuestion;

        public string YesNoQuestion
        {
            get => this._YesNoQuestion;
            set => this.SetProperty(ref this._YesNoQuestion, value);
        }
        public ICommand YesCommand { get; private set; }
        public ICommand NoCommand { get; private set; }
        public ICommand NACommand { get; private set; }
        #endregion

        #region Constructor and Init/Deinit
        public YesNoQuestionScreenViewModel(DataStorage data, IdleTimeoutTimer timer, Settings settings, MainPageMasterViewModel mainViewModel) : base(timer, settings)
        {
            this.DataStore = data;
            this.MainViewModel = mainViewModel;

            this.YesCommand = new Command(p => this.UpdateQuestion());
            this.NoCommand = new Command(p => this.NavService.NavigateTo(typeof(CustomInputScreenViewModel)));
            this.NACommand = new Command(p => this.NavService.NavigateTo(typeof(FailureFeedbackScreenViewModel)));
        }

        public override void Initialize()
        {
            base.Initialize();
            this.UpdateQuestion();
        }

        public override void Deinitialize()
        {
            base.Deinitialize();
        }
        #endregion

        #region Methods

        private void UpdateQuestion()
        {
            if (this.MainViewModel.YesNoQuestionList.Any())
            {
                this.YesNoQuestion = this.MainViewModel.YesNoQuestionList.First();
                this.MainViewModel.YesNoQuestionList.RemoveAt(0);
            }
            else
            {
                this.NavService.NavigateTo(typeof(ScanRequestedScreenViewModel));
            }
        }

        #endregion
    }
}