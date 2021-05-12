using AppGas.Models;
using AppGas.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace AppGas.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GasStationDetailView : ContentPage
    {
        public GasStationDetailView(GasStationsListViewModel gasStationsListViewModel)
        {
            InitializeComponent();

            BindingContext = new GasStationDetailViewModel(gasStationsListViewModel);
        }

        public GasStationDetailView(GasStationsListViewModel gasStationsListViewModel, GasStationModel gasStation)
        {
            InitializeComponent();

            BindingContext = new GasStationDetailViewModel(gasStationsListViewModel, gasStation);
        }
    }
}