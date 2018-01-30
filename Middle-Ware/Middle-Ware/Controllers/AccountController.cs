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
        [Route("api/Account/{userID}/")]
        [HttpGet]
        public IEnumerable<PaymentAccount> Get(string userID)//gets all the accounts and its details for a specific User or beneficiary
        {
            
                PaymentAccount pa = new PaymentAccount { UserID = userID };
                Dictionary<Expression<Func<PaymentAccount, object>>, Func<PaymentAccount, object>> Filters = new Dictionary<Expression<Func<PaymentAccount, object>>, Func<PaymentAccount, object>>();
                Filters.Add(c => c.UserID, c => c.UserID);
                List<PaymentAccount> us = DatabaseHandler<PaymentAccount>.getDocumentContent(pa, Filters);
                return us;
          
        }

        [Route("api/Account/{userID}/{type}")]
        [HttpGet]
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
        public HttpResponseMessage Put( [FromBody]PaymentAccount pay)
        {
            if (pay.BeneficiaryID != null&&pay.UserID==null)
            {
                DBFilterClass<PaymentAccount> pa = new DBFilterClass<PaymentAccount> {Field=c=>c.BeneficiaryID,FieldValues= c => c.BeneficiaryID,condition=FilterCondition.equals};
                DatabaseHandler<PaymentAccount>.UpdateDocument(pay, pa);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }else if (pay.BeneficiaryID == null && pay.UserID != null)
            {
                DBFilterClass<PaymentAccount> pa = new DBFilterClass<PaymentAccount> { Field = c => c.UserID, FieldValues = c => c.UserID, condition = FilterCondition.equals };
                DatabaseHandler<PaymentAccount>.UpdateDocument(pay,pa);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}
