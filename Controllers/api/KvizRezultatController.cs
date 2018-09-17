using KvizZnanja.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace KvizZnanja.Controllers.api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/KvizRezultat")]
    public class KvizRezultatController : ApiController
    {
        KvizZnanjaEntities db = new KvizZnanjaEntities();

        [HttpGet]
        [Route("Get")]
        public IHttpActionResult Get()
        {
            List<KvizRezultatVM> rez = new List<KvizRezultatVM>();
            List<KvizRezultat> kvizRezultat = db.KvizRezultat.ToList();
            foreach(var kr in kvizRezultat)
            {
                KvizRezultatVM o = new KvizRezultatVM ();
                o.KvizRezultat = kr;
                List<KvizRezultatOdgovor> odgovori = db.KvizRezultatOdgovor.Where(odg => odg.KvizRezultatID == kr.ID).ToList();
                foreach(var odg in odgovori)
                {
                    o.Odgovori.Add(new KvizRezultatOdgovorVM() {
                        ID=odg.ID,
                        KvizRezultatID=odg.KvizRezultatID.Value,
                        Pitanje=db.Pitanjes.Where(p => p.ID == odg.PitanjeID).FirstOrDefault(),
                        Odgovor=db.Odgovors.Where(odgovor => odgovor.ID == odg.UserOdgovorID).FirstOrDefault(),
                        TocanOdgovor=db.Odgovors.Where(odgovor => odgovor.ID == odg.TocanOdgovorID).FirstOrDefault()
                    });
                }
                rez.Add(o);
            }
            return Ok(rez);
        }



        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Insert([FromBody]KvizRezultat data)
        {
            KvizRezultat kv = new KvizRezultat();
            kv.KvizID = data.KvizID;
            kv.UserID = data.UserID;
            kv.NazivKviza = data.NazivKviza;
            kv.DatumIVrijeme = data.DatumIVrijeme;
            kv.Score = data.Score;
            kv.VrijemeTrajanja = data.VrijemeTrajanja;
            kv.Tocnih = data.Tocnih;
            kv.Netocnih = data.Netocnih;
            kv.Postotak = data.Postotak;

            db.KvizRezultat.Add(kv);
            db.SaveChanges();
            return Ok(kv);
        }
    }
}