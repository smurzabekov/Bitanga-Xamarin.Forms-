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
    public partial class QRResultPage : ContentPage
    {
        public QRResultPage()
        {
            InitializeComponent();
        }
        public QRResultPage(Result result)
        {
            InitializeComponent();
            Result_Label.Text = $"Result: {result.Text}";
            Format_Label.Text = $"Format: {result.BarcodeFormat}";
        }
    }
}