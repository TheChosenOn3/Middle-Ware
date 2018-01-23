using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class User
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string rsaID;

        public string RsaID
        {
            get { return rsaID; }
            set { rsaID = value; }
        }
        private string surname;

        public string Surname
        {
            get { return surname; }
            set { surname = value; }
        }
        private string cellNr;

        public string CellNr
        {
            get { return cellNr; }
            set { cellNr = value; }
        }
        private string address;

        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        private string accountStatus;

        public string AccountStatus
        {
            get { return accountStatus; }
            set { accountStatus = value; }
        }
        private string businessName;

        public string BusinessName
        {
            get { return businessName; }
            set { businessName = value; }
        }







    }
}
