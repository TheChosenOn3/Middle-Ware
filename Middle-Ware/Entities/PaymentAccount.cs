using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
   public enum AccountTypes
    {
        Savings=1,Cheque,Credit
    }
    public class PaymentAccount
    {
        private string accountNumber;

        public string AccountNumber
        {
            get { return accountNumber; }
            set { accountNumber = value; }
        }

        private string accountHolder;

        public string AccountHolder
        {
            get { return accountHolder; }
            set { accountHolder = value; }
        }
        private string reference;

        public string Reference
        {
            get { return reference; }
            set { reference = value; }
        }
        private AccountTypes typeAcc;

        public AccountTypes TypeAcc
        {
            get { return typeAcc; }
            set { typeAcc = value; }
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








    }
}
