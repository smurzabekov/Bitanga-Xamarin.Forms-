using Bitango_.Files.Resources;
using Firebase.Auth;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bitango_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPasswordPage : ContentPage
    {
        public string WebAPIkey = "AIzaSyAK3-isHMBe_uKFCH92W3I5fj67Dc-Zchc";
        public ForgotPasswordPage()
        {
            InitializeComponent();
        }
        private async void ForgotPassword_Clicked(object sender, EventArgs e)
        {
            try
            {
                ErrorLabel.IsVisible = false;

                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIkey));
                await authProvider.SendPasswordResetEmailAsync(EntryEmail.Text);

                await DisplayAlert(AppResources.PasswordReset, AppResources.ALinkToResetYourPassword, "ОК");
                await Navigation.PopAsync();
            }
            catch (Exception)
            {
                ErrorLabel.Text = AppResources.ThisEmailIsNotRegistered;
                ErrorLabel.IsVisible = true;
            }
        }
        private async void GoBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}