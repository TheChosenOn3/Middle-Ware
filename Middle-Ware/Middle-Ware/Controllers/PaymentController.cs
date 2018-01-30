using ConnectionHandler;
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
            Payment pay = new Payment{RsaID=userID};
            List<Payment> payList = new List<Payment>();
            Dictionary<Expression<Func<Payment, object>>, Func<Payment, object>> Filters = new Dictionary<Expression<Func<Payment, object>>, Func<Payment, object>>();
            Filters.Add(c => c.RsaID, c => c.RsaID);
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
                        processedPayment.Status = "Accepted";
                        processedPayment.PayDate = OriginalDate;
                        processedPayment.Description = "";
                        //Update curent document's pay date 
                    }
                    else
                    {
                        //Execute delete statement to delete * non recuring payments 
                    }
                    History historyToInsert = new History {Amount=processedPayment.Amount,PayDate = processedPayment.PayDate,BeneficairyID= processedPayment.BeneficiaryID,DateCreated= processedPayment.DateCreated,Description= processedPayment.Description,Interval= processedPayment.Interval,PaymentNumber= processedPayment.PaymentNumber,Recurring= processedPayment.Recurring,ScheduleNr= processedPayment.ScheduleNr,Status= processedPayment.Status,TypePayment= processedPayment.TypePayment,UserID= processedPayment.RsaID };
                    DatabaseHandler<History>.insertData(historyToInsert);
                    //make a insert statement to history table
                   
                }
                
                Thread.Sleep(30000);//sleep timer of thread


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
                    VisaPaymentRequest Visarequest = new VisaPaymentRequest { cardNumber = pay.PaymentNumber, amount = pay.Amount,CVV= payCard[0].Cvv, cardExpirationMonth= payCard[0].Expiry.ToString() };
                    VisaResponse cardResponse = VendorController.MakeCardPayment(Visarequest);
                    pay.Status = (cardResponse.ApprovalCode == "1") ?"Approved":"Declined";
                    pay.Description = cardResponse.Description;
                   
                    break;
                case PaymentType.EFT:
                    Dictionary<Expression<Func<PaymentAccount, object>>, Func<PaymentAccount, object>> Filters = new Dictionary<Expression<Func<PaymentAccount, object>>, Func<PaymentAccount, object>>();
                    Filters.Add(c => c.AccountNumber, c => c.AccountNumber);
                    List<PaymentAccount> payAcc = DatabaseHandler<PaymentAccount>.getDocumentContent(new PaymentAccount { AccountNumber = pay.PaymentNumber }, Filters);
                    PayTraceRequest request = new PayTraceRequest { number = pay.PaymentNumber, amount = pay.Amount ,holder=payAcc[0].AccountHolder};
                    PayTraceResponse TraceResponse = VendorController.MakePayment(request);
                    pay.Status = (TraceResponse.response_code == 101) ?"Approved":"Declinded";
                    pay.Description = TraceResponse.status_message;
                    break;
                case PaymentType.Crypto:
                    break;
                default:
                    break;
            }
           
            return pay;
        }
      
        

    }
}
