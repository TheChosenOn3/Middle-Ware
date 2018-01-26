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
