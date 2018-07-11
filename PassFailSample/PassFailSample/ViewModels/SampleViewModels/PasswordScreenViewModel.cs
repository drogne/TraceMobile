using PassFailSample.Helpers;
using PassFailSample.Models;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PassFailSample.ViewModels
{
    public sealed class PasswordScreenViewModel : BaseViewModel
    {
        #region Properties

        private const string defaultPassword = "DMC";
        private CredentialsService _CredentialsService { get; set; }
        public ICommand EntryCompletedCommand { get; private set; }
        private string _password;
        public string Password
        {
            get => this._password;
            set => this.SetProperty(ref this._password, value);
        }

        #endregion

        #region Constructor and Init/Deinit

        public PasswordScreenViewModel(IdleTimeoutTimer timer, Settings settings, CredentialsService credentialsService) : base(timer, settings)
        {
            base.Initialize();
            this._CredentialsService = credentialsService;
            this.EntryCompletedCommand = new Command(async p => await this.CheckLoginMatchAsync().ConfigureAwait(false));
            if (!this._CredentialsService.DoCredentialsExist())
            {
                this._CredentialsService.SaveCredentials(App.userName, defaultPassword);
            }
        }

        public override void Initialize()
        {
            Password = "";
            base.Initialize();
        }

        public override void Deinitialize()
        {
            Password = "";
            base.Deinitialize();
        }

        #endregion

        #region Private Methods

        private bool AreCredentialsCorrect(string username, string password)
        {
            return username == this._CredentialsService.UserName && password == this._CredentialsService.Password;
        }

        private async Task CheckLoginMatchAsync()
        {
            if (this.AreCredentialsCorrect(App.userName, this.Password))
            {
                await this.NavService.NavigateTo(typeof(SettingsScreenViewModel));
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Invalid Password", "The Password you have entered is incorrect", "OK");
            }
        }

        #endregion
    }
}