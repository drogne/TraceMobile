using PassFailSample.ViewModels;
using System;

namespace PassFailSample.Utilities.Navigation
{
    // IViewFor should only be a marker interface
    public interface IViewFor
    {
        BaseViewModel ViewModel { get; set; }
    }
}
