using Bitango_.Models;
using Bitango_.Views.GiftPageItems;
using Bitango_.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Bitango_.Views;

namespace Bitango_.ViewModel
{
    public class GiftViewModel : BaseViewModel
    {
        ObservableCollection<Category> menuItems;
        Category selectedMenuItem;
        INavigation navigation = Application.Current.MainPage.Navigation;
        public GiftViewModel()
        {
            LoadData();
        }
        public ObservableCollection<Category> MenuItems
        {
            get 
            { 
                return menuItems; 
            }
            set
            {
                menuItems = value;
                OnPropertyChanged();
            }
        }
        public Category SelectedMenuItem
        {
            get 
            { 
                return selectedMenuItem; 
            }
            set
            {
                selectedMenuItem = value;
                OnPropertyChanged();
            }
        }
        public ICommand SelectionCommand => new Command(DisplayPage);
        private void DisplayPage()
        {
            if (selectedMenuItem != null)
            {
                var detailsPage = new DishPage(selectedMenuItem);

                SelectedMenuItem = null;
                navigation.PushAsync(detailsPage, true);
            }            
        }
        private async void LoadData()
        {
            await navigation.PushPopupAsync(new LoaderPopup());
            await Task.Run(() => 
            {
                ApiAccess.GetMenu();
                Models.Menu menu = ApiAccess.BaseMenu;
                MenuItems = menu.Categories;
            });
            await navigation.PopPopupAsync();
        }
    }
}
