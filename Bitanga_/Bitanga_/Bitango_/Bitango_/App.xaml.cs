using Bitango_.Views;
using Bitango_.API;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using System.Threading;
using System.Globalization;
using System.Reactive;
using MonkeyCache.FileStore;
using Xamarin.Essentials;
using Bitango_.Styles;

[assembly: ExportFont("Bitanga_Roboto_Regular.ttf", Alias = "Bitanga_Roboto_Regular")]
[assembly: ExportFont("Poppins-Regular.ttf", Alias = "Poppins-Regular")]
[assembly: ExportFont("Poppins-Medium.ttf", Alias = "Poppins-Medium")]
[assembly: ExportFont("Fontello.ttf", Alias = "Fontello")]
namespace Bitango_
{
    public partial class App : Application
    {
        const int smallWidthResolution = 768;
        const int smallHeightResolution = 1280;
        public App()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InstalledUICulture; //Select language from device locale
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            InitializeComponent();
            LoadStyles();

            Plugin.Media.CrossMedia.Current.Initialize();
            Barrel.ApplicationId = "BitangaCache"; //Initialize bitanga cache barrel

            bool isLoggedIn = Current.Properties.ContainsKey("IsLoggedIn") ? Convert.ToBoolean(Current.Properties["IsLoggedIn"]) : false;
            if (!isLoggedIn)
            {
                MainPage = new NavigationPage(new LoginPage()); //Load if Not Logged In
            }
            else
            {
                MainPage = new MainPage(); //Load if Logged In
            }
        }

        void LoadStyles()
        {
            if (IsASmallDevice())
            {
                dictionary.MergedDictionaries.Add(SmallDevicesStyle.SharedInstance);
            }
            else
            {
                dictionary.MergedDictionaries.Add(GeneralDevicesStyle.SharedInstance);
            }
        }
        public static bool IsASmallDevice()
        {
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            var width = mainDisplayInfo.Width;

            var height = mainDisplayInfo.Height;
            return (width <= smallWidthResolution || height <= smallHeightResolution);
        }
    }
}
