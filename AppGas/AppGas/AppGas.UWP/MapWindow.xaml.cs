using AppGas.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Control de usuario está documentada en https://go.microsoft.com/fwlink/?LinkId=234236

namespace AppGas.UWP
{
    public sealed partial class MapWindow : UserControl
    {
        public MapWindow(GasStationModel gasStation)
        {
            this.InitializeComponent();

            WindowPicture.Source = new BitmapImage(new Uri(gasStation.Picture));
            WindowBranchOffice.Text = gasStation.BranchOffice;
            WindowCompany.Text = gasStation.Company;
        }
    }
}
