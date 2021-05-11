using AppGas.Models;
using AppGas.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace AppGas.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GasStationsMapsView : ContentPage
    {
        public GasStationsMapsView(GasStationModel gasStationSelected)
        {
            InitializeComponent();

            gasStationSelected.Picture = new ImageService().SaveImageFromBase64(gasStationSelected.Picture, gasStationSelected.ID);

            MapGasStations.GasStation = gasStationSelected;

            //centra el mapa con las coordenadas de la mascota
            MapGasStations.MoveToRegion(
                MapSpan.FromCenterAndRadius(
                    new Position(
                        gasStationSelected.Latitude,
                        gasStationSelected.Longitude
                        ),
                    Distance.FromMiles(.5)
                  )
             );

            //agrega un pin al mapa con las coordenadas de la mascota
            MapGasStations.Pins.Add(
                    new Pin
                    {
                        Type = PinType.Place,
                        Label = gasStationSelected.BranchOffice,
                        Position = new Position(
                        gasStationSelected.Latitude,
                        gasStationSelected.Longitude
                        )
                    }
                );
        }
    }
}