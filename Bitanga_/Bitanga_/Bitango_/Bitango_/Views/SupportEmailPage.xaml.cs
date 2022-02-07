using Bitango_.Files.Resources;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Net;
using System.Net.Mail;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bitango_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SupportEmailPage : ContentPage
    {
        string typeOfSupport;
        public SupportEmailPage()
        {
            InitializeComponent();
        }
        public SupportEmailPage(string typeOfSupport)
        {
            this.typeOfSupport = typeOfSupport;
            InitializeComponent();
            Shell.SetNavBarHasShadow(this, false); // Delete shadow undel TitleView
        }
        /// <summary>
        /// Отправка письма службе поддержки через SMTP
        /// </summary>
        private async void Send_Clicked(object sender, EventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            string customer_email = EntryUserEmail.Text;
            string body = editor_problem.Text;

            if (customer_email == string.Empty || customer_email == null ||
                body == string.Empty || body == null)
            {
                await DisplayAlert(AppResources.Error_, AppResources.PleaseFillInAllRequiredFields, "OK");
                ((Button)sender).IsEnabled = true;
                return;
            }

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
                Subject = $"Bitanga Issue: {typeOfSupport}",
                Body = $"Почта клиента: {customer_email} {Environment.NewLine}{Environment.NewLine}{body}"
            };
            message.To.Add(toEmail);

            try
            {
                await Navigation.PushPopupAsync(new LoaderPopup(), true);
                client.Send(message);
                await Navigation.PopPopupAsync();
                await DisplayAlert(AppResources.TheEmailWasSentSuccessfully, AppResources.AsSoonAsPossibleTheDevelopers, "OK");
                await Navigation.PopToRootAsync();
            }
            catch (Exception exc)
            {
                await DisplayAlert(AppResources.AnUnexpectedError, $"{exc.Message}", "OK");
            }
            ((Button)sender).IsEnabled = true;
        }
    }
}