using PassFailSample.ViewModels;
using System;
using System.Threading.Tasks;

namespace PassFailSample.Utilities.Navigation
{
    public interface INavigationService
    {
        Task PopAsync();
        Task PopModalAsync();
        Task PushModalAsync(Type viewModelType);
        Task PopToRootAsync(bool animate);
        void SwitchDetailPage(Type viewModelType);
        Task NavigateTo(Type viewModelType);
    }
}
