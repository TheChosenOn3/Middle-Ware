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
    public class HistoryController : ApiController
    {

        [HttpGet]
        [Route("api/History/{userID}/")]
        public IEnumerable<History> Get(string userID)
        {
            History pay = new History { UserID = userID };
            List<History> payList = new List<History>();
            Dictionary<Expression<Func<History, object>>, Func<History, object>> Filters = new Dictionary<Expression<Func<History, object>>, Func<History, object>>();
            Filters.Add(c => c.UserID, c => c.UserID);
            payList = DatabaseHandler<History>.getDocumentContent(pay, Filters);

            return payList;
        }
    }
}
