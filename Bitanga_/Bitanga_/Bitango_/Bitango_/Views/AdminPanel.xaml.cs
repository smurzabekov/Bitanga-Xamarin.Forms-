using Bitango_.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bitango_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminPanel : ContentPage
    {
        public AdminPanel()
        {
            InitializeComponent();
            Shell.SetNavBarHasShadow(this, false); // Delete shadow undel TitleView
            ErrorLabel.IsVisible = false;
        }
        private async void GoBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
        private bool IsEmail(string email)
        {
            if (email.Contains('@') && email.Contains('.'))
            {
                return true;
            }
            return false;
        }
        private async void AddEmploy_Clicked(object sender, EventArgs e)
        {
            try
            {
                ErrorLabel.IsVisible = false;
                if (IsEmail(EntryEmail.Text) && DBConnect.IsEmailOnDB(EntryEmail.Text))
                {
                    DBConnect.AddRole(EntryEmail.Text, "employ");
                    await DisplayAlert("Добавление сотрудника", "Сотрудник успешно добавлен", "ОК");
                    await Navigation.PopAsync();
                }
                else
                {
                    ErrorLabel.Text = "Указан неправильный адрес электронной почты";
                    ErrorLabel.IsVisible = true;
                }
            }
            catch (Exception)
            {
                ErrorLabel.Text = "Данная электронная почта не зарегистрирована в приложении Bitanga";
                ErrorLabel.IsVisible = true;
            }
        }

        private async void AddAdmin_Clicked(object sender, EventArgs e)
        {
            try
            {
                ErrorLabel.IsVisible = false;
                if (IsEmail(EntryEmail.Text) && DBConnect.IsEmailOnDB(EntryEmail.Text))
                {
                    DBConnect.AddRole(EntryEmail.Text, "admin");
                    await DisplayAlert("Добавление админа", "Админ успешно добавлен", "ОК");
                    await Navigation.PopAsync();
                }
                else
                {
                    ErrorLabel.Text = "Указан неправильный адрес электронной почты";
                    ErrorLabel.IsVisible = true;
                }
            }
            catch (Exception)
            {
                ErrorLabel.Text = "Данная электронная почта не зарегистрирована в приложении Bitanga";
                ErrorLabel.IsVisible = true;
            }
        }

        private async void DeleteEmploy_Clicked(object sender, EventArgs e)
        {
            try
            {
                ErrorLabel.IsVisible = false;
                if (IsEmail(EntryEmail.Text) && DBConnect.IsEmailOnDB(EntryEmail.Text))
                {
                    DBConnect.AddRole(EntryEmail.Text, "client");
                    await DisplayAlert("Удаление сотрудника", "Сотрудник успешно удален", "ОК");
                    await Navigation.PopAsync();
                }
                else
                {
                    ErrorLabel.Text = "Указан неправильный адрес электронной почты";
                    ErrorLabel.IsVisible = true;
                }
            }
            catch (Exception)
            {
                ErrorLabel.Text = "Данная электронная почта не зарегистрирована в приложении Bitanga";
                ErrorLabel.IsVisible = true;
            }
        }

        private async void DeleteAdmin_Clicked(object sender, EventArgs e)
        {
            try
            {
                ErrorLabel.IsVisible = false;
                if (IsEmail(EntryEmail.Text) && DBConnect.IsEmailOnDB(EntryEmail.Text))
                {
                    DBConnect.AddRole(EntryEmail.Text, "client");
                    await DisplayAlert("Удаление админа", "Админ успешно удален", "ОК");
                    await Navigation.PopAsync();
                }
                else
                {
                    ErrorLabel.Text = "Указан неправильный адрес электронной почты";
                    ErrorLabel.IsVisible = true;
                }
            }
            catch (Exception)
            {
                ErrorLabel.Text = "Данная электронная почта не зарегистрирована в приложении Bitanga";
                ErrorLabel.IsVisible = true;
            }
        }
    }
}