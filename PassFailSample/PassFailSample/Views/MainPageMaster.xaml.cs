using PassFailSample.Utilities.Navigation;
using PassFailSample.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PassFailSample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageMaster : ContentPage, IViewFor
    {
        public ListView ListView;

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

        public MainPageMaster(MainPageMasterViewModel vm)
        {
            InitializeComponent();
            this.ViewModel = vm;
            this.ListView = MenuItemsListView;
        }
    }
}