using KvizZnanja.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvizZnanja.Controllers
{
    public class PitanjeController : Controller
    {
        KvizZnanjaEntities db = new KvizZnanjaEntities();

        // metoda za brisanje pitanja iz baze preko ID pitanja
        public ActionResult Delete(int ID)
        {
            // dohvati pitanje iz baze
            Pitanje pitanje = db.Pitanjes.Where(p => p.ID == ID).FirstOrDefault();
            // dohvati odgovore vezane za odabrano pitanje
            List<OdgovorPitanje> odgp = db.OdgovorPitanjes.Where(op => op.PitanjeID == ID).ToList();
            List<Odgovor> odgovori = new List<Odgovor>();
            foreach(var o in odgp)
            {
                odgovori.Add(db.Odgovors.Where(od => od.ID == o.OdgovorID).FirstOrDefault());
            }

            // brisi odgovore(za rezultate), odgovore(ponudjeni odgovori za pitanje) i pitanja iz baze
            db.Odgovors.RemoveRange(odgovori);
            db.OdgovorPitanjes.RemoveRange(odgp);
            db.Pitanjes.Remove(pitanje);

            // spremi izmjene u bazi
            db.SaveChanges();
            return RedirectToAction("Index", "Pitanje");
        }

        // metoda koja učitava pogled(view/template) za pregled svih pitanja iz baze
        public ActionResult Index()
        {
            ViewBag.pitanja = db.Pitanjes.ToList();
            return View();
        }

        // metoda koja učitava pogled(view/template) sa formom za unos pitanja
        public ActionResult Create()
        {
            return View();
        }

        // metoda koja obrađuje podatke za unos pitanja
        public ActionResult HandleCreate()
        {
            // varijable za prikaz grešaka unutar pogleda (view/template)
            ViewBag.pitanjeError = 0;
            ViewBag.odg1Error = 0;
            ViewBag.odg2Error = 0;
            ViewBag.odg3Error = 0;
            ViewBag.odg4Error = 0;

            //dohvaćanje unosa iz forme za kreiranje pitanja
            //naziv i razina pitanja (npr. razina: 1,  pitanje: "Koji je glavni grad BiH?")
            string pitanje = Request.Form["Pitanje"];
            string pitanjeRazina = Request.Form["PitanjeRazina"];

            // vrijednosti za ponuđene odgovore (npr. odg1 = Mostar, odg2 = Sarajevo, odg3 = Zenica, odg4 = Zagreb)
            string odg1 = Request.Form["odg1"];
            string odg2 = Request.Form["odg2"];
            string odg3 = Request.Form["odg3"];
            string odg4 = Request.Form["odg4"];

            //dohvati odabrani radio button koji označava koji od ponuđenih odgovora je točan
            string tocanOdg = Request.Form["tocanOdg"];
            

            // validacija unosa u formu - korisnik mora unijeti text pitanja, te 4 ponuđena odgovora
            // ako nesto od navedenog nije uneseno (vrijednost bilo kojeg od gore navedenih polja je prazan string) - prekini izvođenje ove metode i ispiši error
            // npr. ako niste unijeli naslov pitanja, ispisati će se greška "Unesite naslov pitanja!", i korisnik će moći ponovno popuniti formu
            if (!string.IsNullOrEmpty(pitanje))
            {
                if (!string.IsNullOrEmpty(odg1))
                {
                    if (!string.IsNullOrEmpty(odg2))
                    {
                        if (!string.IsNullOrEmpty(odg3))
                        {
                            if (!string.IsNullOrEmpty(odg4))
                            {
                                // Kreiraj objekt tipa "Pitanje" za unos u bazu
                                Pitanje p = new Pitanje();
                                p.Value = pitanje;
                                db.Pitanjes.Add(p);
                                p.Razina = Convert.ToInt32(pitanjeRazina);
                                db.SaveChanges();

                                // Kreiranje "Odgovor" objekta koji sadrži 1. ponuđeni odgovor na pitanje - za spremanje u bazu
                                Odgovor o1 = new Odgovor();
                                o1.Value = odg1;
                                o1.IsTocanOdgovor = Convert.ToByte(tocanOdg == "1" ? "1" : "0");//ako je označen 1. radio button, postavi IsTocanOdgovor na 1 u suprotnom na 0
                                db.Odgovors.Add(o1);
                                db.SaveChanges();


                                // Kreiranje "Odgovor" objekta koji sadrži 2. ponuđeni odgovor na pitanje - za spremanje u bazu
                                Odgovor o2 = new Odgovor();
                                o2.Value = odg2;
                                o2.IsTocanOdgovor = Convert.ToByte(tocanOdg == "2" ? "1" : "0");//ako je označen 2. radio button, postavi IsTocanOdgovor na 1 u suprotnom na 0
                                db.Odgovors.Add(o2);
                                db.SaveChanges();


                                // Kreiranje "Odgovor" objekta koji sadrži 3. ponuđeni odgovor na pitanje - za spremanje u bazu
                                Odgovor o3 = new Odgovor();
                                o3.Value = odg3;
                                o3.IsTocanOdgovor = Convert.ToByte(tocanOdg == "3" ? "1" : "0");//ako je označen 3. radio button, postavi IsTocanOdgovor na 1 u suprotnom na 0
                                db.Odgovors.Add(o3);
                                db.SaveChanges();


                                // Kreiranje "Odgovor" objekta koji sadrži 4. ponuđeni odgovor na pitanje - za spremanje u bazu
                                Odgovor o4 = new Odgovor();
                                o4.Value = odg4;
                                o4.IsTocanOdgovor = Convert.ToByte(tocanOdg == "4" ? "1" : "0"); //ako je označen 4. radio button, postavi IsTocanOdgovor na 1 u suprotnom na 0
                                db.Odgovors.Add(o4);
                                db.SaveChanges();



                                //tablica OdgovorPitanje služi za  povezivanje pitanja sa njihovim odgovorima (međutablica)
                                // sadrži strani ključ(FK) pitanja (PitanjeID) i strani ključ(FK) odgovora (OdgovorID)
                                // npr. PitanjeID: 1,  OdgovorID: 1 --> znači da odgovor iz baze gdje je ID odgovora = 1 je jedan od ponuđenih odgovora za pitanje iz baze gdje je ID pitanja = 1
                                OdgovorPitanje op1 = new OdgovorPitanje();
                                op1.PitanjeID = p.ID;
                                op1.OdgovorID = o1.ID;
                                db.OdgovorPitanjes.Add(op1);
                                db.SaveChanges();


                                // sadrži strani ključ(FK) pitanja (PitanjeID) i strani ključ(FK) odgovora
                                // npr. PitanjeID: 1,  OdgovorID: 2 --> znači da odgovor iz baze gdje je ID odgovora = 2 je jedan od ponuđenih odgovora za pitanje iz baze gdje je ID pitanja = 1
                                OdgovorPitanje op2 = new OdgovorPitanje();
                                op2.PitanjeID = p.ID;
                                op2.OdgovorID = o2.ID;
                                db.OdgovorPitanjes.Add(op2);
                                db.SaveChanges();


                                // sadrži strani ključ(FK) pitanja (PitanjeID) i strani ključ(FK) odgovora
                                // npr. PitanjeID: 1,  OdgovorID: 3 --> znači da odgovor iz baze gdje je ID odgovora = 3 je jedan od ponuđenih odgovora za pitanje iz baze gdje je ID pitanja = 1
                                OdgovorPitanje op3 = new OdgovorPitanje();
                                op3.PitanjeID = p.ID;
                                op3.OdgovorID = o3.ID;
                                db.OdgovorPitanjes.Add(op3);
                                db.SaveChanges();


                                // sadrži strani ključ(FK) pitanja (PitanjeID) i strani ključ(FK) odgovora
                                // npr. PitanjeID: 1,  OdgovorID: 4 --> znači da odgovor iz baze gdje je ID odgovora = 4 je jedan od ponuđenih odgovora za pitanje iz baze gdje je ID pitanja = 1
                                OdgovorPitanje op4 = new OdgovorPitanje();
                                op4.PitanjeID = p.ID;
                                op4.OdgovorID = o4.ID;
                                db.OdgovorPitanjes.Add(op4);
                                db.SaveChanges();

                                // znaci da objasnim sta se događa

                                // prvo je u bazu uneseno pitanje na koje će se vezati odgovori (u bazi tablica Pitanje - pogledaj tablice u bazi)
                                // (u sql serveru IDENTITY ograničenje na primarnom ključu prilikom unosa u neku od tablica automatski generira ID 
                                // te ga postavlja na ID vrijednosti zadnjeg unešenog+1 - znači svaki redak u tablici će imati različit ID)
                                // prvo smo morali unijeti pitanje, kako bismo dobili ID vrijednost pitanja pomoću koga ćemo povezivati odgovore sa pitanjem.


                                // nakon toga u bazu su unesena 4 ponuđena odgovora
                                // dakle, u tablici Odgovor, pojave se 4 nova retka, koja sadrže vrijednosti ponuđenih odgovora
                                // i njihovo ID polje u tablici je označeno kao IDENTITY - automatski se generira prilikom unosa odgovora u bazu (sve tablice u bazi imaju postavljen IDENTITY za vrijednost ID-a)

                                // sada u bazi imamo uneseno 1 pitanje, i 4 odgovora, ali oni jos nisu povezani ni na koji nacin
                                // kako bismo povezali odgovore sa pitanjem, potrebno je unijeti 4 retka u tablicu "OdgovorPitanje"
                                // tablica "OdgovorPitanje" služi kao međutablica, tj. služi za spajanje druge 2 tablice, i ona sadrži samo strane ključeve
                                // PitanjeID (FK) - strani ključ koji označava pitanje koje treba povezati sa odgovorima
                                // OdgovorID (FK) - strani ključ koji označava odgovor koji treba povezati sa pitanjem

                                // npr. ako imas u bazi u tablici OdgovorPitanje 4 retka sa slijedećim vrijednostima
                                // -------------------------------------------------------------
                                // ID        |       PitanjeID           |       OdgovorID    |
                                //--------------------------------------------------------------
                                // 1         |          1                |           1        |
                                // 2         |          1                |           2        |
                                // 3         |          1                |           3        |
                                // 4         |          1                |           4        |
                                // ------------------------------------------------------------

                                // ovo u biti znači poveži iz baze odgovore sa ID-ovima 1,2,3 i 4 na pitanje sa ID-om 1


                                // u bazi nema pitanja sa ID-om 1, nego 20002, isto tako nema pitanja sa ID-ovima 1,2,3,4, nego 10002, 10003, 10004, 10005
                                // zato što su automatski generirani ID-ovi prilikom unosa podataka u bazu
                                // ako želiš testirati u bazi, izvrši sljedeći SQL upit

                                // ovaj upit dohvača ID i text pitanja sa ID-om 20002, te ID-ove i vrijednosti odgovora povezanih sa tim pitanjem
                                /*
                                SELECT

                                    p.ID as IDPitanja, p.Value as NaslovPitanja, o.ID AS IDOdgovora, o.Value as Odgovor, o.IsTocanOdgovor as IsTocanOdgovor
                                FROM
                                    OdgovorPitanje op, Pitanje p, Odgovor o
                                WHERE

                                    op.OdgovorID = o.ID AND

                                    op.PitanjeID = p.ID AND

                                    p.ID = 20002
                                */
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                ViewBag.odg4Error = 1;
                            }
                        }
                        else
                        {
                            ViewBag.odg3Error = 1;
                        }
                    }
                    else
                    {
                        ViewBag.odg2Error = 1;
                    }
                }
                else
                {
                    ViewBag.odg1Error = 1;
                }
            }
            else
            {
                ViewBag.pitanjeError = 1;
            }
            return RedirectToAction("Create");
        }

        // metoda koja učitava pogled (view/template) sa formom za uređivanje već postojećeg pitanja
        public ActionResult Uredi(int ID)
        {
            if (Request.RequestType == "POST")
            {
                HandleUredi(ID);
            }
            // učitaj ID odabranog pitanja preko URL-a (address bar)
            ViewBag.pitanjeID = ID;
            // pronadji pitanje sa odabranim ID-om u bazi
            Pitanje pitanje = db.Pitanjes.Where(p => p.ID == ID).FirstOrDefault();
            // poveži pronadjeno pitanje na ViewBag - kako bismo mu mogli pristupiti unutar template-a (view/pogled)
            ViewBag.pitanje = pitanje;

            // dohvati sve odgovore za pitanje
            List<Odgovor> odgovori = db.Odgovors.Where(o => db.OdgovorPitanjes.Where(op => op.PitanjeID == pitanje.ID).Select(op => op.OdgovorID).ToList().Contains(o.ID)).ToList();
            // poveži ih na ViewBag za korištenje unutar template-a
            ViewBag.odgovori = odgovori;
            // učitaj template
            return View();
        }


        // metoda za  provjeru nanovo unesenih podataka za neko pitanje
        // isto kao i "HandleCreate" metoda, samo sto ce ova UPDATE-ati postojeće podatke u bazi, a ne nanovo unijeti (INSERT) kao "HandleCreate" metoda
        public ActionResult HandleUredi(int ID)
        {
            // varijable za prikaz grešaka unutar pogleda (view/template)
            ViewBag.pitanjeError = 0;
            ViewBag.odg1Error = 0;
            ViewBag.odg2Error = 0;
            ViewBag.odg3Error = 0;
            ViewBag.odg4Error = 0;

            //dohvaćanje unosa iz forme za kreiranje pitanja
            //naziv i razina pitanja (npr. razina: 1,  pitanje: "Koji je glavni grad BiH?")
            string pitanje = Request.Form["Pitanje"];
            string pitanjeRazina = Request.Form["PitanjeRazina"];

            // vrijednosti za ponuđene odgovore (npr. odg1 = Mostar, odg2 = Sarajevo, odg3 = Zenica, odg4 = Zagreb)
            string odg1 = Request.Form["odg1"];
            string odg2 = Request.Form["odg2"];
            string odg3 = Request.Form["odg3"];
            string odg4 = Request.Form["odg4"];

            //dohvati odabrani radio button koji označava koji od ponuđenih odgovora je točan
            string tocanOdg = Request.Form["tocanOdg"];


            
            // validacija unosa u formu - korisnik mora unijeti text pitanja, te 4 ponuđena odgovora
            // ako nesto od navedenog nije uneseno (vrijednost bilo kojeg od gore navedenih polja je prazan string) - prekini izvođenje ove metode i ispiši error
            // npr. ako niste unijeli naslov pitanja, ispisati će se greška "Unesite naslov pitanja!", i korisnik će moći ponovno popuniti formu
            if (!string.IsNullOrEmpty(pitanje))
            {
                if (!string.IsNullOrEmpty(odg1))
                {
                    if (!string.IsNullOrEmpty(odg2))
                    {
                        if (!string.IsNullOrEmpty(odg3))
                        {
                            if (!string.IsNullOrEmpty(odg4))
                            {
                                // Pronadji u bazi objekt pitanja kojeg treba izmjeniti
                                Pitanje pitanjeObj = db.Pitanjes.Where(p => p.ID == ID).FirstOrDefault();
                                // postavi nove vrijednosti u objekt
                                pitanjeObj.Value = pitanje;
                                pitanjeObj.Razina = Convert.ToInt32(pitanjeRazina);
                                // spremi izmjene u bazi
                                db.SaveChanges();


                                // dohvati odgovore pitanja preko ID pitanja
                                List<Odgovor> odgovori = db.Odgovors.Where(o => db.OdgovorPitanjes.Where(op => op.PitanjeID == ID).Select(op => op.OdgovorID).ToList().Contains(o.ID)).ToList();

                                int i = 1;
                                foreach (var odg in odgovori)
                                {
                                    // UPDATE-aj vrijednosti odgovora
                                    odg.Value = Request.Form["odg" + i];
                                    odg.IsTocanOdgovor = tocanOdg == i.ToString() ? Convert.ToByte(true) : Convert.ToByte(false);

                                    // spremi izmjene u bazu
                                    db.SaveChanges();
                                    i++;
                                }
                                
                                // vrati nazad na pregled pitanja
                                return RedirectToAction("Uredi", "Pitanje", new { ID = ID });
                            }
                            else
                            {
                                ViewBag.odg4Error = 1;
                            }
                        }
                        else
                        {
                            ViewBag.odg3Error = 1;
                        }
                    }
                    else
                    {
                        ViewBag.odg2Error = 1;
                    }
                }
                else
                {
                    ViewBag.odg1Error = 1;
                }
            }
            else
            {
                ViewBag.pitanjeError = 1;
            }
            return RedirectToAction("Uredi", "Pitanje", new { ID = ID });
        }


        public ActionResult Testing()
        {
            ViewBag.data = JsonConvert.SerializeObject(TempData["data"]);
            return View();
        }
    }
}