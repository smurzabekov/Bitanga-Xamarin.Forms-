using System;
using System.Collections.Generic;
using System.Linq;
using Bitango_.Models;
using CarouselView.FormsPlugin.iOS;
using Foundation;
using UIKit;

namespace Bitango_.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.SetFlags("Shell_Experimental", "Visual_Experimental", "CollectionView_Experimental");
            global::Xamarin.Forms.Forms.Init();

            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            Rg.Plugins.Popup.Popup.Init();
            CarouselViewRenderer.Init();
            Xamarin.FormsMaps.Init();

            LoadApplication(new App());
            global::Xamarin.Auth.Presenters.XamarinIOS.AuthenticationConfiguration.Init();
            app.SetStatusBarStyle(UIStatusBarStyle.LightContent, true);
            return base.FinishedLaunching(app, options);
        }
        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            var uri = new Uri(url.AbsoluteString);
            AuthenticationState.Authenticator.OnPageLoading(uri);
            return true;
        }
    }
}
