using KvizZnanja.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvizZnanja.Controllers
{
    public class LoginController : Controller
    {
        KvizZnanjaEntities db = new KvizZnanjaEntities();

        // metoda koja ucitava Login template (View)
        public ActionResult Index()
        {
            return View();
        }

        // metoda koja upravlja login formom
        public ActionResult HandleLogin()
        {
            // povuci podatke iz forme (Email i password)
            string email = Request.Form["email"];
            string password = Request.Form["password"];


            // ako su podatci ispravno uneseni
            ViewBag.errors = "";
            if (!string.IsNullOrEmpty(email))
            {
                if (!string.IsNullOrEmpty(password))
                {
                    // pronadji u bazi korisnika sa unesenim Emailom i passwordom
                    User user = db.Users.Where(u => u.Email == email && u.Password == password).FirstOrDefault();

                    // ako je user pronadjen, spremi ga u Sesiju za dalje rukovanje, i ucitaj pocetnu stranicu
                    if(user != null)
                    {
                        Session["loggedUser"] = user;
                        return RedirectToAction("Index", "Kviz", new { });
                    }
                    else
                    {
                        ViewBag.errors = "Greška prilikom logiranja";
                        return RedirectToAction("Index", "Login", new { });
                    }
                }
                return RedirectToAction("Index", "Login", new { });
            }
            return RedirectToAction("Index", "Login", new { });

        }
    }
}