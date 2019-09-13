using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismMVVMTestProject.Models
{
    public class Country
    {
        public string name { get; set; }
        public string code { get; set; }
    }

    public class Region
    {
        public string region { get; set; }
        public string country { get; set; }
    }

    public class City
    {
        public string city { get; set; }
        public string region { get; set; }
        public string country { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
    }
}
