using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FindDoctor.Models;
using PagedList;
using PagedList.Mvc;

namespace FindDoctor.Controllers
{
    public class DoctorPanelController : Controller
    {
        ExDatabaseEntities baglanti = new ExDatabaseEntities();

        // GET: DoctorPanel
        public ActionResult Index()
        {
            FindDoctor.Models.Doctors doc = (FindDoctor.Models.Doctors)Session["LogonDoctor"];

            Classes.PanelMain model = new Classes.PanelMain() {

                Doctor = baglanti.Doctors.Where(x => x.Id == doc.Id).FirstOrDefault(),
                Orders = baglanti.Orders.Where(x => x.Prices.DoctorId == doc.Id && x.IsApproved == true).ToList(),
                Favorites = baglanti.Favorites.Where(x => x.DoctorId == doc.Id).ToList(),
                Blogs = baglanti.Blog.Where(x => x.DoctorId == doc.Id).ToList(),
                Com1 =baglanti.Comments.Where(x=>x.DoctorId==doc.Id).ToList(),
                Com2 = baglanti.BlogComments.Where(x=>x.Blog.DoctorId==doc.Id).ToList()

            };

            return View(model);
        }

        [HttpGet]
        public ActionResult DoctorProfile()
        {
            FindDoctor.Models.Doctors doctor = (FindDoctor.Models.Doctors)Session["LogonDoctor"];

            Classes.DoctorProfile doctormodel = new Classes.DoctorProfile()
            {

                Doctor = baglanti.Doctors.Where(x => x.Id == doctor.Id).FirstOrDefault(),
                Price = baglanti.Prices.Where(x => x.DoctorId == doctor.Id).FirstOrDefault(),
                PriceList = baglanti.Prices.Where(x => x.DoctorId == doctor.Id).ToList()

            };



            return View(doctormodel);
        }
        [HttpPost]
        public ActionResult DoctorProfile(Classes.DoctorProfile model, string txtprice, string txtservice)
        {
            FindDoctor.Models.Doctors doctor = (FindDoctor.Models.Doctors)Session["LogonDoctor"];
            Classes.DoctorProfile willbeupdated = new Classes.DoctorProfile();
            Models.Prices willbeadded = new Models.Prices();

            willbeupdated.Doctor = baglanti.Doctors.Where(x => x.Id == doctor.Id).FirstOrDefault();




            //if (Request.Files.Count > 0)
            //{
            //    string filename = Path.GetFileName(Request.Files[0].FileName);
            //    string url = Path.GetExtension(Request.Files[0].FileName);
            //    string path = "~/Image/" + filename + url;
            //    Request.Files[0].SaveAs(Server.MapPath(path));
            //    model.Doctor.ProfileImage = "/Image/" + filename + url;


            //}

            if (Request.Files != null && Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                
                var folder = Server.MapPath("~/Image/");
                var fileName = Guid.NewGuid() + ".jpg";
                file.SaveAs(Path.Combine(folder, fileName));
                var filePath = "/Image/" + fileName;
                model.Doctor.ProfileImage = filePath;
            }

            willbeupdated.Doctor.Name = model.Doctor.Name;
            willbeupdated.Doctor.OfficePhone = model.Doctor.OfficePhone;
            willbeupdated.Doctor.Phone = model.Doctor.Phone;
            willbeupdated.Doctor.Mail = model.Doctor.Mail;
            willbeupdated.Doctor.Biography = model.Doctor.Biography;
            willbeupdated.Doctor.Adress = model.Doctor.Adress;
            willbeupdated.Doctor.ProfileImage = model.Doctor.ProfileImage;

            if (txtprice != "" && txtservice != "")
            {
                willbeadded.DoctorId = doctor.Id;
                willbeadded.ServiceName = txtservice;
                willbeadded.Price = Convert.ToDecimal(txtprice);
                baglanti.Prices.Add(willbeadded);
            }


            baglanti.SaveChanges();

            return RedirectToAction("Index", "DoctorPanel");
        }


        public ActionResult PanelComments(int id)
        {
            FindDoctor.Models.Doctors doctor = (FindDoctor.Models.Doctors)Session["LogonDoctor"];

            int willtake = 5;
            int willskip = willtake * id;

            Classes.DoctorPanel comments = new Classes.DoctorPanel();
            comments.CommentsList = baglanti.Comments.Where(x => x.DoctorId == doctor.Id).Take(5).ToList();
            comments.CommentsCount = baglanti.Comments.Where(x => x.DoctorId == doctor.Id).ToList();

            if (id == 0)
            {
                comments.CommentsList = baglanti.Comments.Where(x => x.DoctorId == doctor.Id).Take(5).ToList();
            }

            else
            {
                comments.CommentsList = baglanti.Comments.Where(x => x.DoctorId == doctor.Id).OrderBy(x => x.Id).Skip(willskip).Take(5).ToList();
            }

            return View(comments);
        }

        public ActionResult PanelWhoAreFav(int sayfa = 1)
        {
            FindDoctor.Models.Doctors doctor = (FindDoctor.Models.Doctors)Session["LogonDoctor"];

            //Classes.DoctorPanel favlist = new Classes.DoctorPanel();
            // favlist.FavoritesList = baglanti.Favorites.Where(x => x.DoctorId == doctor.Id).ToList().ToPagedList(sayfa, 5);

            var favlist = baglanti.Favorites.Where(x => x.DoctorId == doctor.Id).ToList().ToPagedList(sayfa, 5);

            return View(favlist);
        }

        public ActionResult PanelBlog()
        {
            FindDoctor.Models.Doctors doctor = (FindDoctor.Models.Doctors)Session["LogonDoctor"];

            var bloglist = baglanti.Blog.Where(x => x.DoctorId == doctor.Id).OrderByDescending(x => x.Id).ToList();


            return View(bloglist);
        }

        public ActionResult PanelAllDoctors()
        {
            var model = baglanti.Doctors.OrderBy(x => x.Specializations.Id).ToList();

            return View(model);
        }

        public ActionResult PanelApprove()
        {
            FindDoctor.Models.Doctors doctor = (FindDoctor.Models.Doctors)Session["LogonDoctor"];
            var booking = baglanti.Orders.Where(x => x.IsApproved == false && x.Prices.DoctorId == doctor.Id && x.IsChecked == true && x.DidPay == true).ToList();

            return View(booking);

        }

        public ActionResult PanelAllOrders()
        {
            FindDoctor.Models.Doctors doctor = (FindDoctor.Models.Doctors)Session["LogonDoctor"];
            var booking = baglanti.Orders.Where(x => x.IsApproved == true && x.Prices.DoctorId == doctor.Id && x.IsChecked == true).ToList();

            return View(booking);

        }

        public ActionResult Approved(int id)
        {
            FindDoctor.Models.Doctors doctor = (FindDoctor.Models.Doctors)Session["LogonDoctor"];
            var approved = baglanti.Orders.Where(x => x.Id == id).FirstOrDefault();
            var totalpatiens = baglanti.Doctors.Where(x => x.Id == doctor.Id).FirstOrDefault();
            totalpatiens.Patiens += 1;
            approved.IsApproved = true;
            baglanti.SaveChanges();

            return RedirectToAction("PanelApprove");
        }
        public ActionResult NotApproved(int id)
        {
            var approved = baglanti.Orders.Where(x => x.Id == id).FirstOrDefault();
            baglanti.Orders.Remove(approved);
            baglanti.SaveChanges();

            return RedirectToAction("PanelApprove");
        }

    }
}