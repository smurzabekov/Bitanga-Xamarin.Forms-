using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bitango_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SupportPage : ContentPage
    {
        public SupportPage()
        {
            InitializeComponent();
            Shell.SetNavBarHasShadow(this, false); // Delete shadow undel TitleView
        }
        private async void Support_Clicked(object sender, EventArgs e)
        {
            Frame frame = ((Frame)sender);
            var typeOfSupport = frame.ClassId;

            await Task.WhenAll(frame.FadeTo(0.5, 75));
            await Task.WhenAll(frame.FadeTo(1, 75));
            await Navigation.PushAsync(new SupportEmailPage(typeOfSupport));
        }
    }
}