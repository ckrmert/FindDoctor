using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindDoctor.Classes
{
    public class DoctorProfile
    {

        public Models.Doctors Doctor { get; set; }

        public Models.Prices Price { get; set; }
        
        public List<Models.Prices> PriceList { get; set; }

        public List<Models.Prices> PriceListViewtoController { get; set; }


    }
}