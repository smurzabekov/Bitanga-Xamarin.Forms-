using Bitango_.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Plugin.Connectivity;
using Xamarin.Forms;
using Bitango_.Views;
using System.Collections.Generic;

namespace Bitango_.ViewModel
{
    internal class LoginViewModel : INotifyPropertyChanged
    {
        private string _conn;
        public string Conn {
            get => _conn;
            set {
                _conn = value;
                OnPropertyChanged();
            }
        }
        public LoginViewModel()
        {
            CheckWifiContinuously();
        }
        public void CheckWifiContinuously()
        {
            CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
            {
                var navigation = Application.Current.MainPage.Navigation;
                
                if (!args.IsConnected)
                {
                    if (navigation.ModalStack.Count == 0)
                    {
                        navigation.PushModalAsync(new NoInternetLoginPopup());
                    }
                }
            };
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
