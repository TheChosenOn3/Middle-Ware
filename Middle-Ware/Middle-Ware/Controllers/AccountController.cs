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
    public enum userType
    {
        Client=1,Beneficiary
    }
    public class AccountController : ApiController
    {
        // GET api/<controller>.
        public IEnumerable<PaymentAccount> Get(string userID, userType type)//gets all the accounts and its details for a specific User or beneficiary
        {
            if (type==userType.Client)
            {
                PaymentAccount pa = new PaymentAccount { UserID = userID };
                Dictionary<Expression<Func<PaymentAccount, object>>, Func<PaymentAccount, object>> Filters = new Dictionary<Expression<Func<PaymentAccount, object>>, Func<PaymentAccount, object>>();
                Filters.Add(c => c.UserID, c => c.UserID);
                List<PaymentAccount> us = DatabaseHandler<PaymentAccount>.getDocumentContent(pa, Filters);
                return us;
            }
            else
            {
                PaymentAccount pa = new PaymentAccount { BeneficiaryID = userID };
                Dictionary<Expression<Func<PaymentAccount, object>>, Func<PaymentAccount, object>> Filters = new Dictionary<Expression<Func<PaymentAccount, object>>, Func<PaymentAccount, object>>();
                Filters.Add(c => c.BeneficiaryID, c => c.BeneficiaryID);
                List<PaymentAccount> us = DatabaseHandler<PaymentAccount>.getDocumentContent(pa, Filters);
                return us;
            }
            
        }

        // GET api/<controller>/5

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]PaymentAccount pay)
        {
            try
            {
                DatabaseHandler<PaymentAccount>.insertData(pay);

            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put(int id, [FromBody]PaymentAccount pay)
        {
            if (pay.BeneficiaryID != null&&pay.UserID==null)
            {
                DatabaseHandler<PaymentAccount>.UpdateDocument(pay, c => c.BeneficiaryID, c => c.BeneficiaryID);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }else if (pay.BeneficiaryID == null && pay.UserID != null)
            {
                DatabaseHandler<PaymentAccount>.UpdateDocument(pay, c => c.UserID, c => c.UserID);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}
