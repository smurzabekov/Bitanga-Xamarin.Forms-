using Bitango_.API;
using Bitango_.Models;
using Rg.Plugins.Popup.Pages;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bitango_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QRBasketPopup : PopupPage
    {
        public QRBasketPopup()
        {
        }
        public QRBasketPopup(ObservableCollection<Dish> dishes)
        {
            InitializeComponent();

            qrcode.AutomationId = "zxingBarcodeImageView";

            qrcode.BarcodeFormat = ZXing.BarcodeFormat.QR_CODE;
            qrcode.BarcodeOptions.Width = 250;
            qrcode.BarcodeOptions.Height = 250;
            qrcode.BarcodeOptions.Margin = 1;

            string s = Application.Current.Properties["ID"].ToString() + " " +
                Application.Current.Properties["Username"].ToString().Replace(' ', '_') + " " +
                "88005553535" + " " +
                ApiAccess.current_user.wallet.ID + " ";
            for (int i = 0; i < dishes.Count; i++)
            {
                s += dishes[i].ID + " ";
            }

            qrcode.BarcodeValue = s;

        }
    }
}