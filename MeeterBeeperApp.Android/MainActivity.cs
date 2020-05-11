using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using MeeterBeeperApp.Droid.Helper;
using MeeterBeeperApp.Helper;
using Plugin.Permissions;
using Prism;
using Prism.Ioc;

namespace MeeterBeeperApp.Droid
{
    [Activity(Label = "Meeter Beeper", Icon = "@mipmap/ic_virus", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            //xamarin forms essential initialization
            Xamarin.Essentials.Platform.Init(this, bundle);
            //FFImage Loading Cached image Renderer inirialization. 
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);
            //user dialog initialization
            UserDialogs.Init(this);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(new AndroidInitializer()));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }



    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
            containerRegistry.RegisterSingleton<IDeviceInfo, DeviceInfo>();
        }
    }
}

