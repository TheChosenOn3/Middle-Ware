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
    public class CryptoController : ApiController
    {
        [Route("api/Crypto/{userID}/{type}")]
        [HttpGet]
        public IEnumerable<Crypto> Get(string userID, userType type)//gets all the accounts and its details for a specific User or beneficiary
        {
            if (type == userType.Client)
            {
                Crypto pa = new Crypto { UserID = userID };
                Dictionary<Expression<Func<Crypto, object>>, Func<Crypto, object>> Filters = new Dictionary<Expression<Func<Crypto, object>>, Func<Crypto, object>>();
                Filters.Add(c => c.UserID, c => c.UserID);
                List<Crypto> us = DatabaseHandler<Crypto>.getDocumentContent(pa, Filters);
                return us;
            }
            else
            {
                Crypto pa = new Crypto { BeneficiaryID = userID };
                Dictionary<Expression<Func<Crypto, object>>, Func<Crypto, object>> Filters = new Dictionary<Expression<Func<Crypto, object>>, Func<Crypto, object>>();
                Filters.Add(c => c.BeneficiaryID, c => c.BeneficiaryID);
                List<Crypto> us = DatabaseHandler<Crypto>.getDocumentContent(pa, Filters);
                return us;
            }

        }
        public HttpResponseMessage Post([FromBody]Crypto pay)
        {
            try
            {
                DatabaseHandler<Crypto>.insertData(pay);

            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
