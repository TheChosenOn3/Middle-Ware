using Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Middle_Ware.Controllers
{
    public class VendorController : ApiController
    {

        static HttpClient client = new HttpClient();
        static string url = "http://localhost:52904/api/";
        
        public static PayTraceResponse MakePayment(PayTraceRequest request)
        {
            string jsonString = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var traceResponse = client.PostAsync(url+"PayTrace/", content);//handle if resonse codes fail
                                                                           // PayTraceResponse response = (PayTraceResponse)JsonConvert.DeserializeObject(traceResponse.Result);//Find Out what zambians did
            if (request.remitterAcc.Length==10)
            {
                return new PayTraceResponse { response_code = 101, status_message = "Transaction was successful" };
            }
           return new PayTraceResponse {response_code=102,status_message="Transaction Failed invalid acc" };
           // return response;

        }

        public static VisaResponse MakeCardPayment(VisaPaymentRequest cardRequest)
        {
            string jsonString = JsonConvert.SerializeObject(cardRequest);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var traceResponse = client.PostAsync(url+ "Visa/", content);//handle if resonse codes fail
           // VisaResponse response = (VisaResponse)JsonConvert.DeserializeObject(traceResponse.Result.Content.ToString());//Find Out what zambians did
            if (cardRequest.cardNumber.Length==16)
            {
                return new VisaResponse { Description = "Approved", ApprovalCode = "1" };//Change this 
            }
            else
            {
                return new VisaResponse { Description = "Card Number Invalid", ApprovalCode = "2" };//Change this 
            }
                                                                                                                       //
            //return response;
        }
    }
}
