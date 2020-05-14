using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using MeeterBeeperApp.APIClients.APIClients.Interfaces;
using MeeterBeeperApp.Data.Models;
using MeeterBeeperApp.Helper;
using Newtonsoft.Json;
using Plugin.Geolocator;
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
        #region Private Properties
        private const int DEFAULT_DISTANCE = 1;
        private IDeviceLocationApiClient _deviceLocationApiClient;
        private IPageDialogService _pageDialogService;
        private readonly IDeviceInfo _deviceInfo;
        public string currentDeviceId { set; get; }
        private ISimpleAudioPlayer player;
        private int _distance = DEFAULT_DISTANCE;
        private double latitude = 1;
        private double longitude = 1;
        #endregion

        #region Commands
        public ICommand DecrimentCommand => new Command(DecrimentDistance);
        public ICommand IncrimentCommand => new Command(IncrimentDistance);
        #endregion

        #region Public Propertiues
        public int Distance
        {
            get { return _distance; }
            set { SetProperty(ref _distance, value); }
        }
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
                    location = await UpdateLocation().ConfigureAwait(false);
                }
            }
            if (!location)
            {
                try
                {
                    await _pageDialogService.DisplayAlertAsync("Need location", "MeeterBeeper App need that location", "OK");
                }
                catch (Exception ex)
                {

                }

            }

        }

        private void DecrimentDistance()
        {
            if (Distance > 1)
            {
                Distance--;
            }
        }
        private void IncrimentDistance()
        {
            if (Distance < 100)
            {
                Distance++;
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

                    if (await StartListening())
                    {
                        var request = new Xamarin.Essentials.GeolocationRequest(Xamarin.Essentials.GeolocationAccuracy.Best);
                        var location = await Xamarin.Essentials.Geolocation.GetLocationAsync(request).ConfigureAwait(false);
                        if (location != null)
                        {
                            latitude = location.Latitude;
                            longitude = location.Longitude;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    LocationModel locationModel = new LocationModel
                    {
                        DeviceId = currentDeviceId,
                        Latitude = latitude,
                        Longitude = longitude,
                        Distance = this.Distance
                    };
                    var locationSave = await _deviceLocationApiClient.SaveLocation(locationModel);
                    if (locationSave)
                    {
                        var nearDevices = await _deviceLocationApiClient.GetNearByDevices(locationModel);
                        if (nearDevices.Any())
                        {
                            if (!player.IsPlaying)
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    player.Play();
                                });
                            }
                            return true;
                        }
                        else
                        {
                            if (player.IsPlaying)
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    player.Stop();
                                });

                            }
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
                return false;
            }



        }

        Stream GetStreamFromFile(string filename)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("MeeterBeeperApp." + filename);
            return stream;
        }


        async Task<bool> StartListening()
        {
            if (CrossGeolocator.Current.IsListening)
                return true;

            ///This logic will run on the background automatically on iOS, however for Android and UWP you must put logic in background services. Else if your app is killed the location updates will be killed.
            await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(5), 10, true, new Plugin.Geolocator.Abstractions.ListenerSettings
            {
                ActivityType = Plugin.Geolocator.Abstractions.ActivityType.AutomotiveNavigation,
                AllowBackgroundUpdates = true,
                DeferLocationUpdates = true,
                DeferralDistanceMeters = 1,
                DeferralTime = TimeSpan.FromSeconds(1),
                ListenForSignificantChanges = true,
                PauseLocationUpdatesAutomatically = false
            });
            CrossGeolocator.Current.PositionChanged += Current_PositionChanged;

            return false;
        }

        private void Current_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            latitude = e.Position.Latitude;
            longitude = e.Position.Longitude;
        }
    }
}
