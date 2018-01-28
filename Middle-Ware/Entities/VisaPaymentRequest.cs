using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class VisaPaymentRequest
    {

        public double amount { get; set; }

        public string currency { get; set; }

        public string cardNumber { get; set; }

        public string cardExpirationMonth { get; set; }

        public string cardExpirationYear { get; set; }

        public string CVV { get; set; }

      
    }
}
