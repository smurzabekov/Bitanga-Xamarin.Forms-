using Bitango_.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Plugin.Connectivity;
using Xamarin.Forms;
using Bitango_.Views;
using System.Collections.Generic;
using Bitango_.Models;
using System;
using System.Linq;
using System.Windows.Input;
using Bitango_.API;
using Bitango_.Files.Resources;
using System.Globalization;

namespace Bitango_.ViewModel
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        INavigation navigation = Application.Current.MainPage.Navigation;
        private string _conn;
        public string Conn {
            get => _conn;
            set {
                _conn = value;
                OnPropertyChanged();
            }
        }
        public MainViewModel()
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
                        navigation.PushModalAsync(new NoInternetMainPopup());
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

        public List<Promotion> CollectionsList { get => GetCollections(); }
        private List<Promotion> GetCollections()
        {
            //var trendList = new List<Promotion>();
            //trendList.Add(new Promotion { ImageUrl = "Mainpage_borscht.png", Name = "Борщ с мясом медведя борщ с мясом медведя", Price = "1850 @ ", Description = "Удивительный вкус из Сибири" });
            //trendList.Add(new Promotion { ImageUrl = "Mainpage_salo.png", Name = "Сало с майдана", Price = "1000 @ ", Description = "Борщ с мясом медведя борщ с мясом медведя Борщ с мясом медведя борщ с мясом медведя" });
            //trendList.Add(new Promotion { ImageUrl = "leatherBag.png", Name = "Суп повара", Price = "1300 @ ", Description = "Суп хорош" });
          
                if (AppResources.Culture == new CultureInfo("ru"))
                {
                    for (int i = 0; i < ApiAccess.promotions.Count; i++)
                    {
                        ApiAccess.promotions[i].Title = ApiAccess.promotions[i].TitleRU;
                        ApiAccess.promotions[i].Subtitle = ApiAccess.promotions[i].SubtitleRU;
                        ApiAccess.promotions[i].Description = ApiAccess.promotions[i].DescriptionRU;
                    }
                }
                else
                {
                    for (int i = 0; i < ApiAccess.promotions.Count; i++)
                    {
                        ApiAccess.promotions[i].Title = ApiAccess.promotions[i].TitleEN;
                        ApiAccess.promotions[i].Subtitle = ApiAccess.promotions[i].SubtitleEN;
                        ApiAccess.promotions[i].Description = ApiAccess.promotions[i].DescriptionEN;
                    }
                }
                
 
            return ApiAccess.promotions;
        }
        Promotion selectedMenuItem;
        public Promotion SelectedMenuItem
        {
            get
            {
                return selectedMenuItem;
            }
            set
            {
                selectedMenuItem = value;
                OnPropertyChanged("SelectedMenuItem");
            }
        }
        public ICommand SelectionCommand => new Command(DisplayPage);
        private void DisplayPage()
        {
            if (selectedMenuItem != null)
            {
                var viewModel = new PromotionViewModel { SelectedPromotion = selectedMenuItem };
                var detailsPage = new PromotionDetailsPage(selectedMenuItem) { BindingContext = viewModel};

                SelectedMenuItem = null;
                navigation.PushAsync(detailsPage, true);
            }
        }
    }
}
