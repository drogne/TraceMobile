using PassFailSample.Helpers;
using PassFailSample.Models;

namespace PassFailSample.ViewModels
{
    public sealed class SettingsScreenViewModel : BaseViewModel
    {
        #region Properties

        private CredentialsService _CredentialsService { get; set; }
        private string _password;
        public string Password
        {
            get => this._password;
            set => this.SetProperty(ref this._password, value);
        }

        #endregion

        #region Constructor and Init/Deinit

        public SettingsScreenViewModel(IdleTimeoutTimer timer, Settings settings, CredentialsService credentialsService) : base(timer, settings)
        {
            this._CredentialsService = credentialsService;
        }

        public override void Initialize()
        {
            base.Initialize();
            Password = this._CredentialsService.Password;
        }

        public override void Deinitialize()
        {
            base.Deinitialize();
            this._CredentialsService.SaveCredentials(App.userName, this._password);
        }

        #endregion

    }
}
