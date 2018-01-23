using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    class Card
    {
        private string cardNr;

        public string CardNr
        {
            get { return cardNr; }
            set { cardNr = value; }
        }
        private string accountHolder;

        public string AccountHolder
        {
            get { return accountHolder; }
            set { accountHolder = value; }
        }

        private string cvv;

        public string Cvv
        {
            get { return cvv; }
            set { cvv = value; }
        }

        private DateTime expiry;
        public DateTime Expiry
        {
            get { return expiry; }
            set { expiry = value; }
        }











    }
}
