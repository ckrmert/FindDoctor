using FindDoctor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FindDoctor.Controllers
{
    public class sadController : Controller
    {
        // GET: sad
        public ActionResult Index()
        {
            ExDatabaseEntities baglanti = new ExDatabaseEntities();
            return View(baglanti.Cities);
        }

        [HttpPost]
        public ActionResult Index(string searchTerm)
        {

            ExDatabaseEntities baglanti = new ExDatabaseEntities();
            List<Cities> cities;
            if (string.IsNullOrEmpty(searchTerm))
            {
                cities = baglanti.Cities.ToList();

            }
            else
            {
                cities = baglanti.Cities.Where(x => x.Name.StartsWith(searchTerm)).ToList();
            }

            
            return View(cities);
        }

        public JsonResult GetStudents(string term)
        {
            ExDatabaseEntities baglanti = new ExDatabaseEntities();
            List<string> cities;
           
           
                cities = baglanti.Cities.Where(x => x.Name.StartsWith(term))
                .Select(y=>y.Name).ToList();




            return Json(cities, JsonRequestBehavior.AllowGet);
           
        }

    }
}