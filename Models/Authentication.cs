using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KvizZnanja.Models
{
    public class Authentication : IDisposable
    {
        public static void Logout()
        {
            HttpContext.Current.Session["loggedUser"] = null;
        }

        public static string GetImeIPrezime()
        {
            return getLoggedUser().Ime + " " + getLoggedUser().Prezime;
        }

        public static User getLoggedUser()
        {
            return (User)HttpContext.Current.Session["loggedUser"];
        }

        public void setLoggedUser(User user)
        {
            HttpContext.Current.Session["loggedUser"] = user;
        }

        public static bool IsLogged()
        {
            return HttpContext.Current.Session["loggedUser"] != null ? true : false;
        }

        public static bool IsAdmin()
        {
            User user = (User)HttpContext.Current.Session["loggedUser"];
            return user.Privilegije == "admin" ? true : false;
        }

        public static bool IsKorisnik()
        {
            User user = (User)HttpContext.Current.Session["loggedUser"];
            return user.Privilegije == "korisnik" ? true : false;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}