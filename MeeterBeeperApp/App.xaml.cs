using Prism;
using Prism.Ioc;
using MeeterBeeperApp.ViewModels;
using MeeterBeeperApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.Modularity;
using MeeterBeeperApp.APIClients;
using System;

namespace MeeterBeeperApp
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(BeeperPage)}");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<BeeperPage, BeeperPageViewModel>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            Type apiClientModuleType = typeof(APIClientsModule);
            moduleCatalog.AddModule(
            new ModuleInfo()
            {
                ModuleName = apiClientModuleType.Name,
                ModuleType = apiClientModuleType,
                InitializationMode = InitializationMode.WhenAvailable
            });
            base.ConfigureModuleCatalog(moduleCatalog);
        }
    }
}
