using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindDoctor.Classes
{
    public class Blog
    {

        public List<FindDoctor.Models.Blog> BlogList { get; set; }

        public FindDoctor.Models.Blog Blog1 { get; set; }

        public List<Models.BlogCategories> BlogCategories { get; set; }

        public List<Models.BlogComments> BlogComments { get; set; }

        public Models.BlogComments ViewBlogComments { get; set; }

    }
}