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
    [RoutePrefix("api/Pitanje")]
    public class PitanjeController : ApiController
    {
        KvizZnanjaEntities db = new KvizZnanjaEntities();

        [HttpGet]
        [Route("Get")]
        public IHttpActionResult Get()
        {
            return Ok(db.Pitanjes.ToList());
        }
    }
}