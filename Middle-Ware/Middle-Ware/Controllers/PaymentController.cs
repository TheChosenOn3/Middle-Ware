﻿using ConnectionHandler;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace Middle_Ware.Controllers
{
    public class PaymentController : ApiController
    {
       
        [HttpGet]
        [Route("api/Payment/{userID}/")]
        public IEnumerable<Payment> Get(string userID)
        {
            Payment pay = new Payment{ UserID = userID};
            List<Payment> payList = new List<Payment>();
            Dictionary<Expression<Func<Payment, object>>, Func<Payment, object>> Filters = new Dictionary<Expression<Func<Payment, object>>, Func<Payment, object>>();
            Filters.Add(c => c.UserID, c => c.UserID);
            payList= DatabaseHandler<Payment>.getDocumentContent(pay, Filters);
           
            return payList;
        }

        public HttpResponseMessage Post([FromBody]Payment pay)
        {
            try
            {
                DatabaseHandler<Payment>.insertData(pay);
            }
            catch (Exception)
            {

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            return new HttpResponseMessage(HttpStatusCode.OK);



        }

        public static void paymentChecker()
        {
            Thread t1 = new Thread(()=>checkPayments());
            t1.Start();
        }
        private static void checkPayments()
        {
            string currentDate = DateTime.Now.ToString("dd/MM/yyyy");
            Payment pay = new Payment { PayDate = currentDate,Status="Pending" };//change status
            
            Dictionary<Expression<Func<Payment, object>>, Func<Payment, object>> filters = new Dictionary<Expression<Func<Payment, object>>, Func<Payment, object>>();  
            List<Payment> payList = new List<Payment>();
            filters.Add(c => c.PayDate, c => c.PayDate);
            filters.Add(c=>c.Status, c => c.Status);
            
            while (true)
            {
                payList = DatabaseHandler<Payment>.getDocumentContent(pay, filters);
                foreach (Payment item in payList)
                {
                    Payment processedPayment = makePayment(item);//Checks if payment is successfull or failed
                    if (processedPayment.Recurring==true&&processedPayment.Recurring)
                    {

                        
                        string[] intervalBuffer = processedPayment.Interval.Split('/');
                        string OriginalDate = processedPayment.PayDate;
                        string originalStatus = processedPayment.Status;
                        string originalDesc = processedPayment.Description;
                        processedPayment.Description = "Recurring Payment";
                        int dayInt = Convert.ToInt16(intervalBuffer[0]);
                        
                        int monthInt = Convert.ToInt16(intervalBuffer[1]);
                        DateTime payDate = DateTime.ParseExact(processedPayment.PayDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);//Convert Date format to correct 
                       payDate= payDate.AddDays(dayInt);
                        payDate = payDate.AddMonths(monthInt);//gets the new Pay date
                        processedPayment.PayDate = payDate.ToString("dd/MM/yyyy");
                        processedPayment.Status = "Pending";
                        DatabaseHandler<Payment>.UpdateDocument(processedPayment, new DBFilterClass<Payment>
                        {
                            Field = c => c.ScheduleNr,
                            FieldValues = c => c.ScheduleNr,
                            condition = FilterCondition.equals
                        });
                        processedPayment.Status = originalStatus;
                        processedPayment.PayDate = OriginalDate;
                        processedPayment.Description = originalDesc;
                       // processedPayment.Description = "";
                        //Update curent document's pay date 
                    }
                    else
                    {
                        DatabaseHandler<Payment>.DeleteRow(processedPayment,c=>c.ScheduleNr, c => c.ScheduleNr);
                        //Execute delete statement to delete * non recuring payments 
                    }
                    History historyToInsert = new History {Amount=processedPayment.Amount,PayDate = processedPayment.PayDate,BeneficairyID= processedPayment.BeneficairyID,DateCreated= processedPayment.DateCreated,Description= processedPayment.Description,Interval= processedPayment.Interval,PaymentNumber= processedPayment.PaymentNumber,Recurring= processedPayment.Recurring,ScheduleNr= processedPayment.ScheduleNr,Status= processedPayment.Status,TypePayment= processedPayment.TypePayment,UserID= processedPayment.UserID };
                    DatabaseHandler<History>.insertData(historyToInsert);

                    string desc = "Payment with to Account : " + processedPayment.BeneficiaryAccount + " " + processedPayment.Status;
                    Random rand = new Random();
                    int someNum = rand.Next(10, 50000);
                    Notification notify = new Notification { DateChange = DateTime.Now.ToString(), Description = desc, NotID = someNum.ToString(), UserID = processedPayment.UserID };
                    DatabaseHandler<Notification>.insertData(notify);
                    //make a insert statement to history table

                }
                
                Thread.Sleep(300000);//sleep timer of thread


            }
            
        }

        public HttpResponseMessage Put([FromBody]Payment ben)//Updates The scheduled Payment
        {
            if (ben.BeneficairyID != null)
            {
                DatabaseHandler<Payment>.UpdateDocument(ben, new DBFilterClass<Payment> { Field = c => c.ScheduleNr, FieldValues = c => c.ScheduleNr, condition = FilterCondition.equals });
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }


        private static Payment makePayment(Payment pay)//This is the method that communicates with the 3rd party API and that process our paymentszs
        {
            PaymentType typePay = pay.TypePayment;
                 
            switch (typePay)
            {
                case PaymentType.Card:
                    Dictionary<Expression<Func<Card, object>>, Func<Card, object>> CardFilters = new Dictionary<Expression<Func<Card, object>>, Func<Card, object>>();
                    CardFilters.Add(c => c.CardNr, c => c.CardNr);
                    List<Card> payCard = DatabaseHandler<Card>.getDocumentContent(new Card { CardNr = pay.PaymentNumber }, CardFilters);
                    string year = payCard[0].Expiry.Substring(0, 4);
                    string month = payCard[0].Expiry.Substring(5, 2);
                    VisaPaymentRequest Visarequest = new VisaPaymentRequest { cardNumber = pay.PaymentNumber, amount = pay.Amount,CVV= payCard[0].Cvv, cardExpirationMonth= month,cardExpirationYear=year };
                    VisaResponse cardResponse = VendorController.MakeCardPayment(Visarequest);
                    pay.Status = (cardResponse.ApprovalCode == "1") ?"Approved":"Declined";
                    pay.Description = cardResponse.Description;
                   
                    break;
                case PaymentType.EFT:
                    Dictionary<Expression<Func<PaymentAccount, object>>, Func<PaymentAccount, object>> Filters = new Dictionary<Expression<Func<PaymentAccount, object>>, Func<PaymentAccount, object>>();
                    Filters.Add(c => c.AccountNumber, c => c.AccountNumber);
                    List<PaymentAccount> payAcc = DatabaseHandler<PaymentAccount>.getDocumentContent(new PaymentAccount { AccountNumber = pay.PaymentNumber }, Filters);
                    
                    PayTraceRequest request = new PayTraceRequest { remitterAcc = pay.PaymentNumber, amount = pay.Amount ,remitterName=payAcc[0].AccountHolder,beneficiaryAcc=pay.BeneficiaryAccount,beneficiaryName=pay.BeneficairyID,beneficiarySortCode="569875",narration="",remitterSortCode="125469"};
                    PayTraceResponse TraceResponse = VendorController.MakePayment(request);
                    pay.Status = (TraceResponse.response_code == 101) ?"Approved":"Declinded";
                    pay.Description = TraceResponse.status_message;
                    break;
                case PaymentType.Crypto:
                    Dictionary<Expression<Func<Crypto, object>>, Func<Crypto, object>> crypfilters = new Dictionary<Expression<Func<Crypto, object>>, Func<Crypto, object>>();
                    crypfilters.Add(c => c.Waletaddress, c => c.Waletaddress);
                    List<Crypto> crypList = DatabaseHandler<Crypto>.getDocumentContent(new Crypto {Waletaddress=pay.PaymentNumber }, crypfilters);
                    //PayTraceRequest request = new PayTraceRequest { number = pay.PaymentNumber, amount = pay.Amount, holder = payAcc[0].AccountHolder };
                    //PayTraceResponse TraceResponse = VendorController.MakePayment(request);
                    //pay.Status = (TraceResponse.response_code == 101) ? "Approved" : "Declinded";
                    //pay.Description = TraceResponse.status_message;
                    ///addd code for crypto here
                    Crypto ct = crypList[0];
                    if (ct.Amount<pay.Amount)
                    {
                        pay.Description = "Failed InseficientFunds";
                        pay.Status = "Failed";
                    }
                    else
                    {
                        ct.Amount -= pay.Amount;
                    }
                    DBFilterClass<Crypto> dbF = new DBFilterClass<Crypto> { Field=c=>c.Waletaddress,FieldValues = c => c.Waletaddress ,condition=FilterCondition.equals}; 
                    DatabaseHandler<Crypto>.UpdateDocument(ct,dbF);
                    
                    break;
                default:
                    break;
            }
           
            return pay;
        }
      
        

    }
}
