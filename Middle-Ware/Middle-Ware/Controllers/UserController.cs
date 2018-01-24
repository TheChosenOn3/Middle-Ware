using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Middle_Ware.Controllers
{
    public class UserController : ApiController
    {
        public bool Get(int id)
        {
            return id == 1 ? true : false;
        }
    }
}
