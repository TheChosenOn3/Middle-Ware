using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Crypto
    {
        private string walletName;

        public string WalletName
        {
            get { return walletName; }
            set { walletName = value; }
        }
        private string walletaddress;

        public string WalletAddress
        {
            get { return walletaddress; }
            set { walletaddress = value; }
        }
        private string userID;

        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        private string beneficiaryID;

        public string BeneficiaryID
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
