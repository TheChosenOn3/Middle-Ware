using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class Beneficiary
    {
        private string beneficiaryID;

        public string BeneficiaryID
        {
            get { return beneficiaryID; }
            set { beneficiaryID = value; }
        }
        private string beneficiaryName;

        public string BeneficiaryName
        {
            get { return beneficiaryName; }
            set { beneficiaryName = value; }
        }
        private string clientReference;

        public string ClientReference
        {
            get { return clientReference; }
            set { clientReference = value; }
        }

        private string beneficiaryBranch;

        public string BeneficiaryBranch
        {
            get { return beneficiaryBranch; }
            set { beneficiaryBranch = value; }
        }




    }
}
