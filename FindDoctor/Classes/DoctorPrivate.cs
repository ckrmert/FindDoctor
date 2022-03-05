using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindDoctor.Classes
{
    public class DoctorPrivate
    {

        public Models.Doctors Doctor { get; set; }
        public List<Models.JDoctorsSchools> DoctorSchool { get; set; }

        public List<Models.Prices> DoctorPrices { get; set; }

        public List<Models.Orders> Order { get; set; }
        public List<Boolean> Orders { get; set; }

        public Models.Members Member { get; set; }

        public List<Models.Comments> Comments { get; set; }

        public Models.Favorites Favs { get; set; }

        public List<Models.Blog> DoctorBlog { get; set; }

        public Models.Comments CommentsNewList { get; set; }
    }
}