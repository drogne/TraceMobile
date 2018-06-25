using PassFailSample.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PassFailSample.Utilities.Navigation
{
    public class NavigationService : INavigationService
    {
        #region Properties

        INavigation FormsNavigation
        {
            get
            {
                var tabController = Application.Current.MainPage as TabbedPage;
                var masterController = Application.Current.MainPage as MasterDetailPage;

                // First check to see if we're on a tabbed page, then master detail, finally go to overall fallback
                return tabController?.CurrentPage?.Navigation ??
                    (masterController?.Detail as TabbedPage)?.CurrentPage?.Navigation ?? // special consideration for a tabbed page inside master/detail
                    masterController?.Detail?.Navigation ??
                    Application.Current.MainPage.Navigation;
            }
        }

        // View model to view lookup - making the assumption that view model to view will always be 1:1
        readonly Dictionary<BaseViewModel, IViewFor> _viewModelViewDictionary = new Dictionary<BaseViewModel, IViewFor>();

        // Because we're going to do a hard switch of the page, either return
        // the detail page, or if that's null, then the current main page       
        Page DetailPage
        {
            get
            {
                var masterController = Application.Current.MainPage as MasterDetailPage;
                return masterController?.Detail ?? Application.Current.MainPage;
            }
            set
            {
                if (Application.Current.MainPage is MasterDetailPage masterController)
                {
                    masterController.Detail = value;
                    masterController.IsPresented = false;
                }
                else
                {
                    Application.Current.MainPage = value;
                }
            }
        }

        #endregion

        #region Constructor

        public NavigationService(IEnumerable<IViewFor> viewList)
        {
            foreach(var view in viewList)
            {
                this._viewModelViewDictionary.Add(view.ViewModel, view);
            }
        }

        #endregion

        #region Utility Methods
        public void SwitchDetailPage(Type viewModelType)
        {
            this.InitCurrentViewModel(false);
            var viewModel = this._viewModelViewDictionary.FirstOrDefault(vm => vm.Key.GetType() == viewModelType).Key;
            var view = this.GetAssociatedView(viewModel);

            Page newDetailPage;

            // Tab pages shouldn't go into navigation pages
            if (view is TabbedPage)
                newDetailPage = (Page)view;
            else
                newDetailPage = new NavigationPage((Page)view);

            this.DetailPage = newDetailPage;
            this.InitCurrentViewModel(true);
        }

        IViewFor GetAssociatedView(BaseViewModel viewModel)
        {
            // look up what type of view it corresponds to
            return _viewModelViewDictionary[viewModel];
        }

        /// <summary>
        /// Will initailize or de-iniitalize the current View Model based on the <param name="init"></param>
        /// </summary>
        /// <param name="init"></param>
        private void InitCurrentViewModel(bool init)
        {
            var currentViewModel = FormsNavigation.NavigationStack.Last().BindingContext as BaseViewModel;
            if (init)
            {
                currentViewModel.Initialize();
            }
            else
            {
                currentViewModel.Deinitialize();
            }
        }

        #endregion

        #region Pop Methods

        public async Task PopAsync()
        {
            this.InitCurrentViewModel(false);
            await this.FormsNavigation.PopAsync(true);
            this.InitCurrentViewModel(true);
        }

        public async Task PopModalAsync()
        {
            this.InitCurrentViewModel(false);
            await this.FormsNavigation.PopModalAsync(true);
            this.InitCurrentViewModel(true);
        }

        public async Task PopToRootAsync(bool animate)
        {
            try
            {
                var currentPage = (IViewFor) FormsNavigation.NavigationStack.First();
                await PopTo(currentPage.GetType());
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Exception", ex.Message, "OK");
            }
        }

        private async Task PopTo(Type viewModelType)
        {
            try
            {
                var pagesToRemove = new List<Page>();
                var upper = FormsNavigation.NavigationStack.Count;
                this.InitCurrentViewModel(false);

                // Loop through the nav stack backwards starting right below the top stack item
                for (int i = upper - 2; i >= 0; i--)
                {
                    var currentPage = this.FormsNavigation.NavigationStack[i] as IViewFor;

                    // Stop the whole show if one of the pages isn't an IViewFor
                    if (currentPage == null)
                        return;

                    //var strongTypedPaged = currentPage as IViewFor;

                    // If we hit the view model type, break out
                    if (currentPage.ViewModel.GetType() == viewModelType)
                        break;

                    // Finally - always add to the list
                    pagesToRemove.Add(currentPage as Page);
                }

                // Remove all pages
                foreach (var item in pagesToRemove)
                {
                    this.FormsNavigation.RemovePage(item);
                }
                // Do a popasync to transition to the next page
                await this.FormsNavigation.PopAsync(true);
                this.InitCurrentViewModel(true);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Navigation Error", ex.Message, "Ok");
            }
        }

        #endregion

        #region Push Methods

        private async Task PushAsync(Type viewModelType)
        {
            try
            {
                var viewModel = this._viewModelViewDictionary.FirstOrDefault(vm => vm.Key.GetType() == viewModelType).Key;
                this.InitCurrentViewModel(false);
                var view = this.GetAssociatedView(viewModel);
                var viewPage = (Page)view;
                if (this.FormsNavigation != null && viewPage != null)
                {
                    await this.FormsNavigation.PushAsync(viewPage);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                await Application.Current.MainPage.DisplayAlert("Exception", ex.Message, "Ok");
#endif
            }
            finally
            {
                this.InitCurrentViewModel(true);
            }
        }

        public async Task PushModalAsync(Type viewModelType)
        {
            var viewModel = this._viewModelViewDictionary.FirstOrDefault(vm => vm.Key.GetType() == viewModelType).Key;
            this.InitCurrentViewModel(false);
            var view = this.GetAssociatedView(viewModel);

            // Most likely we're going to want to put this into a navigation page so we can have a title bar on it
            var nv = new NavigationPage((Page)view);

            await FormsNavigation.PushModalAsync(nv);
            this.InitCurrentViewModel(true);
        }

        #endregion

        #region PushOrPop

        public async Task NavigateTo(Type viewModelType)
        {
            var currentPage = (IViewFor) FormsNavigation.NavigationStack.Last();
            if (currentPage.ViewModel.GetType() != viewModelType) // If navigating to the active screen, do nothing and prevent exceptions
            {
                var inStack = false;
                var pagesCount = FormsNavigation.NavigationStack.Count;
                foreach (IViewFor page in FormsNavigation.NavigationStack)
                {
                    if (page.ViewModel.GetType() == viewModelType)
                    {
                        inStack = true;
                        break;
                    }
                }
                if (inStack)
                {
                    await this.PopTo(viewModelType);
                }
                else
                {
                    await this.PushAsync(viewModelType);
                }
            }
        }

        #endregion

    }
}
