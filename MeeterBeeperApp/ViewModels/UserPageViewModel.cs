using MeeterBeeperApp.APIClients.APIClients.Interfaces;
using MeeterBeeperApp.Data;
using MeeterBeeperApp.Data.Helpers;
using MeeterBeeperApp.Data.Models;
using MeeterBeeperApp.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MeeterBeeperApp.ViewModels
{
    public class BeeperPageViewModel : ViewModelBase
    {
        private const string JOIN_URL = "https://github.com/join";
        #region Private Properties
        private IUserApiClient _userApiClient;
        private IPageDialogService _pageDialogService;
        public string UserName { set; get; }

        #endregion

        #region Commands

        public ICommand SeeUserDetailsCommand => new Command(GetUserDetails);
        public ICommand GithubSignupCommand => new Command(OpenGithubSingupUrl);

        

        #endregion
        public BeeperPageViewModel(INavigationService navigationService,
            IUserApiClient userApiClient,
            IPageDialogService pageDialogService)
            : base(navigationService)
        {

            _userApiClient = userApiClient;
            _pageDialogService = pageDialogService;

             Title = String.Empty;
        }
        private async void GetUserDetails()
        {
            IsBusy = true;
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                User user = await _userApiClient.GetUser(UserName);
                if (user != null)
                {
                    Preferences.Set(Constants.Keys.User, JsonConvert.SerializeObject(user));
                }
            }
            else
            {
                UserDialogsHelper.ShowNotification("Username cannot be empty.", NotificationTypeEnum.Error);
            }
            IsBusy = false;
        }

        private async void OpenGithubSingupUrl()
        {
             await Launcher.OpenAsync(new Uri(JOIN_URL));
        }
    }
}
