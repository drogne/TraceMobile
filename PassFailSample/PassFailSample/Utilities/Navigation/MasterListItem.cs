using PassFailSample.ViewModels;
using System;

namespace PassFailSample.Utilities.Navigation
{
    public class MasterListItem<T> : IMasterListItem<T> where T : BaseViewModel
    {
        public string DisplayName { get; private set; }

        public MasterListItem(string displayName)
        {
            DisplayName = displayName;
        }
    }
}
