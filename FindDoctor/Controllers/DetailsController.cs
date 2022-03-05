using FindDoctor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace FindDoctor.Controllers
{
    public class DetailsController : Controller
    {

        ExDatabaseEntities baglanti = new ExDatabaseEntities();
        // GET: Details
        public ActionResult Index()
        {
            return View();
        }

       
        [HttpGet]
        public ActionResult ListOfDoctors(int id,int sayfa=1)
        {
           var doctor = baglanti.Doctors.Where(x => x.CityId == id).OrderByDescending(y=> y.Viewed).ToList().ToPagedList(sayfa,4);
           

            return View(doctor);
        }

        public ActionResult ListOfDoctorsUnit(int id,int sayfa = 1)
        {
            var doctor = baglanti.Doctors.Where(x => x.SpecializationId == id).OrderByDescending(y => y.Viewed).ToList().ToPagedList(sayfa, 4);

            return View(doctor);
        }


        [HttpGet]
        public PartialViewResult BestRated(int id,int sayfa = 1)
        {
            var doctor = baglanti.Doctors.Where(x => x.SpecializationId == id).OrderByDescending(y => y.TotalVote).ToList().ToPagedList(sayfa, 4);

            if (Session["MedUnit"].ToString() == "MedUnitIsActive") // birim için
            {
                doctor = baglanti.Doctors.Where(x=>x.SpecializationId==id).OrderByDescending(y => y.TotalVote).ToList().ToPagedList(sayfa, 4);
                
            }
            else { // il için
                doctor = baglanti.Doctors.Where(x => x.CityId == id).OrderByDescending(y => y.TotalVote).ToList().ToPagedList(sayfa, 4); 
            }

            return PartialView("_BestRated", doctor);

            
        }

        [HttpGet]
        public PartialViewResult MostPatiens(int id, int sayfa = 1)
        {
            var doctor = baglanti.Doctors.Where(x => x.SpecializationId == id).OrderByDescending(y => y.TotalVote).ToList().ToPagedList(sayfa, 4);

            if (Session["MedUnit"].ToString() == "MedUnitIsActive") // birimler için
            {
                doctor = baglanti.Doctors.Where(x => x.SpecializationId == id).OrderByDescending(y => y.Patiens).ToList().ToPagedList(sayfa, 4);

            }

            else // il kısmı için
            {

                doctor = baglanti.Doctors.Where(x => x.CityId == id).OrderByDescending(y => y.Patiens).ToList().ToPagedList(sayfa, 4);
            }
            return PartialView("_MostPatiens", doctor);


        }

        [HttpGet]
        public PartialViewResult MostViews(int id, int sayfa = 1)
        {
            var doctor = baglanti.Doctors.Where(x => x.SpecializationId == id).OrderByDescending(y => y.TotalVote).ToList().ToPagedList(sayfa, 4);

            if (Session["MedUnit"].ToString() == "MedUnitIsActive") // birimler için
            {
                doctor = baglanti.Doctors.Where(x => x.SpecializationId == id).OrderByDescending(y => y.Viewed).ToList().ToPagedList(sayfa, 4);

            }

            else {
                doctor = baglanti.Doctors.Where(x => x.CityId == id).OrderByDescending(y => y.Viewed).ToList().ToPagedList(sayfa, 4);
            }
            return PartialView("_MostViewed", doctor);

            
        }


        [HttpGet]
        public ActionResult DoctorPrivate(int id)
        {
            var customer = (FindDoctor.Models.Members)Session["LogonUser"];
            var onemoreview = baglanti.Doctors.Where(x => x.Id == id).FirstOrDefault();
            onemoreview.Viewed +=1;

            baglanti.SaveChanges();
            


            Classes.DoctorPrivate doctor;

            if (customer!=null)
            {
                doctor = new Classes.DoctorPrivate()
                {

                    Doctor = baglanti.Doctors.Where(x => x.Id == id).FirstOrDefault(),
                    DoctorSchool = baglanti.JDoctorsSchools.Where(y => y.DoctorId == id).ToList(),
                    DoctorPrices = baglanti.Prices.Where(z => z.DoctorId == id).ToList(),
                    Comments = baglanti.Comments.Where(x => x.DoctorId == id).ToList(),
                    Member = baglanti.Members.Where(x => x.Id == customer.Id).FirstOrDefault(),
                    Order = baglanti.Orders.Where(x => x.MemberId == customer.Id).ToList(),
                    Favs = baglanti.Favorites.Where(x => x.MemberId == customer.Id && x.DoctorId == id).FirstOrDefault(),
                    DoctorBlog = baglanti.Blog.Where(x => x.DoctorId == id).ToList(),
                    CommentsNewList = baglanti.Comments.Where(x=>x.DoctorId==id && x.MemberId==customer.Id).FirstOrDefault()
                    
                };
            }
            else {

                doctor = new Classes.DoctorPrivate() {

                    Doctor = baglanti.Doctors.Where(x => x.Id == id).FirstOrDefault(),
                    DoctorSchool = baglanti.JDoctorsSchools.Where(y => y.DoctorId == id).ToList(),
                    DoctorPrices = baglanti.Prices.Where(z => z.DoctorId == id).ToList(),
                    Comments = baglanti.Comments.Where(x => x.DoctorId == id).ToList(),
                    DoctorBlog = baglanti.Blog.Where(x => x.DoctorId == id).ToList(),
                    CommentsNewList = baglanti.Comments.Where(x => x.DoctorId == id).FirstOrDefault(),
                    Member =null
                    

            };
            }
           
            return View(doctor);
        }

        [HttpPost]
        public ActionResult DoctorPrivate(Classes.DoctorPrivate order,string txtdate,string txttime)
        {
            var customer = (FindDoctor.Models.Members)Session["LogonUser"];
            bool canComment= false;

            txtdate = txtdate.Replace('/' , '.');
            string date = DateTime.Now.ToString("MM/dd/yyyy");
           

            if ( Convert.ToInt32(date.Substring(6,4)) > Convert.ToInt32(txtdate.Substring(6,4)) )
            {
                canComment = true;
            }

            else if((Convert.ToInt32(date.Substring(6, 4)) == Convert.ToInt32(txtdate.Substring(6, 4))) && (Convert.ToInt32(date.Substring(0, 2)) > Convert.ToInt32(txtdate.Substring(0,2))) ) 
            {
                canComment= true;
            }

            else if((Convert.ToInt32(date.Substring(6, 4)) == Convert.ToInt32(txtdate.Substring(6, 4))) && (Convert.ToInt32(date.Substring(0, 2)) == Convert.ToInt32(txtdate.Substring(0, 2))) && (Convert.ToInt32(date.Substring(3, 2)) > Convert.ToInt32(txtdate.Substring(3, 2))))
            {
                canComment = true;
            }





            if (customer == null)
            {
                return RedirectToAction("Login", "Home");
            }

            else
            {
                for (int i = 0; i < order.DoctorPrices.Count; i++)
                {
                    Models.Orders neworder = new Models.Orders()
                    {
                        IsChecked = order.Orders[i],
                        PriceId = order.DoctorPrices[i].Id,
                        MemberId = customer.Id,
                        Date = txtdate,
                        Time = txttime,
                        CanComment = canComment,
                        DidPay = false,
                        IsApproved = false
                        

                        
                    };

                    baglanti.Orders.Add(neworder);
                    baglanti.SaveChanges();       // tekrar randevu alabilmek için tarihli olarak randevudan sonra db'den silinmesi gerekecek
                }

                List<Models.Orders> paymentmodel = baglanti.Orders.Where(x=>x.MemberId==customer.Id && x.IsChecked==true).ToList();
                // TempData["sss"] = order.DoctorPrices[0].Id;
               // TempData["PaymentModel"] = paymentmodel;

                return RedirectToAction("Payment/"+customer.Id, "Details");
            }
        }
        [HttpGet]
        public ActionResult Payment(int id)
        {
            List<Models.Orders> paymentmodel = baglanti.Orders.Where(x => x.MemberId == id && x.IsChecked == true && x.DidPay==false).ToList();

            return View(paymentmodel);
        }

        [HttpPost]
        public ActionResult Payment()
        {
            var customer = (FindDoctor.Models.Members)Session["LogonUser"];

            var paidorder = baglanti.Orders.Where(x => x.MemberId == customer.Id && x.DidPay==false).ToList();
            foreach (var item in paidorder)
            {
                
                item.DidPay = true;
                baglanti.Orders.Add(item);
                baglanti.SaveChanges();
            }

          var willberemoved =  baglanti.Orders.Where(x => x.MemberId == customer.Id && x.DidPay == false).ToList();
            for (int i = 0; i < willberemoved.Count; i++)
            {
                baglanti.Orders.Remove(willberemoved[i]);
                baglanti.SaveChanges();
            }
            

            return RedirectToAction("Index", "Home");
        }

        public ActionResult ClearPayment(int id)
        {
            var willbedeleted = baglanti.Orders.Where(x => x.Members.Id == id && x.DidPay == false && x.IsApproved == false).ToList();

            foreach (var item in willbedeleted)
            {
                baglanti.Orders.Remove(item);
                baglanti.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult AddComment(string txtcomment,int rngvote)
        { 
            Models.Comments comment = new Models.Comments() { 
            CContent=txtcomment,
            Date= DateTime.Now.ToString("MM/dd/yyyy"),
            DoctorId=Convert.ToInt32(TempData["DocReturnId"]),
            MemberId=Convert.ToInt32(TempData["UserReturnId"]),
            VotePoint = rngvote
            };

            int y = Convert.ToInt32(TempData["DocReturnId"]);
            int z = Convert.ToInt32(TempData["UserReturnId"]);
            var doc = baglanti.Doctors.Where(x => x.Id == y).FirstOrDefault();
            
      
                baglanti.Comments.Add(comment);
            

                doc = baglanti.Doctors.Where(x => x.Id == y).FirstOrDefault();
                doc.AverageVote += rngvote;
                doc.TotalVote += 1;


            baglanti.SaveChanges();
            return RedirectToAction("DoctorPrivate/"+TempData["DocReturnId"], "Details");
        }

        public ActionResult Favorites(int id)
        {
            Models.Favorites fav = new Models.Favorites();

            if (Session["LogonUser"]==null)
            {
                return RedirectToAction("Login","Home");
            }

            else
            {
                var member = (FindDoctor.Models.Members)Session["LogonUser"];
                fav.MemberId = member.Id;
                fav.DoctorId = id;

                baglanti.Favorites.Add(fav);
                baglanti.SaveChanges();
                return RedirectToAction("DoctorPrivate/" + id, "Details");

            }
           
          

           
        }

        public ActionResult NotFavorite(int id)
        {
            FindDoctor.Models.Members user = (FindDoctor.Models.Members)Session["LogonUser"];
            Models.Favorites doctorwillbedeleted = baglanti.Favorites.Where(x => x.DoctorId == id && x.MemberId == user.Id).FirstOrDefault();

            baglanti.Favorites.Remove(doctorwillbedeleted);
            baglanti.SaveChanges();

            return RedirectToAction("UserFavorites", "Home");
        }


        public ActionResult Blogs()
        {
            Classes.Blog blogmodel = new Classes.Blog()
            {
                BlogCategories = baglanti.BlogCategories.ToList(),
                BlogList = baglanti.Blog.OrderByDescending(x => x.Id).ToList()
        };

            foreach (var item in blogmodel.BlogList)
            {
                item.Text = item.Text.Substring(0, 600);
                item.Text += ">>devamını oku..";
            }

            var blog = baglanti.Blog.OrderByDescending(x => x.Id).ToList();
            foreach (var item in blog)
            {
                item.Text = item.Text.Substring(0, 600);
                item.Text += ">>devamını oku..";
            }

            return View(blogmodel);
        }

        [HttpGet]
        public PartialViewResult BlogMore()
        {
            var model = baglanti.Blog.OrderByDescending(x => x.Id).ToList();
            foreach (var item in model)
            {
                item.Text = item.Text.Substring(0, 600);
                item.Text += ">>devamını oku..";
            }

            return PartialView("BlogMore",model);
        }

        [HttpGet]
        public PartialViewResult BlogPartial(int id)
        {
           var modelblog = baglanti.Blog.Where(x => x.CategoryId == id).ToList();

            return PartialView("_BlogPartial", modelblog);
        }

        [HttpGet]
        public ActionResult BlogText(int id)
        {
            Classes.Blog blog = new Classes.Blog()
            {

                Blog1 = baglanti.Blog.Where(x=>x.Id==id).FirstOrDefault(),
                ViewBlogComments = null,
                BlogComments = baglanti.BlogComments.Where(x => x.BlogId == id).ToList()
            };


            int y= blog.Blog1.CategoryId;
            blog.BlogList = baglanti.Blog.Where(x => x.BlogCategories.Id == y).ToList();
            return View(blog);
        }

        [HttpPost]
        public ActionResult BlogText(int id,Classes.Blog model)
        {
            FindDoctor.Models.Members member = (FindDoctor.Models.Members)Session["LogonUser"];


            int blogid = id; 
            int memberid = member.Id;
            DateTime date = DateTime.Now;
            string datestring = date.ToString();
          
            string content = model.ViewBlogComments.Text;

                Models.BlogComments comment = new Models.BlogComments()
                {
                   MemberId=memberid,
                   BlogId= blogid,
                   Date=datestring,
                   Text=content

                    
                };

            baglanti.BlogComments.Add(comment);
            baglanti.SaveChanges();
            return RedirectToAction("BlogText/"+blogid, "Details");
        }

        public ActionResult DeleteBlog(int id)
        {
            var willbedeleted = baglanti.Blog.Where(x => x.Id == id).FirstOrDefault();
            baglanti.Blog.Remove(willbedeleted);
            baglanti.SaveChanges();

            return RedirectToAction("Index", "DoctorPanel");
        }

        [HttpGet]
        public ActionResult AddBlog()
        {
            FindDoctor.Models.Doctors doctor = (FindDoctor.Models.Doctors)Session["LogonDoctor"];

            if (doctor == null)
            {
               return RedirectToAction("LoginDoctor", "Home");
            }

            else
            {
                List<SelectListItem> selected = (from i in baglanti.BlogCategories.ToList()
                                                 select new SelectListItem
                                                 {
                                                     Text = i.CategoryName,
                                                     Value = i.Id.ToString()
                                                 }).ToList();



                ViewBag.CategoryNames = selected;


                return View();
            }
        }

        [HttpPost]
        public ActionResult AddBlog(Classes.Blog model)
        {
            string date = DateTime.Now.ToString("MM/dd/yyyy");
            FindDoctor.Models.Doctors doctor = (FindDoctor.Models.Doctors)Session["LogonDoctor"];
            model.Blog1.Date = date;
            model.Blog1.DoctorId = doctor.Id;
            

            if (Request.Files.Count > 0)
            {
                string filename = Path.GetFileName(Request.Files[0].FileName);
                string url = Path.GetExtension(Request.Files[0].FileName);
                string path = "~/Image/" + filename + url;
                Request.Files[0].SaveAs(Server.MapPath(path));
                model.Blog1.ImageUrl = "/Image/" + filename + url;
                

            }

            baglanti.Blog.Add(model.Blog1);
            baglanti.SaveChanges();

            return RedirectToAction("PanelBlog", "DoctorPanel");
        }



    }
}