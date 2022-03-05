using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindDoctor.Classes
{
    public class PanelMain
    {

        public Models.Doctors Doctor { get; set; }
        public List<Models.Orders> Orders { get; set; }

        public List<Models.Favorites> Favorites { get; set; }

        public List<Models.Blog> Blogs { get; set; }

        public List<Models.Comments> Com1 { get; set; }

        public List<Models.BlogComments> Com2 { get; set; }
    }
}