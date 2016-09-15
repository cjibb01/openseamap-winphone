//---------------------------------------------------------------------------
//
// <copyright file="PivotHomePage.xaml.cs" company="Microsoft">
//    Copyright (C) 2015 by Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <createdOn>9/2/2016 10:06:02 AM</createdOn>
//
//---------------------------------------------------------------------------

using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using AppStudio.Uwp;
using AppStudio.Uwp.Commands;
using AppStudio.Uwp.Navigation;

using OpenSeaMap.ViewModels;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;

namespace OpenSeaMap.Pages
{
    public sealed partial class PivotHomePage : Page
    {
        public PivotHomePage()
        {
            ViewModel = new MainViewModel(0);            			
            AboutThisAppModel = new AboutThisAppViewModel();
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
			commandBar.DataContext = ViewModel;
			searchBox.SearchCommand = SearchCommand;
			this.SizeChanged += OnSizeChanged;

            myMap.MapServiceToken = "dLNVGx5JD4EU3FM03Vtr~OpYi0nYDAdVDNmzUUsb5Uw~AnbJ3KAO92vJ7ItnjUG1NBpBJ0pxL6spGjgbMUMqeboMF-XoPK9ZUVx0dUxTIUYF";
            myMap.Loaded += MyMap_Loaded;
            myMap.MapTapped += MyMap_MapTapped;
        }

        private void MyMap_Loaded(object sender, RoutedEventArgs e)
        {
            myMap.Center =
                new Geopoint(new BasicGeoposition()
                {
                    //Geopoint for Seattle 
                    Latitude = 47.604,
                    Longitude = -122.329
                });
            myMap.ZoomLevel = 12;

            // Create an HTTP data source.
            // This example retrieves tiles from a fictitious web service.
            HttpMapTileDataSource dataSource = new HttpMapTileDataSource("http://tiles.openseamap.org/seamark/{zoomlevel}/{x}/{y}.png");
            //"http://www.<web service name>.com/z={zoomlevel}&x={x}&y={y}");

            // Optionally, add custom HTTP headers if the web service requires them.
            //dataSource.AdditionalRequestHeaders.Add("header name", "header value");

            // Create a tile source and add it to the Map control.
            MapTileSource tileSource = new MapTileSource(dataSource);
            //tileSource.Bounds = boundingBox;
            myMap.TileSources.Add(tileSource);

        }

        private void MyMap_MapTapped(Windows.UI.Xaml.Controls.Maps.MapControl sender, Windows.UI.Xaml.Controls.Maps.MapInputEventArgs args)
        {
            var tappedGeoPosition = args.Location.Position;
            //string status = "MapTapped at \nLatitude:" + tappedGeoPosition.Latitude + "\nLongitude: " + tappedGeoPosition.Longitude;
            //rootPage.NotifyUser(status, NotifyType.StatusMessage);
        }

        public MainViewModel ViewModel { get; set; }
        public AboutThisAppViewModel AboutThisAppModel { get; private set; }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            //await this.ViewModel.LoadDataAsync();
            //Page cache requires set commandBar in code
            ShellPage.Current.ShellControl.SetCommandBar(commandBar);
            ShellPage.Current.ShellControl.SelectItem("Home");
            //rootPage = MainPage.Current;
        }
		public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand<string>(text =>
                {
                    searchBox.Reset();
                    ShellPage.Current.ShellControl.CloseLeftPane();
                    NavigationService.NavigateToPage("SearchPage", text, true);
                },
                SearchViewModel.CanSearch);
            }
        }

		private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            searchBox.SearchWidth = e.NewSize.Width > 640 ? 230 : 190;
        }

        private void TrafficFlowVisible_Checked(object sender, RoutedEventArgs e)
        {
            myMap.TrafficFlowVisible = true;
        }

        private void trafficFlowVisibleCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            myMap.TrafficFlowVisible = false;
        }

        private void styleCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (styleCombobox.SelectedIndex)
            {
                case 0:
                    myMap.Style = MapStyle.None;
                    break;
                case 1:
                    myMap.Style = MapStyle.Road;
                    break;
                case 2:
                    myMap.Style = MapStyle.Aerial;
                    break;
                case 3:
                    myMap.Style = MapStyle.AerialWithRoads;
                    break;
                case 4:
                    myMap.Style = MapStyle.Terrain;
                    break;
            }
        }
    }
}
