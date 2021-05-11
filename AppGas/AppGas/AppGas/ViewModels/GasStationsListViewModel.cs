using AppGas.Models;
using AppGas.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppGas.ViewModels
{
    public class GasStationsListViewModel : BaseViewModel
    {
        private static GasStationsListViewModel instance;

        Command newCommand;
        public Command NewCommand => newCommand ?? (newCommand = new Command(NewAction));

        List<GasStationModel> gasStations;
        public List<GasStationModel> GasStations
        {
            get => gasStations;
            set => SetProperty(ref gasStations, value);
        }

        GasStationModel gasStationSelected;
        public GasStationModel GasStationSelected
        {
            get => gasStationSelected;
            set
            {
                if (SetProperty(ref gasStationSelected, value))
                {
                    SelectAction();
                }

            }
        }

        public GasStationsListViewModel()
        {
            instance = this;

            LoadGasStations();
        }

        public static GasStationsListViewModel GetInstance()
        {
            return instance;
        }

        public async void LoadGasStations()
        {
            GasStations = await App.SQLiteDatabase.GetAllGasStationsAsync();
        }

        private void NewAction()
        {
            Application.Current.MainPage.Navigation.PushAsync(new GasStationDetailView());
        }

        private void SelectAction()
        {
            Application.Current.MainPage.Navigation.PushAsync(new GasStationDetailView(gasStationSelected));
        }
    }
}
