using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class PayTraceRequest
    {
        public double amount { get; set; }

        public string currency { get; set; }

        public string number { get; set; }

        public string expiration_month { get; set; }

        public string expiration_year { get; set; }
        public string holder { get; set; }

        public string csc { get; set; }
    }
}
