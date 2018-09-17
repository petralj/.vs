using KvizZnanja.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvizZnanja.Controllers
{
    public class HomeController : Controller
    {
        // Metoda koja pokreće HOME stranicu
        public ActionResult Index()
        {
            // dohvati objekt ulogiranog korisnika
            User u = Authentication.getLoggedUser();

            return View();
        }
        
    }
}