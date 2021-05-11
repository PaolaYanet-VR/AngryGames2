using AppGas.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppGas.ViewModels
{
    public class GasStationDetailViewModel : BaseViewModel
    {
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
            //ImageBase64 = taskSelected.ImageBase64;
        }
    }
}
