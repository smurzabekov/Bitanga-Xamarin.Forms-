using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace Bitango_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QRGuestCardPopup : PopupPage
    {
        public QRGuestCardPopup()
        {
            InitializeComponent();

            qrcode.AutomationId = "zxingBarcodeImageView";

            qrcode.BarcodeFormat = ZXing.BarcodeFormat.QR_CODE;
            qrcode.BarcodeOptions.Width = 250;
            qrcode.BarcodeOptions.Height = 250;
            qrcode.BarcodeOptions.Margin = 1;
            qrcode.BarcodeValue = Application.Current.Properties["ID"].ToString(); 

        }
    }
}