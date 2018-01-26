using ConnectionHandler;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace Middle_Ware.Controllers
{
    public class PaymentController : ApiController
    {
       
        [HttpGet]
        public IEnumerable<Payment> Get(string user)
        {
            Payment pay = new Payment{UserID=user};
            List<Payment> payList = new List<Payment>();
            Dictionary<Expression<Func<Payment, object>>, Func<Payment, object>> Filters = new Dictionary<Expression<Func<Payment, object>>, Func<Payment, object>>();
            Filters.Add(c => c.UserID, c => c.UserID);
            payList= DatabaseHandler<Payment>.getDocumentContent(pay, Filters);
            if (payList.Count == 1)
            {
                return payList;

            }
            return payList;
        }

        public HttpResponseMessage Post([FromBody]Payment pay)
        {
            try
            {
                DatabaseHandler<Payment>.insertData(pay);
            }
            catch (Exception)
            {

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            return new HttpResponseMessage(HttpStatusCode.OK);



        }

        private void checkPayments()
        {
            DateTime currentDate = DateTime.Now;
            string currentDateString = currentDate.ToShortDateString();
            Dictionary<Expression<Func<Payment, object>>, Func<Payment, object>> filters = new Dictionary<Expression<Func<Payment, object>>, Func<Payment, object>>();  
            List<Payment> payList = new List<Payment>();
            filters.Add(c => c.PayDate, c => c.PayDate);
            
            while (true)
            {
                payList = DatabaseHandler<Payment>.getDocumentContent(new Payment(), filters);
                Thread.Sleep(10000);


            }
            
        }
    }
}
