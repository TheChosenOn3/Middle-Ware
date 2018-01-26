using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.IO;
using MongoDB.Bson;

namespace Entities
{
   
    public  enum PaymentType
    {
        Card=1,EFT,Crypto
    }
    public class Payment
    {

        public ObjectId Id { get; set; }
        private string scheduleNr;

        public string ScheduleNr
        {
            get { return scheduleNr; }
            set { scheduleNr = value; }
        }
        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        private string beneficiaryID;

        public string BeneficairyID
        {
            get { return beneficiaryID; }
            set { beneficiaryID = value; }
        }

        private string payDate;

        public string PayDate
        {
            get { return payDate; }
            set { payDate = value; }
        }
        private float amount;

        public float Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        private string interval;

        public string Interval
        {
            get { return interval; }
            set { interval = value; }
        }

        private string status;

        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        private string paymentNumber;

        public string PaymentNumber
        {
            get { return paymentNumber; }
            set { paymentNumber = value; }
        }

        private PaymentType typePayment;

        public PaymentType TypePayment
        {
            get { return typePayment; }
            set { typePayment = value; }
        }
        private bool recurring;

        public bool Recurring
        {
            get { return recurring; }
            set { recurring = value; }
        }
        private string dateCreated;

        public string DateCreated
        {
            get { return dateCreated; }
            set { dateCreated = value; }
        }
        private string userID;

        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }










    }
}
