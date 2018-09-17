using KvizZnanja.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvizZnanja.Controllers
{
    public class KvizController : Controller
    {
        // objekt za rukovanje bazom podataka
        KvizZnanjaEntities db = new KvizZnanjaEntities();


        // metoda koja učitava pregled svih kvizova iz baze
        public ActionResult Index()
        {
            // dohvati podatke o kvizovima iz baze i spremi ih u ViewBag kao listu
            // da bi im mogli pristupiti iz View-a (template fajla)
            ViewBag.kvizovi = db.Kvizs.ToList();
            return View();
        }

        // metoda koja briše iz baze podatke o kvizu preko ID-a kviza
        public ActionResult Delete(int ID)
        {
            // dohvati kviz iz baze preko ID
            Kviz k = db.Kvizs.Where(kv => kv.ID == ID).FirstOrDefault();

            // ako postoji kviz sa zadanim ID
            if(k != null)
            {
                // brisi ga iz baze
                db.Kvizs.Remove(k);
                // spremi promjene u bazi
                db.SaveChanges();

                // redirekt na prikaz svih kvizova
                return RedirectToAction("Index");
            }
            // redirekt na prikaz svih kvizova
            return RedirectToAction("Index");
        }


        // metoda koja ucitava template za izmjenu podataka o kvizu preko ID kviza
        public ActionResult Edit(int ID)
        {
            // dohvati kviz iz baze preko ID i spremi ga u ViewBag
            Kviz k = db.Kvizs.Where(kv => kv.ID == ID).FirstOrDefault();
            ViewBag.kviz = k;
            ViewBag.selected = "selected";

            // dohvati iz baze sva pitanja koja su vezana za odabrani kviz
            List<long> kvizPitanjaIDs = db.KvizPitanjes.Where(kp => kp.KvizID == k.ID && kp.PitanjeID.HasValue).Select(kvp => kvp.PitanjeID.Value).ToList();
            ViewBag.pitanjaKviza = db.Pitanjes.Where(p => kvizPitanjaIDs.Contains(p.ID)).ToList();
            return View();
        }

        // metoda koja sprema unesene izmjene za kviz u bazu
        public ActionResult HandleEdit(int ID)
        {
            // vrijednost input polja za naziv kviza
            string naziv = Request.Form["NazivKviza"];

            // vrijednost input polja za razinu kviza
            string razina = Request.Form["Razina"];

            // ako naziv kviza nije prazan string (ako je nesto uneseno za naziv kviza)
            if (!string.IsNullOrEmpty(naziv))
            {
                int raz = Convert.ToInt32(razina);

                // dohvati objekt odabranog kviza preko ID iz baze
                Kviz k = db.Kvizs.Where(kv => kv.ID == ID).FirstOrDefault();
                if(k != null)
                {
                    // izmjeni podatke objekta iz baze
                    k.NazivKviza = naziv;
                    k.Razina = raz;

                    // spremi izmjene u bazi
                    db.SaveChanges();

                    // redirekt na pregled kvizova
                    return RedirectToAction("Index");
                }
            }

            // ako nije unesen naziv kviza
                // ispisi ERROR
            ViewBag.errorNaziv = "Unesite naziv kviza!";

            // ponovno ucitaj stranicu za Edit kviza
            return RedirectToAction("Edit");
        }

        // metoda koja ucitava template za kreiranje novog kviza
        public ActionResult Create()
        {
            if(Request.RequestType == "POST")
            {
                if (HandleCreate())
                {
                    return RedirectToAction("Index", "Kviz", new { });
                }
            }
            return View();
        }

        public bool HandleCreate()
        {
            ViewBag.errorNaziv = "";
            // povuci podatke iz forme
            string naziv = Request.Form["NazivKviza"];
            string r = Request.Form["Razina"];

            // ako su podatci uneseni
            if (!string.IsNullOrEmpty(naziv))
            {
                // napravi novi objekt tipa Kviz i dodijeli mu unesene vrijednosti
                Kviz k = new Kviz();
                k.NazivKviza = naziv;
                k.Razina = Convert.ToInt32(r);

                // unesi kviz u bazu
                db.Kvizs.Add(k);

                // spremi izmjene
                db.SaveChanges();
                return true;
            }

            // ispisi error
            ViewBag.errorNaziv = "Unesite naziv kviza!";
            return false;
        }

        // metoda koja sluzi za ucitavanje template za uređivanje pitanja kviza
        public ActionResult UrediPitanja(int ID)
        {
            // dohvati kviz iz baze
            Kviz k = db.Kvizs.Where(kvv => kvv.ID == ID).FirstOrDefault();
            ViewBag.kviz = k;
            ViewBag.selected = "selected";

            // dohvati trenutna pitanja kviza 
            List<long> kvizPitanjaIDs = db.KvizPitanjes.Where(kp => kp.KvizID == k.ID && kp.PitanjeID.HasValue).Select(kvp => kvp.PitanjeID.Value).ToList();
            ViewBag.pitanjaKviza = db.Pitanjes.Where(p => kvizPitanjaIDs.Contains(p.ID)).ToList();

            /****************************************/
            ViewBag.chk = "checked";
            // dohvati kviz iz baze
            Kviz kv = db.Kvizs.Where(kz => kz.ID == ID).FirstOrDefault();

            // dohvati pitanja iz baze
            List<Pitanje> pitanja = db.Pitanjes.Where(p => p.Razina == kv.Razina).ToList();

            // dohvati ID-ove pitanja koji su vezani za odabrani kviz
            List<long> selectedPitanja = db.KvizPitanjes.Where(kp => kp.KvizID == kv.ID && kp.KvizID.HasValue).Select(kvp => kvp.PitanjeID.Value).ToList();
            ViewBag.k = kv;
            ViewBag.pitanja = pitanja;
            ViewBag.selectedPitanja = selectedPitanja;
            return View();
        }


        // metoda koja sluzi za pokretanje kviza
        public ActionResult StartKviz(int ID)
        {
            // dohvati sve podatke o odabranom kvizu
            ZapocetiKvizData kvizData = new ZapocetiKvizData();
            Kviz kviz = db.Kvizs.Where(k => k.ID == ID).FirstOrDefault();
            kvizData.Kviz = kviz;

            // pretvori podatke  u ljepši format za lakše rukovanje unutar template fajla (View-a)
            foreach (var kvizPitanje in db.KvizPitanjes.Where(kp => kp.KvizID == kviz.ID).ToList())
            {
                foreach(var pitanje in db.Pitanjes.Where(p => p.ID == kvizPitanje.PitanjeID).ToList())
                {
                    PitanjaSaOdgovorima ptnj = new PitanjaSaOdgovorima();
                    ptnj.Pitanje = pitanje;
                    ptnj.Odgovori = db.Odgovors.Where(o => db.OdgovorPitanjes.Where(op => op.PitanjeID == pitanje.ID).Select(op => op.OdgovorID).ToList().Contains(o.ID)).ToList();
                    kvizData.Data.Add(ptnj);
                }
            }
            ViewBag.kvizData = kvizData;
            ViewBag.kvizDataJSON = JsonConvert.SerializeObject(kvizData);
            return View();
        }
        
    }
}