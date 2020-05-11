using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using MeeterBeeperApp.APIClients.APIClients.Interfaces;
using MeeterBeeperApp.Data.Models;
using MeeterBeeperApp.Helper;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Plugin.SimpleAudioPlayer;
using Prism.AppModel;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace MeeterBeeperApp.ViewModels
{
    public class BeeperPageViewModel : ViewModelBase, IPageLifecycleAware
    {
        private const string JOIN_URL = "https://github.com/join";
        #region Private Properties
        private IDeviceLocationApiClient _deviceLocationApiClient;
        private IPageDialogService _pageDialogService;
        private readonly IDeviceInfo _deviceInfo;
        public string currentDeviceId { set; get; }
        private ISimpleAudioPlayer player;
        #endregion

        #region Commands
        public ICommand GithubSignupCommand => new Command(OpenGithubSingupUrl);
        #endregion
        public BeeperPageViewModel(INavigationService navigationService,
            IDeviceLocationApiClient deviceLocationApiClient,
            IPageDialogService pageDialogService,
            IDeviceInfo deviceInfo)
            : base(navigationService)
        {

            _deviceLocationApiClient = deviceLocationApiClient;
            _pageDialogService = pageDialogService;
            _deviceInfo = deviceInfo;
            Title = String.Empty;
        }

        public async override void Initialize(INavigationParameters parameters)
        {
            currentDeviceId = _deviceInfo.GetDeviceId();


            var stream = GetStreamFromFile("audio.wav");
            if (stream != null)
            {
                player = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
                player.Load(stream);
            }
            base.Initialize(parameters);
        }

        public async void OnAppearing()
        {
            bool location = true;
            while (location)
            {
                if (!player.IsPlaying)
                {
                    location = await UpdateLocation();
                }
            }
            if (!location)
            {
                await _pageDialogService.DisplayAlertAsync("Need location", "MeeterBeeper App need that location", "OK");
            }

        }

        public void OnDisappearing()
        {

        }

        private async Task<bool> UpdateLocation()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync<LocationPermission>();
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        await _pageDialogService.DisplayAlertAsync("Need location", "MeeterBeeper App need that location", "OK");
                    }

                    status = await CrossPermissions.Current.RequestPermissionAsync<LocationPermission>();
                }

                if (status == PermissionStatus.Granted)
                {

                    var request = new Xamarin.Essentials.GeolocationRequest(Xamarin.Essentials.GeolocationAccuracy.Best);
                    var location = await Xamarin.Essentials.Geolocation.GetLocationAsync(request);

                    if (location != null)
                    {
                        LocationModel locationModel = new LocationModel
                        {
                            DeviceId = currentDeviceId,
                            Latitude = location.Latitude,
                            Longitude = location.Longitude,
                            Distance = 2
                        };
                        var locationSave = await _deviceLocationApiClient.SaveLocation(locationModel);
                        if (locationSave)
                        {
                            var nearDevices = await _deviceLocationApiClient.GetNearByDevices(locationModel);
                            if (nearDevices.Any())
                            {
                                if (!player.IsPlaying)
                                {
                                    player.Play();
                                }
                                return true;
                            }
                            else
                            {
                                player.Stop();
                                return true;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                else if (status != PermissionStatus.Unknown)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return true;
            }



        }

        private async void OpenGithubSingupUrl()
        {
            await Xamarin.Essentials.Launcher.OpenAsync(new Uri(JOIN_URL));
        }

        Stream GetStreamFromFile(string filename)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("MeeterBeeperApp." + filename);
            return stream;
        }
    }
}
