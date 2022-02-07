using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Bitango_.Models;
using Xamarin.Forms.Xaml;
using Bitango_.Files.Resources;
using System.Globalization;

namespace Bitango_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        public MapPage()
        {
            InitializeComponent();
            Shell.SetNavBarHasShadow(this, false);

            Task.Delay(2000);
            UpdateMap();
        }


        List<Place> placesList = new List<Place>();

        private void UpdateMap()
        {
            try
            {
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(MainPage)).Assembly;
                Stream stream;
                if (AppResources.Culture == new CultureInfo("ru"))
                {
                    stream = assembly.GetManifestResourceStream("Bitango_.Files.Places_ru.json");
                }
                else
                {
                    stream = assembly.GetManifestResourceStream("Bitango_.Files.Places.json");
                }
                string text = string.Empty;
                using (var reader = new StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                }

                var resultObject = JsonConvert.DeserializeObject<Places>(text);

                foreach (var place in resultObject.results)
                {
                    placesList.Add(new Place
                    {
                        PlaceName = place.name,
                        Address = place.vicinity,
                        Location = place.geometry.location,
                        Position = new Position(place.geometry.location.lat, place.geometry.location.lng),
                        //Icon = place.icon,
                        //Distance = $"{GetDistance(lat1, lon1, place.geometry.location.lat, place.geometry.location.lng, DistanceUnit.Kiliometers).ToString("N2")}km",
                        //OpenNow = GetOpenHours(place?.opening_hours?.open_now)
                    });
                }

                MyMap.ItemsSource = placesList;
                //PlacesListView.ItemsSource = placesList;
                //var loc = await Xamarin.Essentials.Geolocation.GetLocationAsync();
                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(43.2511951, 76.945676), Distance.FromKilometers(0.3)));

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}