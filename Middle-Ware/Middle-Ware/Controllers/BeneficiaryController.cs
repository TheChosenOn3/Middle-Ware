using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Middle_Ware.Controllers
{
    public class BeneficiaryController : ApiController
    {
        // GET api/<controller>
        public Beneficiary Get()
        {
            Beneficiary ben = new Beneficiary { BeneficairyName="piele",BeneficairyID
            ="2001"};
            return ben;
        }

        // GET api/<controller>/5
        [Route("api/Beneficiary/{user}/{pass}")]
        [HttpGet]
        public string Get(string user, string pass)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}