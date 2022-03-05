using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindDoctor.Classes
{
    public class CityAndSpec
    {
        public List<Models.Cities> cities { get; set; }
        public List<Models.Specializations> specs { get; set; }

        public List<Models.Doctors> vieweddoctors { get; set; }

    }
}