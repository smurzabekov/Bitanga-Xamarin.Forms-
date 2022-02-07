using Rg.Plugins.Popup.Extensions;
using System;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using Bitango_.Files.Resources;
using Telegram.Bot;

namespace Bitango_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookPage : ContentPage
    {
        string newline = Environment.NewLine;
        public BookPage()
        {
            InitializeComponent();

            Shell.SetNavBarHasShadow(this, false); // Delete shadow undel TitleView

            datepicker.MinimumDate = DateTime.Now;
            datepicker.MaximumDate = (DateTime.Now).AddDays(20);

            if (Application.Current.Properties.ContainsKey("Username") && Application.Current.Properties["Username"] != null)
            {
                EntryUserName.Text = Application.Current.Properties["Username"].ToString();
            }
            if (Application.Current.Properties.ContainsKey("Phone") && Application.Current.Properties["Phone"] != null)
            {
                EntryUserPhoneNumber.Text = Application.Current.Properties["Phone"].ToString();
            }
        }
        /// <summary>
        /// Бронирование, реализованное через SMTP и Telegram Bot
        /// </summary>
        private async void Book_Clicked(object sender, EventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            string customer_name = EntryUserName.Text;
            string customer_phone = EntryUserPhoneNumber.Text;
            string numOfpersons = EntryNumberOfPersons.Text;
            string body = editor.Text;

            if (customer_phone == string.Empty || customer_phone == null ||
                body == string.Empty || body == null ||
                customer_name == string.Empty || customer_name == null ||
                numOfpersons == string.Empty || numOfpersons == null)
            {
                await DisplayAlert(AppResources.Error_, AppResources.PleaseFillInAllRequiredFields, "OK");
                ((Button)sender).IsEnabled = true;
                return;
            }

            string messageBody = $"Имя: {customer_name}{newline}Телефон клиента: {customer_phone}{newline}" +
                $"Количество персон: {numOfpersons}{newline}Дата: {datepicker.Date.ToString("dd/MM/yyyy")}{newline}" +
                $"Время брони: {timepicker.Time.ToString(@"hh\:mm")}{newline}{newline}{body}";

            SmtpClient client = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential()
                {
                    UserName = "bloodhounds.studio@gmail.com",
                    Password = "SEVAdanyasularadik2019"
                }
            };
            MailAddress fromEmail = new MailAddress("bloodhounds.studio@gmail.com", "Bitanga customer");
            MailAddress toEmail = new MailAddress("bloodhounds.studio@gmail.com", "Bloodhounds Studio");
            MailMessage message = new MailMessage()
            {
                From = fromEmail,
                Subject = $"Bitanga BOOK",
                Body = messageBody
            };
            message.To.Add(toEmail);

            try
            {
                await Navigation.PushPopupAsync(new LoaderPopup(), true);
                client.Send(message);

                TelegramBotClient botClient = new TelegramBotClient("1746973755:AAE26bKYrNE1vsHgP6TaLa997MDGXzoEoY8");
                await botClient.SendTextMessageAsync("-520190371", messageBody);

            }
            catch (Exception exc)
            {
                await DisplayAlert(AppResources.AnUnexpectedError, $"{exc.Message}", "OK");
            }
            await Navigation.PopPopupAsync();
            await Navigation.PushModalAsync(new BookingSuccessPopup());
            ((Button)sender).IsEnabled = true;
        }
        private void Time_Changed(object sender, PropertyChangedEventArgs args)
        {
            
        }
    }
}