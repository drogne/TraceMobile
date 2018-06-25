using PassFailSample.Helpers;
using PassFailSample.Models;
using PassFailSample.Utilities.Navigation;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace PassFailSample.ViewModels
{
    public class MainPageMasterViewModel : BaseViewModel
    {
        #region Properties

        // The overall list that will keep track of which view models can be navigated
        // to and displayed in the "master" portion of master/detail
        public List<IMasterListItem<BaseViewModel>> AvailablePages { get; set; }

        private string _title;
        public string Title
        {
            get => this._title;
            set
            {
                this._title = value;
                this.SetProperty(ref this._title, value);
            }
        }

        public List<string> YesNoQuestionList { get; }

        public ICommand NavigateCommand { get; private set; }
        private CredentialsService CredentialsService {get; set; }

        #endregion

        #region Constructor

        public MainPageMasterViewModel(IdleTimeoutTimer timer, Settings settings, CredentialsService credentialsService) : base(timer, settings)
        {
            this.YesNoQuestionList = new List<string>();
            // This is where we add the view models we can navigate to
            // And the descriptions to be displayed
            AvailablePages = new List<IMasterListItem<BaseViewModel>>
            {
                new MasterListItem<HomeScreenViewModel>("Scan Barcode"),
                new MasterListItem<PasswordScreenViewModel>("Settings")
            };
            Title = "Navigation";
            this.CredentialsService = credentialsService;
            this.NavigateCommand = new Command(selectedItem => GoToSelectedScreen(selectedItem));
        }

        #endregion

        #region Private Methods

        private void GoToSelectedScreen(object selectedItem)
        {
            // Get the selected item from the command
            if (selectedItem is IMasterListItem<BaseViewModel> itemToNavigate)
            {
                // Get the view model type
                var viewModelType = itemToNavigate.GetType().GenericTypeArguments[0];

                // Switch Detail Page
                this.NavService.SwitchDetailPage(viewModelType);
            }
        }

        #endregion
    }
}
