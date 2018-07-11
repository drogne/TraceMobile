using PassFailSample.Utilities.Navigation;
using PassFailSample.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PassFailSample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PasswordScreen : ContentPage, IViewFor
    {
        private BaseViewModel _ViewModel;
        public BaseViewModel ViewModel
        {
            get => this._ViewModel;
            set
            {
                this._ViewModel = value;
                BindingContext = this._ViewModel;
            }
        }

        public PasswordScreen(PasswordScreenViewModel vm)
        {
            InitializeComponent();
            this.ViewModel = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            PasswordEntry.Focus(); //select when appears
        }
    }
}