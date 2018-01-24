using ConnectionHandler;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Middle_Ware.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
             
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
           
        }
        [Route("api/Values/{user}/{pass}")]
        public void Get(string user, string pass)
        {
            List<User> userList = DatabaseHandler<User>.getFilteredObj(new User(), c => c.Email, c => c.Email);
            if (userList.Count == 1)
            {
                HttpResponseMessage response = Request.CreateResponse<User>(HttpStatusCode.BadRequest, userList[0]);
                // return response;
            }
            else
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, "Error message"); //return response; 
            }


        }
        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
