using Bitango_.Models;
using Newtonsoft.Json;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Bitango_.API;
using Rg.Plugins.Popup.Extensions;
using Bitango_.ViewModel;
using Plugin.Connectivity;
using Bitango_.Files.Resources;
using System.Diagnostics;
using Firebase.Auth;
using Xamarin.Essentials;
using Xamarin.Auth;
using Bitango_.Files;
using System.Linq;
using static Bitango_.Models.FacebookUser;
using System.Net.Http;
using Plugin.GoogleClient;
using Plugin.GoogleClient.Shared;
using Plugin.FacebookClient;
using System.Threading.Tasks;

namespace Bitango_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        IGoogleClientManager _googleService; //Google services for Android
        IFacebookClient _facebookService; //Facebook services for Android
        public string WebAPIkey = "AIzaSyAK3-isHMBe_uKFCH92W3I5fj67Dc-Zchc";
        Account account;
        [Obsolete]
        AccountStore store;
        static Random rnd = new Random();

        [Obsolete]
        public LoginPage()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            InitializeComponent();
            store = AccountStore.Create();
            BindingContext = new LoginViewModel();
            Application.Current.Properties["RegisteredNow"] = "false";

            if (!CrossConnectivity.Current.IsConnected)
            {
                Navigation.PushModalAsync(new NoInternetLoginPopup());
            }
            else
            {

                DBConnect.GetPromotionList(); //Get actual data of promotions
                ApiAccess.aboutContainer = DBConnect.GetAboutPageData(); //Get actual data of about page
            }

            stopwatch.Stop();
            DisplayAlert("Error", "TIME OF WORKING LOGIN: " + stopwatch.ElapsedMilliseconds, "OK");
        }
        /// <summary>
        /// Открытие страницы для регистрации
        /// </summary>
        private async void SignUp_Clicked(object sender, EventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            await Navigation.PushAsync(new RegistrationPage());
            ((Button)sender).IsEnabled = true; ;
        }
        /// <summary>
        /// Нажатие на кнопку авторизации
        /// </summary>
        private async void Login_Clicked(object sender, EventArgs e)
        {
            ((Button)sender).IsEnabled = false;

            string email = EntryUserEmail.Text;
            string password = EntryPassword.Text;

            if (email == string.Empty || email == null ||
                password == string.Empty || password == null)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert(AppResources.Error_, AppResources.PleaseFillInAllRequiredFields, "OK");
                });
                ((Button)sender).IsEnabled = true;
                return;
            }
            await Navigation.PushPopupAsync(new LoaderPopup(), true);

            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIkey));
            try
            {
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(email, password);
                var content = await auth.GetFreshAuthAsync();
                var serializedcontnet = JsonConvert.SerializeObject(content);
                Preferences.Set("MyFirebaseRefreshToken", serializedcontnet);

                string id = DBConnect.GetUserID(email);
                Application.Current.Properties["IsLoggedIn"] = Boolean.TrueString;
                Application.Current.Properties["Image"] = "NONE";
                Application.Current.Properties["Email"] = email;
                Application.Current.Properties["ID"] = id;

                if (Application.Current.Properties["RegisteredNow"].ToString() == "false")
                {
                    App.Current.MainPage = new MainPage();
                }
                else
                {
                    App.Current.MainPage = new MainPage();
                    await Navigation.PushPopupAsync(new StartHelpPopup(), true);
                }

            }
            catch (Exception)
            {
                await this.DisplayAlert(AppResources.Error_, AppResources.AuthorizationWasUnsuccessful, "OK");
            }

            await Navigation.PopPopupAsync();
            ((Button)sender).IsEnabled = true;
        }
        /// <summary>
        /// Забыли пароль
        /// </summary>
        private async void Forgot_Clicked(object sender, EventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            await Navigation.PushAsync(new ForgotPasswordPage());
            ((Button)sender).IsEnabled = true;
        }
        /// <summary>
        /// Google аутентификация
        /// </summary>
        private void OnGoogleLoginCommand(object btn, EventArgs e)
        {
            ((Button)btn).IsEnabled = false;
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    OnGoogleIOSLoginCommand(btn, e);
                    break;

                case Device.Android:
                    OnGoogleAndroidLoginCommand(btn, e);
                    break;
            }
            ((Button)btn).IsEnabled = true;
        }
        /// <summary>
        /// Google аутентификация на Android
        /// </summary>
        private async void OnGoogleAndroidLoginCommand(object btn, EventArgs e1)
        {
            await Navigation.PushPopupAsync(new LoaderPopup(), true);
            try
            {
                _googleService = CrossGoogleClient.Current;

                if (!string.IsNullOrEmpty(_googleService.IdToken))
                {
                    _googleService.Logout();
                }

                EventHandler<GoogleClientResultEventArgs<Plugin.GoogleClient.Shared.GoogleUser>> userLoginDelegate = null;
                userLoginDelegate = async (object sender, GoogleClientResultEventArgs<Plugin.GoogleClient.Shared.GoogleUser> e) =>
                {
                    switch (e.Status)
                    {
                        case GoogleActionStatus.Completed:
                            string name = e.Data.Name;
                            string phone = "8888" + rnd.Next(1000000, 10000000);
                            string email = e.Data.Email.Trim();
                            string social = "Google";
                            bool isOnDB = DBConnect.IsEmailOnDB(email);
                            string birthday = DateTime.Now.ToString("yyyy-MM-dd");
                            string ID;
                            if (!isOnDB)
                            {
                                ID = ApiAccess.CreateUser(name, phone, email, birthday).Replace("\"", "");
                                DBConnect.Add(new Models.User(name, phone, ID, email, social, birthday, "client"));
                            }
                            else
                            {
                                ID = DBConnect.GetUserID(email);
                            }

                            Application.Current.Properties["RegisteredNow"] = "true";
                            Application.Current.Properties["IsLoggedIn"] = Boolean.TrueString;
                            Application.Current.Properties["Image"] = "NONE";
                            Application.Current.Properties["Email"] = email;
                            Application.Current.Properties["ID"] = ID;
                            Application.Current.Properties["RegisteredNow"] = Boolean.TrueString;
                            await Navigation.PopPopupAsync();
                            App.Current.MainPage = new MainPage();
                            await Navigation.PushPopupAsync(new StartHelpPopup(), true);
                            break;
                        case GoogleActionStatus.Canceled:
                            await Navigation.PopPopupAsync();
                            await App.Current.MainPage.DisplayAlert("Google Auth", "Canceled", "Ok");
                            break;
                        case GoogleActionStatus.Error:
                            await Navigation.PopPopupAsync();
                            await App.Current.MainPage.DisplayAlert("Google Auth", "Error", "Ok");
                            break;
                        case GoogleActionStatus.Unauthorized:
                            await Navigation.PopPopupAsync();
                            await App.Current.MainPage.DisplayAlert("Google Auth", "Unauthorized", "Ok");
                            break;
                    }

                    _googleService.OnLogin -= userLoginDelegate;
                };

                _googleService.OnLogin += userLoginDelegate;

                await _googleService.LoginAsync();
            }
            catch (GoogleClientSignInNetworkErrorException)
            {
                await Navigation.PopPopupAsync();
            }
            catch (GoogleClientSignInCanceledErrorException)
            {
                await Navigation.PopPopupAsync();
            }
            catch (Exception ex)
            {
                await Navigation.PopPopupAsync();
                await DisplayAlert("EXCEPTION", ex.ToString(), "OK");
            }
        }
        /// <summary>
        /// Google аутентификация IOS
        /// </summary>
        [Obsolete]
        private void OnGoogleIOSLoginCommand(object btn, EventArgs e1)
        {
            string clientId = null;
            string redirectUri = null;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    clientId = Constants.iOSClientId;
                    redirectUri = Constants.iOSRedirectUrl;
                    break;

                case Device.Android:
                    clientId = Constants.AndroidClientId;
                    redirectUri = Constants.AndroidRedirectUrl;
                    break;
            }

            account = store.FindAccountsForService(Constants.AppName).FirstOrDefault();

            var authenticator = new OAuth2Authenticator(
                clientId,
                null,
                Constants.Scope,
                new Uri(Constants.AuthorizeUrl),
                new Uri(redirectUri),
                new Uri(Constants.AccessTokenUrl),
                null,
                true);

            authenticator.Completed += OnAuthCompleted;
            authenticator.Error += OnAuthError;

            AuthenticationState.Authenticator = authenticator;

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);
        }
        /// <summary>
        /// Facebook аутентификация
        /// </summary>
        private void OnFacebookLoginCommand(object btn, EventArgs e1)
        {
            ((Button)btn).IsEnabled = false;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    OnFacebookIOSLoginCommand(btn, e1);
                    break;

                case Device.Android:
                    OnFacebookAndroidLoginCommand(btn, e1);
                    break;
            }

            ((Button)btn).IsEnabled = true;
        }
        /// <summary>
        /// Facebook аутентификация на Android
        /// </summary>
        private async void OnFacebookAndroidLoginCommand(object btn, EventArgs e1)
        {
            await Navigation.PushPopupAsync(new LoaderPopup(), true);
            try
            {
                _facebookService = CrossFacebookClient.Current;

                if (_facebookService.IsLoggedIn)
                {
                    _facebookService.Logout();
                }

                EventHandler<FBEventArgs<string>> userDataDelegate = null;
                userDataDelegate = FacebookStatus;

                _facebookService.OnUserData += userDataDelegate;

                string[] fbRequestFields = { "email", "first_name", "gender", "last_name" };
                string[] fbPermisions = { "email" };

                await _facebookService.RequestUserDataAsync(fbRequestFields, fbPermisions);
            }
            catch (Exception ex)
            {
                await DisplayAlert("EXCEPTION", ex.ToString(), "OK");
            }
        }
        /// <summary>
        /// Дальнейшая работа Facebook аутентификации в зависимости от статуса на Android
        /// </summary>
        private async void FacebookStatus(object sender, FBEventArgs<string> e)
        {
            switch (e.Status)
            {
                case FacebookActionStatus.Completed:
                    var facebookProfile = await Task.Run(() => JsonConvert.DeserializeObject<FacebookProfile>(e.Data));

                    string name = $"{facebookProfile.FirstName} {facebookProfile.LastName}";
                    string phone = "8888" + rnd.Next(1000000, 10000000);
                    string email = facebookProfile.Email;
                    string social = "Facebook";
                    bool isOnDB = DBConnect.IsEmailOnDB(email);
                    string ID;
                    string birthday = DateTime.Now.ToString("yyyy-MM-dd");

                    if (!isOnDB)
                    {
                        ID = ApiAccess.CreateUser(name, phone, email, birthday).Replace("\"", "");
                        DBConnect.Add(new Models.User(name, phone, ID, email, social, birthday, "client"));
                    }
                    else
                    {
                        ID = DBConnect.GetUserID(email);
                    }

                    Application.Current.Properties["RegisteredNow"] = "true";
                    Application.Current.Properties["IsLoggedIn"] = Boolean.TrueString;
                    Application.Current.Properties["Image"] = "NONE";
                    Application.Current.Properties["Email"] = email;
                    Application.Current.Properties["ID"] = ID;
                    Application.Current.Properties["RegisteredNow"] = Boolean.TrueString;

                    await Navigation.PopPopupAsync();
                    App.Current.MainPage = new MainPage();
                    await Navigation.PushPopupAsync(new StartHelpPopup(), true);
                    await DisplayAlert("Test", facebookProfile.ToString(), "OK");
                    break;
                case FacebookActionStatus.Canceled:
                    await Navigation.PopPopupAsync();
                    await App.Current.MainPage.DisplayAlert("Facebook Auth", "Canceled", "Ok");
                    break;
                case FacebookActionStatus.Error:
                    await Navigation.PopPopupAsync();
                    await App.Current.MainPage.DisplayAlert("Facebook Auth", "Error", "Ok");
                    break;
                case FacebookActionStatus.Unauthorized:
                    await Navigation.PopPopupAsync();
                    await App.Current.MainPage.DisplayAlert("Facebook Auth", "Unauthorized", "Ok");
                    break;
            }
            _facebookService.OnUserData -= FacebookStatus;
        }
        /// <summary>
        /// Facebook аутентификация на IOS
        /// </summary>
        [Obsolete]
        private void OnFacebookIOSLoginCommand(object btn, EventArgs e1)
        {
            string clientId = null;
            string redirectUri = null;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    clientId = Constants.FacebookiOSClientId;
                    redirectUri = Constants.FacebookiOSRedirectUrl;
                    break;

                case Device.Android:
                    clientId = Constants.FacebookAndroidClientId;
                    redirectUri = Constants.FacebookAndroidRedirectUrl;
                    break;
            }

            account = store.FindAccountsForService(Constants.AppName).FirstOrDefault();

            var authenticator = new OAuth2Authenticator(
                clientId,
                Constants.FacebookScope,
                new Uri(Constants.FacebookAuthorizeUrl),
                new Uri(Constants.FacebookAccessTokenUrl),
                null);

            authenticator.Completed += OnAuthCompleted;
            authenticator.Error += OnAuthError;

            AuthenticationState.Authenticator = authenticator;

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);
        }
        /// <summary>
        /// Дальнейшая работа Facebook аутентификации в случае успешного статуса на IOS
        /// </summary>
        [Obsolete]
        async void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {

            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

            if (authenticator.AuthorizeUrl.Host == "www.facebook.com")
            {
                FacebookEmail facebookEmail = null;

                var httpClient = new HttpClient();
                string json;
                try
                {
                    json = await httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=id,name,first_name,last_name,email,picture.type(large)&access_token=" + e.Account.Properties["access_token"]);
                }
                catch (NullReferenceException)
                {
                    return;
                }

                if (json != null)
                {
                    facebookEmail = JsonConvert.DeserializeObject<FacebookEmail>(json);
                    await store.SaveAsync(account = e.Account, Constants.AppName);
                }

                if (facebookEmail != null)
                {
                    await Navigation.PushPopupAsync(new LoaderPopup(), true);

                    string name = $"{facebookEmail.First_Name} {facebookEmail.Last_Name}";
                    string phone = "8888" + rnd.Next(1000000, 10000000);
                    string email = facebookEmail.Email;
                    string social = "Facebook";
                    bool isOnDB = DBConnect.IsEmailOnDB(email);
                    string ID;
                    string birthday = DateTime.Now.ToString("yyyy-MM-dd");

                    if (!isOnDB)
                    {
                        ID = ApiAccess.CreateUser(name, phone, email, birthday).Replace("\"", "");
                        DBConnect.Add(new Models.User(name, phone, ID, email, social, birthday, "client"));
                    }
                    else
                    {
                        ID = DBConnect.GetUserID(email);
                    }

                    Application.Current.Properties["RegisteredNow"] = "true";
                    Application.Current.Properties["IsLoggedIn"] = Boolean.TrueString;
                    Application.Current.Properties["Username"] = name;
                    Application.Current.Properties["Image"] = "NONE";
                    Application.Current.Properties["Email"] = email;
                    Application.Current.Properties["ID"] = ID;
                    Application.Current.Properties["RegisteredNow"] = Boolean.TrueString;

                    await Navigation.PopPopupAsync();
                    App.Current.MainPage = new MainPage();
                    await Navigation.PushPopupAsync(new StartHelpPopup(), true);
                    await DisplayAlert("Test", facebookEmail.ToString(), "OK");
                }
            }
            else
            {
                Models.GoogleUser user = null;
                if (e.IsAuthenticated)
                {
                    await Navigation.PushPopupAsync(new LoaderPopup(), true);
                    var request = new OAuth2Request("GET", new Uri(Constants.UserInfoUrl), null, e.Account);
                    var response = await request.GetResponseAsync();
                    if (response != null)
                    {
                        // Deserialize the data and store it in the account store
                        // The users email address will be used to identify data in SimpleDB
                        string userJson = await response.GetResponseTextAsync();
                        user = JsonConvert.DeserializeObject<Models.GoogleUser>(userJson);
                    }

                    if (user != null)
                    {
                        string name = user.Name;
                        string phone = "8888" + rnd.Next(1000000, 10000000);
                        string email = user.Email.Trim();
                        string social = "Google";
                        bool isOnDB = DBConnect.IsEmailOnDB(email);
                        string birthday = DateTime.Now.ToString("yyyy-MM-dd");
                        string ID;
                        if (!isOnDB)
                        {
                            ID = ApiAccess.CreateUser(name, phone, email, birthday).Replace("\"", "");
                            DBConnect.Add(new Models.User(name, phone, ID, email, social, birthday, "client"));
                        }
                        else
                        {
                            ID = DBConnect.GetUserID(email);
                        }

                        Application.Current.Properties["RegisteredNow"] = "true";
                        Application.Current.Properties["IsLoggedIn"] = Boolean.TrueString;
                        Application.Current.Properties["Image"] = "NONE";
                        Application.Current.Properties["Username"] = name;
                        Application.Current.Properties["Email"] = email;
                        Application.Current.Properties["ID"] = ID;
                        Application.Current.Properties["RegisteredNow"] = Boolean.TrueString;
                        await Navigation.PopPopupAsync();
                        App.Current.MainPage = new MainPage();
                        await Navigation.PushPopupAsync(new StartHelpPopup(), true);
                    }

                    //await store.SaveAsync(account = e.Account, AppConstant.Constants.AppName);
                    //await DisplayAlert("Email address", user.Email, "OK");
                }
            }
        }
        /// <summary>
        /// Дальнейшая работа Facebook аутентификации в случае ошибочного статуса на IOS
        /// </summary>
        [Obsolete]
        async void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            await Navigation.PushPopupAsync(new LoaderPopup(), true);
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }
            await Navigation.PopPopupAsync();
            Debug.WriteLine("Authentication error: " + e.Message);
        }
    }
}