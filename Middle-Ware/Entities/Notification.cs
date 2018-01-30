using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Notification
    {
        private ObjectId id;

        public ObjectId Id
        {
            get { return id; }
            set { id = value; }
        }

        private string notID;

        public string NotID
        {
            get { return notID; }
            set { notID = value; }
        }
        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        private string dateChange;

        public string DateChange
        {
            get { return dateChange; }
            set { dateChange = value; }
        }
        private string userID;

        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }

    }
}
