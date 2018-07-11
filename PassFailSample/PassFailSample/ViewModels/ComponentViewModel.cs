using PassFailSample.Helpers;
using PassFailSample.Models;
using Xamarin.Forms;

namespace PassFailSample.ViewModels
{
    public class ComponentViewModel : BaseViewModel
    {
        public Session Session { get; private set; }
        private IBarcodeScanner Scanner { get; set; }
        public ComponentViewModel(IdleTimeoutTimer timer, Settings Settings, Session session) : base(timer, Settings)
        {
            this.Session = session;
            this.Scanner = DependencyService.Get<IBarcodeScanner>();
        }
    }
}