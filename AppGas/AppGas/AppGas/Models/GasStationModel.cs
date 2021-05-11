using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppGas.Models
{
    [Table("GasStation")]
    public class GasStationModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Company { get; set; }
        public string BranchOffice { get; set; }
        public string Picture { get; set; }
        public double GreenPrice { get; set; }
        public double RedPrice { get; set; }
        public double DieselPrice { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
