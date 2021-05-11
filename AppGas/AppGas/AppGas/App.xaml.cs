using AppGas.Data;
using AppGas.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace AppGas
{
    public partial class App : Application
    {
        private static SQLiteDatabase _SQLiteDatabase;

        public static SQLiteDatabase SQLiteDatabase
        {
            get
            {
                if (_SQLiteDatabase == null) _SQLiteDatabase = new SQLiteDatabase();
                return _SQLiteDatabase;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new GasStationsListView());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
