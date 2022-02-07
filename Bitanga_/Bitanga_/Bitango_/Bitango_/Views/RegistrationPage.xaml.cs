using Bitango_.Tables;
using SQLite;
using System;
using Bitango_.API;
using Bitango_.Models;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text.RegularExpressions;
using Bitango_.Files.Resources;
using Firebase.Auth;

namespace Bitango_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        public string WebAPIkey = "AIzaSyAK3-isHMBe_uKFCH92W3I5fj67Dc-Zchc";
        public RegistrationPage()
        {
            InitializeComponent();
            datepicker.MinimumDate = DateTime.Now.AddMonths(-1000);
            datepicker.MaximumDate = DateTime.Now.AddMonths(-216);
        }
        private async void Register_Clicked(object sender, EventArgs e)
        {
            ((Button)sender).IsEnabled = false;

            string username = EntryUserName.Text;
            string password = EntryUserPassword.Text;
            string email = EntryUserEmail.Text;
            string phone = EntryUserPhoneNumber.Text;
            string repeat_password = EntryRepeatUserPassword.Text;

            if (username == string.Empty || username == null ||
                password == string.Empty || password == null ||
                email == string.Empty || email == null ||
                phone == string.Empty || phone == null ||
                repeat_password == string.Empty || repeat_password == null)
            {
                await DisplayAlert(AppResources.UnsuccessfulRegistration, AppResources.PleaseFillInAllRequiredFields, "OK");
                ((Button)sender).IsEnabled = true;
                return;
            }
            string phoneEntry = ReplaceFirstNumber(phone);
            if (!IsPhoneNumber(phoneEntry))
            {
                await DisplayAlert(AppResources.Error_, AppResources.InvalidPhone, "OK");
                ((Button)sender).IsEnabled = true;
                return;
            }
            if (password.Trim().Length < 6)
            {
                await DisplayAlert(AppResources.Error_, "ХУЙ", "OK");
                ((Button)sender).IsEnabled = true;
                return;
            }
            if (password.Trim() != repeat_password.Trim())
            {
                await DisplayAlert(AppResources.Error_, AppResources.PasswordsDoNotMatch, "OK");
                ((Button)sender).IsEnabled = true;
                return;
            }

            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIkey));
                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password);
                string gettoken = auth.FirebaseToken;

                string social = "NONE";
                string datetime = datepicker.Date.ToString("yyyy-MM-dd");
                bool isOnDB = DBConnect.IsEmailOnDB(email);

                if (!isOnDB)
                {
                    string ID = ApiAccess.CreateUser(username, phoneEntry, email, datetime).Replace("\"", "");
                    DBConnect.Add(new Models.User(username, phoneEntry, ID, email, social, datetime, "client"));
                }

                Application.Current.Properties["RegisteredNow"] = "true";
                await DisplayAlert(AppResources.Congratulations, AppResources.UserRegistrationWasSuccessful, "OK");
                await Navigation.PopAsync();
            }
            catch (Exception)
            {
                await DisplayAlert(AppResources.UnsuccessfulRegistration, AppResources.PleaseFillInAllRequiredFields, "OK");
            }

            ((Button)sender).IsEnabled = true;
        }
        private async void GoBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
        private string ReplaceFirstNumber(string number)
        {
            char[] ch = number.ToCharArray();
            if (ch[0] == '+')
            {
                return number;
            }
            char[] newCh = new char[ch.Length + 1];
            newCh[0] = '+';
            newCh[1] = '7';
            for (int i = 2; i < newCh.Length; i++)
            {
                newCh[i] = ch[i - 1];
            }
            return new string(newCh);
        }
        private static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^(\+[0-9]{11})$").Success;
        }
    }
}