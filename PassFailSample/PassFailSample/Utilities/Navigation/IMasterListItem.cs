using PassFailSample.ViewModels;
using System;

namespace PassFailSample.Utilities.Navigation
{
    public interface IMasterListItem<out T> where T : BaseViewModel
    {
    }
}
