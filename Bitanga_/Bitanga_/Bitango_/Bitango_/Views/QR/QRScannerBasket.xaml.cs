using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;

namespace Bitango_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QRScannerBasket : ContentPage
    {
        public QRScannerBasket()
        {
            InitializeComponent();
            scanView.IsScanning = true;
        }
        public void scanView_OnScanResult(Result result)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Navigation.PushPopupAsync(new LoaderPopup(), true);
                scanView.IsScanning = false;
                await Navigation.PushAsync(new QRResultBasketPage(result));
                await Navigation.PopPopupAsync();
            });
        }
    }
}