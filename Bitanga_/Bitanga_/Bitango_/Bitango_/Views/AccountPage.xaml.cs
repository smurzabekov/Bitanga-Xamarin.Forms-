using Bitango_.API;
using Bitango_.Files.Resources;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bitango_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountPage : ContentPage
    {
        public AccountPage()
        {
            try
            {
                InitializeComponent();

                Shell.SetNavBarHasShadow(this, false); // Delete shadow undel TitleView

                if (ApiAccess.current_user.Role == "admin")
                {
                    Row.Height = 100;
                    Manage.IsVisible = true;
                    Manage.IsEnabled = true;
                }
                if (Application.Current.Properties.ContainsKey("Username"))
                {
                    if (Application.Current.Properties["Username"] != null)
                    {
                        UserName.Text = Application.Current.Properties["Username"].ToString();
                    }
                    else
                    {
                        UserName.Text = Application.Current.Properties["Email"].ToString().Split('@')[0];
                    }

                }
                if (Application.Current.Properties.ContainsKey("Email"))
                {
                    UserEmail.Text = Application.Current.Properties["Email"].ToString();
                }


                Balance.Text = $"{Application.Current.Properties["Balance"]} @ ";

                InitProfilePhoto();

            }
            catch (Exception e)
            {
                DisplayAlert("EXCEPTION", e.ToString(), "OK");
            }

        }
        /// <summary>
        /// Инициализация фото пользователя 
        /// </summary>
        private void InitProfilePhoto()
        {
            bool hasProfileImage = Application.Current.Properties.ContainsKey("Image");
            if (hasProfileImage)
            {
                string image = Application.Current.Properties["Image"].ToString();

                if (image == "NONE")
                {
                    Profile_Image.Source = "profile_default.png";
                    return;
                }
                byte[] imageArray = System.Convert.FromBase64String(image);
                Profile_Image.Source = ImageSource.FromStream(() => new MemoryStream(imageArray));
            }
            else
            {
                Profile_Image.Source = "profile_default.png";
            }
        }
        /// <summary>
        /// Выход из аккаунта
        /// </summary>
        private async void LogOut_Clicked(object sender, EventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            bool isWantedToExit = await DisplayAlert(AppResources.Warning, AppResources.AreYouSureYouWant, AppResources.YES, AppResources.CANCEL);

            if (isWantedToExit)
            {
                Application.Current.Properties["IsLoggedIn"] = Boolean.FalseString;
                Application.Current.Properties["Image"] = "NONE";

                App.Current.MainPage = new NavigationPage(new LoginPage());
            }
            ((Button)sender).IsEnabled = true;
        }
        /// <summary>
        /// Загрузка фото из галереи
        /// </summary>
        private async void ProfileImage_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("Error!", "Picking a photo is not supported", "OK");
                    return;
                }

                var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,
                    CompressionQuality = 40
                });

                if (file == null)
                    return;

                byte[] imageArray = File.ReadAllBytes(file.Path);
                Application.Current.Properties["Image"] = System.Convert.ToBase64String(imageArray);
                Profile_Image.Source = ImageSource.FromStream(() => new MemoryStream(imageArray));
            }
            catch (Plugin.Media.Abstractions.MediaPermissionException)
            {
                return;
            }
        }

        //private async void Orders_Tapped(object sender, EventArgs e)
        //{
        //    await Task.WhenAll(Order_frame.FadeTo(0.5, 75));
        //    await Task.WhenAll(Order_frame.FadeTo(1, 75));
        //    await Navigation.PushAsync(new OrdersPage());
        //}
        /// <summary>
        /// Открытие страницы службы поддержки
        /// </summary>
        private async void Support_Tapped(object sender, EventArgs e)
        {
            await Task.WhenAll(Support_frame.FadeTo(0.5, 75));
            await Task.WhenAll(Support_frame.FadeTo(1, 75));
            await Navigation.PushAsync(new SupportPage());
        }
        /// <summary>
        /// Открытие настроек
        /// </summary>
        private async void Settings_Tapped(object sender, EventArgs e)
        {
            await Task.WhenAll(Settings_frame.FadeTo(0.5, 75));
            await Task.WhenAll(Settings_frame.FadeTo(1, 75));
            await Navigation.PushAsync(new SettingsPage());
        }
        private async void Manage_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AdminPanel());
        }
    }
}