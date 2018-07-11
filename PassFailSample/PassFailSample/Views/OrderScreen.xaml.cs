using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PassFailSample.Utilities.Navigation;
using PassFailSample.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PassFailSample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderScreen : ContentPage, IViewFor
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
        public OrderScreen(OrderViewModel vm)
        {
            InitializeComponent();
            this.ViewModel = vm;
        }
    }
}