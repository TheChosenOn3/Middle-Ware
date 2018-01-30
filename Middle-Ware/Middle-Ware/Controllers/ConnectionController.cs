using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Middle_Ware.Controllers
{
    public class ConnectionController : ApiController
    {
        // GET api/<controller>
        public bool Maintenance = false;
        public bool Get()
        {

            return Maintenance;
        }
    }
}
