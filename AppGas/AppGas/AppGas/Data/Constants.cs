using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AppGas.Data
{
    public class Constants
    {
        //constante para abrir nuestro archivo SQLite en modo de lectura, escritura, crear (si no existe) y cache compartido
        public const SQLite.SQLiteOpenFlags Flags = SQLite.SQLiteOpenFlags.ReadWrite |
                                                    SQLite.SQLiteOpenFlags.Create |
                                                    SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath
        {
            get
            {
                //formula la ruta donde generaremos y accederemos al archivo de SQLite para esta app
                string basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, "AppGas.db3"); //breakpoint para el basepath
            }
        }
    }
}
