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
    public partial class LoaderPopup : PopupPage
    {
        public LoaderPopup()
        {
            InitializeComponent();
            CloseWhenBackgroundIsClicked = false;
        }
    }
}