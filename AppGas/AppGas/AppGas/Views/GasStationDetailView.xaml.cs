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
        public GasStationDetailView()
        {
            InitializeComponent();

            BindingContext = new GasStationDetailViewModel();
        }

        public GasStationDetailView(GasStationModel gasStationSelected)
        {
            InitializeComponent();

            BindingContext = new GasStationDetailViewModel(gasStationSelected);
        }
    }
}