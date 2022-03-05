using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindDoctor.Classes
{
    public class DoctorPanel
    {

        public List<Models.Comments> CommentsList { get; set; }

        public List<Models.Comments> CommentsCount { get; set; }

        public PagedList.IPagedList<Models.Favorites> FavoritesList { get; set; }
    }
}