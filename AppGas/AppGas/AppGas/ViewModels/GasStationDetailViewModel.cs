using AppGas.Models;
using AppGas.Services;
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

        string imageBase64;
        public string ImageBase64
        {
            get => imageBase64;
            set => SetProperty(ref imageBase64, value);
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

        GasStationModel gasStationSelected;
        public GasStationModel GasStationSelected
        {
            get => gasStationSelected;
            set => SetProperty(ref gasStationSelected, value);
        }

        public GasStationDetailViewModel()
        {
            GasStationSelected = new GasStationModel();
        }

        public GasStationDetailViewModel(GasStationModel gasStationSelected)
        {
            GasStationSelected = gasStationSelected;
            ImageBase64 = gasStationSelected.Picture;
            GasStationLatitude = gasStationSelected.Latitude;
            GasStationLongitude = gasStationSelected.Longitude;
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

                ImageBase64 = await new ImageService().ConvertImageFilePathToBase64(file.Path);
                gasStationSelected.Picture = ImageBase64;

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

                ImageBase64 = await new ImageService().ConvertImageFilePathToBase64(file.Path);
                gasStationSelected.Picture = ImageBase64;

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("AppGas", $"Se generó un error al tomar la fotografía ({ex.Message})", "OK");
            }
            
        }

        private async void SaveAction()
        {
            //guardamos la tarea en sqlite
            await App.SQLiteDatabase.SaveGasStationAsync(gasStationSelected);
            //refrescamos el listado de las tareas
            GasStationsListViewModel.GetInstance().LoadGasStations();
            //cerramos la página actual
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void DeleteAction()
        {
            //eliminamos la tarea acutal en SQLite
            await App.SQLiteDatabase.DeleteGasStationAsync(gasStationSelected);
            //refrescamos el listado de las tareas
            GasStationsListViewModel.GetInstance().LoadGasStations();
            //cerramos la página actual
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void GetLocationAction()
        {
            try
            {
                GasStationSelected.Latitude = GasStationSelected.Longitude = 0;
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    //Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    GasStationSelected.Latitude = location.Latitude;
                    GasStationSelected.Longitude = location.Longitude;
                }
                GasStationLatitude = location.Latitude;
                GasStationLongitude = location.Longitude;
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
    }
}
