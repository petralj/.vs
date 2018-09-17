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
    [RoutePrefix("api/KvizPitanja")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class KvizPitanjaController : ApiController
    {
        KvizZnanjaEntities db = new KvizZnanjaEntities();

        [HttpGet]
        [Route("Get")]
        public IHttpActionResult Get()
        {
            return Ok(db.KvizPitanjes.ToList());
        }

        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Insert([FromBody]KvizPitanjaInsertData data)
        {
            Kviz kviz = db.Kvizs.Where(k => k.ID == data.KvizID).FirstOrDefault();
            if(kviz != null)
            {
                kviz.NazivKviza = data.NazivKviza;
                kviz.Razina = data.Razina;
                db.SaveChanges();
            }
            List<KvizPitanje> toRemove = db.KvizPitanjes.Where(k => k.KvizID == data.KvizID).ToList();
            db.KvizPitanjes.RemoveRange(toRemove);

            foreach(var i in data.PitanjaIDs)
            {
                KvizPitanje kp = new KvizPitanje();
                kp.KvizID = data.KvizID;
                kp.PitanjeID = i;
                db.KvizPitanjes.Add(kp);
            }
            db.SaveChanges();
            return Ok(1);
        }
    }
}