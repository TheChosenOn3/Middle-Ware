using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Crypto
    {
        public ObjectId Id { get; set; }
        private string waletName;

        public string WaletName
        {
            get { return waletName; }
            set { waletName = value; }
        }
        private string waletaddress;

        public string Waletaddress
        {
            get { return waletaddress; }
            set { waletaddress = value; }
        }
        private string userID;

        public string UserId
        {
            get { return userID; }
            set { userID = value; }
        }
        private string beneficiaryID;

        public string BeneficiaryId
        {
            get { return beneficiaryID; }
            set { beneficiaryID = value; }
        }
        private float amount;

        public float Amount
        {
            get { return amount; }
            set { amount = value; }
        }



    }
}
