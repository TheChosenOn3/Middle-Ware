using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Entities;
using Newtonsoft.Json;
using ConnectionHandler;

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
        public User Get(string id)
        {
            User us =(User)JsonConvert.DeserializeObject(id);
          List<User> userList =  DatabaseHandler<User>.getFilteredObj(us, c => c.Email, c => c.Email);
            if (userList.Count==1)
            {
                return userList[0];
            }
            return new User();

          
        }

        // POST: api/User
        public void Post([FromBody]string value)
        {
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
