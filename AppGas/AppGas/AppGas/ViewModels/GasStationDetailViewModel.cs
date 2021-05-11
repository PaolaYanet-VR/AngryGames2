using AppGas.Models;
using AppGas.Services;
using AppGas.Views;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppGas.ViewModels
{
    public class GasStationDetailViewModel : BaseViewModel
    {
        GasStationsListViewModel GasStationsListViewModel;

        Command takePictureCommand;
        public Command TakePictureCommand => takePictureCommand ?? (takePictureCommand = new Command(TakePictureAction));

        Command selectPictureCommand;
        public Command SelectPictureCommand => selectPictureCommand ?? (selectPictureCommand = new Command(SelectPictureAction));

        Command saveCommand;
        public Command SaveCommand => saveCommand ?? (saveCommand = new Command(SaveAction));

        Command deleteCommand;
        public Command DeleteCommand => deleteCommand ?? (deleteCommand = new Command(DeleteAction));

        Command getLocationCommand;
        public Command GetLocationCommand => getLocationCommand ?? (getLocationCommand = new Command(GetLocationAction));

        Command _MapCommand;
        public Command MapCommand => _MapCommand ?? (_MapCommand = new Command(MapAction));

        int gasStationID;

        string gasStationCompany;
        public string GasStationCompany
        {
            get => gasStationCompany;
            set => SetProperty(ref gasStationCompany, value);
        }

        string gasStationBranchOffice;
        public string GasStationBranchOffice
        {
            get => gasStationBranchOffice;
            set => SetProperty(ref gasStationBranchOffice, value);
        }

        string gasStationPicture;
        public string GasStationPicture
        {
            get => gasStationPicture;
            set => SetProperty(ref gasStationPicture, value);
        }

        double gasStationGreenPrice;
        public double GasStationGreenPrice
        {
            get => gasStationGreenPrice;
            set => SetProperty(ref gasStationGreenPrice, value);
        }

        double gasStationRedPrice;
        public double GasStationRedPrice
        {
            get => gasStationRedPrice;
            set => SetProperty(ref gasStationRedPrice, value);
        }

        double gasStationDieselPrice;
        public double GasStationDieselPrice
        {
            get => gasStationDieselPrice;
            set => SetProperty(ref gasStationDieselPrice, value);
        }

        double gasStationLatitude;
        public double GasStationLatitude
        {
            get => gasStationLatitude;
            set => SetProperty(ref gasStationLatitude, value);
        }

        double gasStationLongitude;
        public double GasStationLongitude
        {
            get => gasStationLongitude;
            set => SetProperty(ref gasStationLongitude, value);
        }

        string imageBase64;
        public string ImageBase64
        {
            get => imageBase64;
            set => SetProperty(ref imageBase64, value);
        }

        GasStationModel gasStationSelected;
        public GasStationModel GasStationSelected
        {
            get => gasStationSelected;
            set => SetProperty(ref gasStationSelected, value);
        }

        // Constructors
        public GasStationDetailViewModel(GasStationsListViewModel gasStationsListViewModel)
        {
            GasStationsListViewModel = gasStationsListViewModel;
        }

        public GasStationDetailViewModel(GasStationsListViewModel gasStationsListViewModel, GasStationModel gasStation)
        {
            GasStationsListViewModel = gasStationsListViewModel;

            ImageBase64 = gasStation.Picture;

            //Pet = pet;
            gasStationID = gasStation.ID;
            GasStationCompany = gasStation.Company;
            GasStationBranchOffice = gasStation.BranchOffice;
            GasStationPicture = gasStation.Picture;
            GasStationGreenPrice = gasStation.GreenPrice;
            GasStationRedPrice = gasStation.RedPrice;
            GasStationDieselPrice = gasStation.DieselPrice;
            GasStationLatitude = gasStation.Latitude;
            GasStationLongitude = gasStation.Longitude;
        }

        // Actions
        private async void SaveAction()
        {
            GasStationModel gasStation = new GasStationModel
            {
                ID = gasStationID,
                Company = gasStationCompany,
                BranchOffice = gasStationBranchOffice,
                Picture = gasStationPicture,
                GreenPrice = gasStationGreenPrice,
                RedPrice = gasStationRedPrice,
                DieselPrice = gasStationDieselPrice,
                Latitude = gasStationLatitude,
                Longitude = gasStationLongitude
            };

            //guardamos la tarea en sqlite
            await App.SQLiteDatabase.SaveGasStationAsync(gasStation);
            //refrescamos el listado de las tareas
            GasStationsListViewModel.GetInstance().LoadGasStations();
            //cerramos la página actual
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void DeleteAction()
        {
            GasStationModel gasStation = new GasStationModel
            {
                ID = gasStationID,
                Company = gasStationCompany,
                BranchOffice = gasStationBranchOffice,
                Picture = gasStationPicture,
                GreenPrice = gasStationGreenPrice,
                RedPrice = gasStationRedPrice,
                DieselPrice = gasStationDieselPrice,
                Latitude = gasStationLatitude,
                Longitude = gasStationLongitude
            };

            //eliminamos la tarea acutal en SQLite
            await App.SQLiteDatabase.DeleteGasStationAsync(gasStation);
            //refrescamos el listado de las tareas
            GasStationsListViewModel.GetInstance().LoadGasStations();
            //cerramos la página actual
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void GetLocationAction()
        {
            try
            {
                GasStationLatitude = GasStationLongitude = 0;
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    //Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    GasStationLatitude = location.Latitude;
                    GasStationLongitude = location.Longitude;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                await Application.Current.MainPage.DisplayAlert("AppGas", $"El GPS no está soportado en el dispositivo ({ fnsEx.Message })", "Ok");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                await Application.Current.MainPage.DisplayAlert("AppGas", $"El GPS no está activado en el dispositivo ({ fneEx.Message })", "Ok");

            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                await Application.Current.MainPage.DisplayAlert("AppGas", $"No se pudo obtener el permiso para las coordenadas ({ pEx.Message })", "Ok");
            }
            catch (Exception ex)
            {
                // Unable to get location
                await Application.Current.MainPage.DisplayAlert("AppGas", $"Se generó un error al obtener las coordenadas del dispositivo ({ ex.Message })", "Ok");
            }
        }

        private async void TakePictureAction()
        {
            try
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await Application.Current.MainPage.DisplayAlert("AppGas", "No existe camara disponible en el dispositivo", "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "AppGas",
                    Name = "GasStationPicture.jpg"
                });

                if (file == null)
                    return;

                GasStationPicture = await new ImageService().ConvertImageFilePathToBase64(file.Path);

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("AppGas", $"Se generó un error al tomar la fotografía ({ex.Message})", "OK");
            }

        }

        private async void SelectPictureAction()
        {
            try
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await Application.Current.MainPage.DisplayAlert("AppGas", "No es posible seleccionar fotgrafías en el dispositivo", "OK");
                    return;
                }

                var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                });

                if (file == null)
                    return;

                GasStationPicture = await new ImageService().ConvertImageFilePathToBase64(file.Path);

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("AppGas", $"Se generó un error al tomar la fotografía ({ex.Message})", "OK");
            }
            
        }

        private void MapAction()
        {
            Application.Current.MainPage.Navigation.PushAsync(
                new GasStationsMapsView(new GasStationModel 
                {
                    ID = gasStationID,
                    Company = gasStationCompany,
                    BranchOffice = gasStationBranchOffice,
                    Picture = gasStationPicture,
                    GreenPrice = gasStationGreenPrice,
                    RedPrice = gasStationRedPrice,
                    DieselPrice = gasStationDieselPrice,
                    Latitude = gasStationLatitude,
                    Longitude = gasStationLongitude
                })
            );
        }
    }
}
