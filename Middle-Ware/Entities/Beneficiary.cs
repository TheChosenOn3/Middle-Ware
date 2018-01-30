using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
   public class Beneficiary
    {
        public ObjectId Id { get; set; }
        private string beneficiaryID;

        public string BeneficairyID
        {
            get { return beneficiaryID; }
            set { beneficiaryID = value; }
        }
        private string beneficiaryName;

        public string BeneficairyName
        {
            get { return beneficiaryName; }
            set { beneficiaryName = value; }
        }
        private string beneficiaryBranch;

        public string BeneficiaryBranch
        {
            get { return beneficiaryBranch; }
            set { beneficiaryBranch = value; }
        }
        private string userID;

        public string UserId
        {
            get { return userID; }
            set { userID = value; }
        }





    }
}
