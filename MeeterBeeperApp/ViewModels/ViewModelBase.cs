using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeeterBeeperApp.ViewModels
{
    public class ViewModelBase : BindableBase, IInitialize, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; private set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private bool _isBusy = false;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value, SetShowControlsProperty); }
        }
        private bool _isShowControls = true;
        public bool IsShowControls
        { 
            get { return _isShowControls; }
            set { SetProperty(ref _isShowControls, value); }
        }

        private void SetShowControlsProperty()
        {
            IsShowControls = !_isBusy;
        }
        public ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public virtual void Initialize(INavigationParameters parameters)
        {}

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {}

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {}

        public virtual void Destroy()
        {}
    }
}
