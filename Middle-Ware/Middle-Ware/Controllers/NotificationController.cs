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
    public class NotificationController : ApiController
    {
        [HttpGet]
        [Route("api/Notification/{userID}/")]
        public IEnumerable<Notification> Get(string userID)
        {
            Notification pay = new Notification { UserID = userID };
            List<Notification> payList = new List<Notification>();
            Dictionary<Expression<Func<Notification, object>>, Func<Notification, object>> Filters = new Dictionary<Expression<Func<Notification, object>>, Func<Notification, object>>();
            Filters.Add(c => c.UserID, c => c.UserID);
            payList = DatabaseHandler<Notification>.getDocumentContent(pay, Filters);

            return payList;
        }
    }
}
