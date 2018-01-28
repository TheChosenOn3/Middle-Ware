using ConnectionHandler;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Middle_Ware.Controllers
{
    public class BeneficiaryController : ApiController
    {
        // GET api/<controller>
        public Beneficiary Get(string benID)
        {
            Beneficiary toPass = new Entities.Beneficiary {BeneficairyID=benID };
            Dictionary<Expression<Func<Beneficiary, object>>, Func<Beneficiary, object>> Filters = new Dictionary<Expression<Func<Beneficiary, object>>, Func<Beneficiary, object>>();
            Filters.Add(c => c.BeneficairyID, c => c.BeneficairyID);
            List<Beneficiary> us = DatabaseHandler<Beneficiary>.getDocumentContent(toPass, Filters);
            if (us.Count == 1)
            {
                return us[0];

            }
            return new Beneficiary();
        }

        // GET api/<controller>/5
    
        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]Beneficiary ben)
        {
            try
            {
                DatabaseHandler<Beneficiary>.insertData(ben);
              
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            return  new HttpResponseMessage(HttpStatusCode.OK);
        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put(int id, [FromBody]Beneficiary ben)
        {
            if (ben.BeneficairyID != null)
            {
                DatabaseHandler<Beneficiary>.UpdateDocument(ben, c => c.BeneficairyID, c => c.BeneficairyID);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}