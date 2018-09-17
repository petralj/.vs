using KvizZnanja.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvizZnanja.Controllers
{
    public class LogoutController : Controller
    {
        // metoda  koja brise trenutno ulogiranog korisnika, i redirekta na Login stranicu
        public ActionResult Index()
        {
            Authentication.Logout();
            return RedirectToAction("Index", "Login", new { });
        }
    }
}