using System;
using PassFailSample.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PassFailSample.Utilities.Navigation;

namespace PassFailSample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanRequestedScreen : ContentPage, IViewFor
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

        public ScanRequestedScreen(ScanRequestedScreenViewModel vm)
        {
            InitializeComponent();
            this.ViewModel = vm;
        }
    }
}