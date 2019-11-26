using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocationApp.Models
{
    public class LocationModel
    {
        public int id { get; set; }
        public string ip { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public string city { get; set; }
        public List<LocationModel> list { get; set; }
        public bool DbOk { get; set; }
        public bool ApiOk { get; set; }
    }
}
