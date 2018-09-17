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
    [RoutePrefix("api/Kviz")]
    public class KvizController : ApiController
    {
        KvizZnanjaEntities db = new KvizZnanjaEntities();

        [HttpGet]
        [Route("Get")]
        public IHttpActionResult Get()
        {
            return Ok(db.Kvizs.ToList());
        }


        [HttpPost]
        [Route("GetAllData")]
        public IHttpActionResult GetAllData([FromBody]long ID)
        {
            ZapocetiKvizData kvizData = new ZapocetiKvizData();
            Kviz kviz = db.Kvizs.Where(k => k.ID == ID).FirstOrDefault();
            kvizData.Kviz = kviz;

            foreach (var kvizPitanje in db.KvizPitanjes.Where(kp => kp.KvizID == kviz.ID).ToList())
            {
                foreach (var pitanje in db.Pitanjes.Where(p => p.ID == kvizPitanje.PitanjeID).ToList())
                {
                    PitanjaSaOdgovorima ptnj = new PitanjaSaOdgovorima();
                    ptnj.Pitanje = pitanje;
                    ptnj.Odgovori = db.Odgovors.Where(o => db.OdgovorPitanjes.Where(op => op.PitanjeID == pitanje.ID).Select(op => op.OdgovorID).ToList().Contains(o.ID)).ToList();
                    kvizData.Data.Add(ptnj);
                }
            }
            return Ok(kvizData);
        }
    }
}
