using KvizZnanja.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvizZnanja.Controllers
{
    public class RezultatiController : Controller
    {
        KvizZnanjaEntities db = new KvizZnanjaEntities();

        // metoda koja dohvaća podatke iz baze o svim rezultatima odigranih kvizova
        private List<KvizRezultatVM> Get()
        {
            List<KvizRezultatVM> rez = new List<KvizRezultatVM>();
            List<KvizRezultat> kvizRezultat = db.KvizRezultat.ToList();
            foreach (var kr in kvizRezultat)
            {
                KvizRezultatVM o = new KvizRezultatVM();
                o.KvizRezultat = kr;
                o.User = db.Users.Where(u => u.ID == kr.UserID).FirstOrDefault();
                List<KvizRezultatOdgovor> odgovori = db.KvizRezultatOdgovor.Where(odg => odg.KvizRezultatID == kr.ID).ToList();
                foreach (var odg in odgovori)
                {
                    o.Odgovori.Add(new KvizRezultatOdgovorVM()
                    {
                        ID = odg.ID,
                        KvizRezultatID = odg.KvizRezultatID.Value,
                        Pitanje = db.Pitanjes.Where(p => p.ID == odg.PitanjeID).FirstOrDefault(),
                        Odgovor = db.Odgovors.Where(odgovor => odgovor.ID == odg.UserOdgovorID).FirstOrDefault(),
                        TocanOdgovor = db.Odgovors.Where(odgovor => odgovor.ID == odg.TocanOdgovorID).FirstOrDefault()
                    });
                }
                rez.Add(o);
            }
            return rez;
        }

        // metoda koja dohvaća podatke iz baze o željenom rezultatu odigranog kviza
        private KvizRezultatVM Get(long ID)
        {
            KvizRezultat kvizRezultat = db.KvizRezultat.Where(kr => kr.ID == ID).FirstOrDefault();
            KvizRezultatVM rez = new KvizRezultatVM();
            rez.KvizRezultat = kvizRezultat;
            rez.User = db.Users.Where(u => u.ID == kvizRezultat.UserID).FirstOrDefault();
            List<KvizRezultatOdgovor> odgovori = db.KvizRezultatOdgovor.Where(odg => odg.KvizRezultatID == rez.KvizRezultat.ID).ToList();
            foreach (var odg in odgovori)
            {
                rez.Odgovori.Add(new KvizRezultatOdgovorVM()
                {
                    ID = odg.ID,
                    KvizRezultatID = odg.KvizRezultatID.Value,
                    Pitanje = db.Pitanjes.Where(p => p.ID == odg.PitanjeID).FirstOrDefault(),
                    Odgovor = db.Odgovors.Where(odgovor => odgovor.ID == odg.UserOdgovorID).FirstOrDefault(),
                    TocanOdgovor = db.Odgovors.Where(odgovor => odgovor.ID == odg.TocanOdgovorID).FirstOrDefault()
                });
            }
            return rez;
        }

        // metoda ucitava pregled svih rezultata
        public ActionResult Index()
        {
            ViewBag.rezultati = this.Get();
            return View();
        }

        // metoda ucitava detaljan pregled rezultata
        public ActionResult Details(int ID)
        {
            ViewBag.rezultat = this.Get(ID);
            
            return View();
        }

        // metoda brise iz baze podatke o rezultatima odigranog kviza
        public ActionResult Delete(long ID) {
            if(ID > 0)
            {
                List<KvizRezultatOdgovor> toRemove = db.KvizRezultatOdgovor.Where(kro => kro.KvizRezultatID == ID).ToList();
                KvizRezultat rez = db.KvizRezultat.Where(kr => kr.ID == ID).FirstOrDefault();

                db.KvizRezultatOdgovor.RemoveRange(toRemove);
                db.KvizRezultat.Remove(rez);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}