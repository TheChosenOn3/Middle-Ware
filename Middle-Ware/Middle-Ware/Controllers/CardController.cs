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
    public class CardController : ApiController
    {
        [Route("api/Card/{userID}/")]
        [HttpGet]
        public IEnumerable<Card> Get(string userID )//gets all the accounts and its details for a specific User or beneficiary
        {

            Card pa = new Card { UserID = userID };
                Dictionary<Expression<Func<Card, object>>, Func<Card, object>> Filters = new Dictionary<Expression<Func<Card, object>>, Func<Card, object>>();
                Filters.Add(c => c.UserID, c => c.UserID);
                List<Card> us = DatabaseHandler<Card>.getDocumentContent(pa, Filters);
                return us;
            

        }

    }
}
