using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
   public class Beneficiary
    {
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
        private int myVar;

        public int MyProperty
        {
            get { return myVar; }
            set { myVar = value; }
        }




    }
}
