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

        public HttpResponseMessage Post([FromBody]Card user)
        {
            try
            {
                DatabaseHandler<Card>.insertData(user);

            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            return new HttpResponseMessage(HttpStatusCode.OK);

        }

        public HttpResponseMessage Put([FromBody]Card user_card)
        {
            if (user_card.CardNr != null)
            {
                DatabaseHandler<Card>.UpdateDocument(user_card, new DBFilterClass<Card> { Field = c => c.CardNr, FieldValues = c => c.CardNr, condition = FilterCondition.equals });//email
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }



        }
        [HttpDelete]
        public void Delete(string id)
        {
            Card crd = new Card { CardNr=id};
            DatabaseHandler<Card>.DeleteRow(crd, c => c.CardNr, c => c.CardNr);
        }

    }
}
