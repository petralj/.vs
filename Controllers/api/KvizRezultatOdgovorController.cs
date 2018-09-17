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
    [RoutePrefix("api/KvizRezultatOdgovor")]
    public class KvizRezultatOdgovorController : ApiController
    {
        KvizZnanjaEntities db = new KvizZnanjaEntities();
        
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Insert([FromBody]List<KvizZnanja.Models.KvizRezultatOdgovor> data)
        {
            if (ModelState.IsValid)
            {
                db.KvizRezultatOdgovor.AddRange(data);
                db.SaveChanges();
                return Ok(data);
            }
            return Ok(-1);
        }
    }
}