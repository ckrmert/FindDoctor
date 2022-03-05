using FindDoctor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FindDoctor.Controllers
{
    public class HomeController : Controller
    {

        ExDatabaseEntities baglanti = new ExDatabaseEntities();
        // GET: Home
        public ActionResult Index()
        {
            Classes.CityAndSpec model = new Classes.CityAndSpec()
            {
                cities = baglanti.Cities.ToList(),
                specs = baglanti.Specializations.ToList(),
                vieweddoctors = baglanti.Doctors.OrderByDescending(x => x.Viewed).Take(5).ToList() // aslında orderby yapmamız lazımdı ama view'da foreach ile dönüp en sona en yüksek viewlı doktoru attığı için burada terse çevirdik

            };


            //List<Models.Cities> cities = baglanti.Cities.ToList();
            //List<Models.Specializations> specs = baglanti.Specializations.ToList();


            return View(model);
        }

        [HttpPost]
        public ActionResult Index(string txtsearch, int sayfa = 1)
        {
            if (baglanti.Doctors.Any(x => x.Cities.Name == txtsearch))
            {

                var doctor = baglanti.Doctors.Where(x => x.Cities.Name == txtsearch).OrderByDescending(y => y.Viewed).ToList();
                int id = doctor[0].CityId;

                return RedirectToAction(("ListOfDoctors/" + id), "Details");
            }

            else if (baglanti.Doctors.Any(x => x.Specializations.Name == txtsearch))
            {
                var doctor = baglanti.Doctors.Where(x => x.Specializations.Name == txtsearch).OrderByDescending(y => y.Viewed).FirstOrDefault();
                int id = doctor.SpecializationId;
                return RedirectToAction(("ListOfDoctorsUnit/" + id), "Details");
            }
            else return RedirectToAction("Index");

        }




        [HttpGet]
        public PartialViewResult ListCity()
        {

            return PartialView(baglanti.Cities.ToList());


        }

        [HttpGet]
        public ActionResult Register()
        {

            return View();
        }


        [HttpPost]
        public ActionResult Register(FindDoctor.Models.Members member, string textboxsifre, string chc)
        {


            if (!baglanti.Members.Any(x => x.Mail == member.Mail))
            {
                if (member.Password == textboxsifre && chc == "true")
                {
                    member.Password = Classes.Password.Encrypt(member.Password);
                    baglanti.Members.Add(member);
                    baglanti.SaveChanges();
                    return RedirectToAction("Index");
                }
                else if (chc == "false")
                {
                    ViewBag.Error = "Kayıt Olmak İçin Sözleşmeyi Onaylamanız Gerekmektedir.";
                    return View();
                }
                else
                {
                    ViewBag.Error = "Şifreler Uyuşmuyor.";
                    return View();
                }
            }
            else
            {
                ViewBag.Error = "Mail Adresi Başka Bir Kullanıcı Tarafından Kullanılıyor.";
                return View();
            }



        }

        [HttpGet]
        public ActionResult RegisterDoctor()
        {

            List<SelectListItem> selected = (from i in baglanti.Cities.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = i.Name,
                                                 Value = i.Id.ToString()
                                             }).ToList();

            List<SelectListItem> selectedSpec = (from i in baglanti.Specializations.ToList()
                                                 select new SelectListItem
                                                 {
                                                     Text = i.Name,
                                                     Value = i.Id.ToString()
                                                 }).ToList();

            ViewBag.CityNames = selected;
            ViewBag.SpecNames = selectedSpec;
            return View();


        }

        [HttpPost]
        public ActionResult RegisterDoctor(Models.Doctors doctor)
        {



            if (baglanti.Doctors.Any(x => x.Mail == doctor.Mail))
            {
                TempData["DoctorRegisterError"] = "Mail Adresi Başka Bir Kullanıcı Tarafından Kullanılıyor.";
                return RedirectToAction("RegisterDoctor");


            }
            else
            {
                doctor.Biography = "-";
                doctor.Password = Classes.Password.Encrypt(doctor.Password);
                doctor.Viewed = 0;
                doctor.Patiens = 0;
                doctor.ProfileImage = "~/Image/anony";
                doctor.AverageVote = 0;
                doctor.TotalVote = 0;
                doctor.CanBlog = false;

                baglanti.Doctors.Add(doctor);



                baglanti.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult Login()
        {


            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.Members member)
        {



            Models.Members homemember = baglanti.Members.Where(x => x.Mail == member.Mail).FirstOrDefault();
            if (homemember != null)
            {
                homemember.Password = Classes.Password.Discrypt(homemember.Password);
            }


            if (homemember != null && homemember.Password == member.Password)
            {
                Session["LogonUser"] = homemember;

                return RedirectToAction("Index");
            }
            else if (homemember != null && homemember.Password != member.Password)
            {
                ViewBag.LoginError = "Hatalı Şifre Girdiniz.";
                return View();

            }
            else
            {
                ViewBag.LoginError = "Hatalı Mail Adresi Girdiniz.";
                return View();
            }



        }

        [HttpGet]
        public ActionResult LoginDoctor()
        {

            return View();
        }

        [HttpPost]
        public ActionResult LoginDoctor(Models.Doctors doctor)
        {

            Models.Doctors homedoctor = baglanti.Doctors.Where(x => x.Mail == doctor.Mail).FirstOrDefault();
            if (homedoctor != null)
            {
                homedoctor.Password = Classes.Password.Discrypt(homedoctor.Password);

            }


            if (homedoctor != null && homedoctor.Password == doctor.Password)
            {
                Session["LogonDoctor"] = homedoctor;
                return RedirectToAction("Index", "DoctorPanel");
            }
            else if (homedoctor != null && homedoctor.Password != doctor.Password)
            {
                ViewBag.LoginError = "Hatalı Şifre Girdiniz.";
                return View();

            }
            else
            {
                ViewBag.LoginError = "Hatalı Mail Adresi Girdiniz.";
                return View();
            }

        }

        public ActionResult LogOut()
        {
            Session["LogonUser"] = null;
            return RedirectToAction("Index");
        }

        public ActionResult DoctorLogOut()
        {
            Session["LogonDoctor"] = null;

            return RedirectToAction("Index", "Home");
        }
        public ActionResult UserFavorites()
        {

            FindDoctor.Models.Members user = (FindDoctor.Models.Members)Session["LogonUser"];
            var doctor = baglanti.Favorites.Where(x => x.MemberId == user.Id).ToList();

            return View(doctor);
        }


    }



}