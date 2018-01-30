using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Card
    {
        public ObjectId Id { get; set; }
        private string cardNr;

        public string CardNr
        {
            get { return cardNr; }
            set { cardNr = value; }
        }
        private string cardHaolder;

        public string CardHolder
        {
            get { return cardHaolder; }
            set { cardHaolder = value; }
        }

        private string cvv;

        public string Cvv
        {
            get { return cvv; }
            set { cvv = value; }
        }

        private string expiry;
        public string Expiry
        {
            get { return expiry; }
            set { expiry = value; }
        }
        private string userID;

        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }












    }
}
