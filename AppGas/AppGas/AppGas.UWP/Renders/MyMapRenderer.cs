using AppGas.Models;
using AppGas.Renders;
using AppGas.UWP.Renders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls.Maps;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.UWP;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(MyMap), typeof(MyMapRenderer))]
namespace AppGas.UWP.Renders
{
    public class MyMapRenderer : MapRenderer
    {
        MapControl NativeMap;
        GasStationModel GasStation;
        MapWindow GasStationWindow;
        bool IsGasStationWindowVisible = false;

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                NativeMap.MapElementClick -= OnMapElementClick;
                NativeMap.Children.Clear();
                NativeMap = null;
                GasStationWindow = null;
            }

            if (e.NewElement != null)
            {
                this.GasStation = (e.NewElement as MyMap).GasStation;

                var formsMap = (MyMap)e.NewElement;
                NativeMap = Control as MapControl;
                NativeMap.Children.Clear();
                NativeMap.MapElementClick += OnMapElementClick;

                var position = new BasicGeoposition
                {
                    Latitude = GasStation.Latitude,
                    Longitude = GasStation.Longitude
                };
                var point = new Geopoint(position);

                var mapIcon = new MapIcon();
                mapIcon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///pin.png"));
                mapIcon.CollisionBehaviorDesired = MapElementCollisionBehavior.RemainVisible;
                mapIcon.Location = point;
                mapIcon.NormalizedAnchorPoint = new Windows.Foundation.Point(0.5, 1.0);

                NativeMap.MapElements.Add(mapIcon);

            }
        }

        private void OnMapElementClick(MapControl sender, MapElementClickEventArgs args)
        {
            var mapIcon = args.MapElements.FirstOrDefault(x => x is MapIcon) as MapIcon;
            if (mapIcon != null)
            {
                if (!IsGasStationWindowVisible)
                {
                    if (GasStationWindow == null) GasStationWindow = new MapWindow(GasStation);

                    var position = new BasicGeoposition
                    {
                        Latitude = GasStation.Latitude,
                        Longitude = GasStation.Longitude
                    };
                    var point = new Geopoint(position);

                    NativeMap.Children.Add(GasStationWindow);
                    MapControl.SetLocation(GasStationWindow, point);
                    MapControl.SetNormalizedAnchorPoint(GasStationWindow, new Windows.Foundation.Point(0.5, 1.0));

                    IsGasStationWindowVisible = true;
                }
                else
                {
                    NativeMap.Children.Remove(GasStationWindow);
                    IsGasStationWindowVisible = false;
                }
            }
        }
    }
}
