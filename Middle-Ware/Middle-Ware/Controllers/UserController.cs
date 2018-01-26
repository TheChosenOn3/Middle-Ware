using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Entities;
using Newtonsoft.Json;
using ConnectionHandler;
using System.Linq.Expressions;

namespace Middle_Ware.Controllers
{
    public class UserController : ApiController
    {
        // GET: api/User
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/User/5
        [Route("api/User/{user}/{pass}")]
        [HttpGet]
        public User Get(string user, string pass)
        {
            
            User toPass = new Entities.User { Email = user, Password = pass };
            Dictionary<Expression<Func<User, object>>, Func<User, object>> Filters = new Dictionary<Expression<Func<User, object>>, Func<User, object>>();
            Filters.Add(c => c.Email, c => c.Email);
            Filters.Add(c => c.Password, c => c.Password);
            List<User> us = DatabaseHandler<User>.getDocumentContent(toPass,Filters);
            if (us.Count==1)
            {
                return us[0];

            }
            return new User();
        }

        // POST: api/User
        public HttpResponseMessage Post([FromBody]User user)
        {
            DatabaseHandler<User>.insertData(user);

            return new HttpResponseMessage(HttpStatusCode.OK);

        }

        // PUT: api/User/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/User/5
        public void Delete(int id)
        {
        }
    }
}
