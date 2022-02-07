using Bitango_.API;
using Bitango_.Files.Resources;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bitango_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            Shell.SetNavBarHasShadow(this, false); // Delete shadow undel TitleView
            LoadInfo();
        }
        private async void Forgot_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ForgotPasswordPage());
        }
        private void LoadInfo()
        {
            datepicker.MinimumDate = DateTime.Now.AddMonths(-1000);
            datepicker.MaximumDate = DateTime.Now.AddMonths(-216);
            if (Application.Current.Properties.ContainsKey("Username") && Application.Current.Properties["Username"] != null)
            {
                EntryUserName.Text = Application.Current.Properties["Username"].ToString();
            }
            if (Application.Current.Properties.ContainsKey("Email") && Application.Current.Properties["Email"] != null)
            {
                EntryUserEmail.Text = Application.Current.Properties["Email"].ToString();
            }
            if (Application.Current.Properties.ContainsKey("Phone") && Application.Current.Properties["Phone"] != null)
            {
                EntryUserName.Text = Application.Current.Properties["Phone"].ToString();
            }
            if (Application.Current.Properties.ContainsKey("Birthday") && Application.Current.Properties["Birthday"] != null)
            {
                datepicker.Date = DateTime.Parse(Application.Current.Properties["Birthday"].ToString());
            }
            //if (Application.Current.Properties.ContainsKey("ID"))
            //{
            //    EntryUserPhoneNumber.Text = Application.Current.Properties["ID"].ToString();
            //}
        }
        /// <summary>
        /// Сохранение
        /// </summary>
        private async void Save_Clicked(object sender, EventArgs e)
        {
            string name = EntryUserName.Text;
            string phone = EntryUserPhoneNumber.Text;
            string datetime = datepicker.Date.ToString("yyyy-MM-dd");
            if (name == Application.Current.Properties["Username"].ToString() && phone == Application.Current.Properties["Phone"].ToString() && datetime == Application.Current.Properties["Birthday"].ToString())
            {
                await DisplayAlert(AppResources.Error_, AppResources.ChangePersonalData, "OK");
                return;
            }
            if (phone == null || phone == string.Empty)
            {
                await DisplayAlert(AppResources.Error_, AppResources.EnterYourPhoneNumber, "OK");
                return;
            }
            if (!IsPhoneNumber(phone))
            {
                await DisplayAlert(AppResources.Error_, AppResources.InvalidPhone, "OK");
                return;
            }

            await Navigation.PushModalAsync(new LoaderPopup());
            if ((phone.Equals(ApiAccess.current_user.Phone)) || !DBConnect.IsPhoneOnDB(phone))
            {
                DBConnect.Delete(ApiAccess.current_user);
                string id = ApiAccess.UpdateUser(name, phone, ApiAccess.current_user.Email, ApiAccess.current_user.ID, datetime).Replace("\"", "");
                DBConnect.Add(new Models.User(name, phone, id, ApiAccess.current_user.Email, "", datetime, ApiAccess.current_user.Role));
                ApiAccess.GetUserByID(id);

                Application.Current.Properties["Username"] = name;
                Application.Current.Properties["Phone"] = phone;
                Application.Current.Properties["ID"] = id;
                Application.Current.Properties["Birthday"] = datetime;
                await Navigation.PopModalAsync();
                await DisplayAlert(AppResources.Success, AppResources.DataSavedSuccessfully, "OK");
            }
            else
            {
                await Navigation.PopModalAsync();
                await DisplayAlert(AppResources.Error_, AppResources.ThisNumberBelongs, "OK");
            }
        }
        /// <summary>
        /// Проверка на валидность мобильного телефона
        /// </summary>
        /// <param name="number">Мобильный телефон</param>
        public static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^(\+[0-9]{11})$").Success;
        }
    }
}