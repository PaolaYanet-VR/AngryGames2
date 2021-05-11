using AppGas.Models;
using AppGas.Renders;
using AppGas.UWP.Renders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls.Maps;
using Xamarin.Forms.Maps.UWP;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(MyMap), typeof(MyMapRenderer))]
namespace AppGas.UWP.Renders
{
    public class MyMapRenderer : MapRenderer
    {
        MapControl NativeMap;
        GasStationModel GasStation;
        MapWindow PetWindow;
        bool IsPetWindowVisible = false;

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                NativeMap.MapElementClick -= OnMapElementClick;
                NativeMap.Children.Clear();
                NativeMap = null;
                PetWindow = null;
            }

            if (e.NewElement != null)
            {
                this.Pet = (e.NewElement as MyMap).Pet;

                var formsMap = (MyMap)e.NewElement;
                NativeMap = Control as MapControl;
                NativeMap.Children.Clear();
                NativeMap.MapElementClick += OnMapElementClick;

                var position = new BasicGeoposition
                {
                    Latitude = Pet.Latitude,
                    Longitude = Pet.Longitude
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
                if (!IsPetWindowVisible)
                {
                    if (PetWindow == null) PetWindow = new MapWindow(Pet);

                    var position = new BasicGeoposition
                    {
                        Latitude = Pet.Latitude,
                        Longitude = Pet.Longitude
                    };
                    var point = new Geopoint(position);

                    NativeMap.Children.Add(PetWindow);
                    MapControl.SetLocation(PetWindow, point);
                    MapControl.SetNormalizedAnchorPoint(PetWindow, new Windows.Foundation.Point(0.5, 1.0));

                    IsPetWindowVisible = true;
                }
                else
                {
                    NativeMap.Children.Remove(PetWindow);
                    IsPetWindowVisible = false;
                }
            }
        }
    }
}
