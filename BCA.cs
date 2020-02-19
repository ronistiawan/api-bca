using System;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using RestSharp;
using System.Web;

namespace ConsoleApp {

    public class BCA {
        private string _Origin { get; set; }
        private string _Host { get; set; }
        private string _RelativeUrl_requestToken { get; set; }
        private string _RelativeUrl_checkBalance { get; set; }
        private string _RelativeUrl_fundTransfer { get; set; }
        private string _RelativeUrl_fundCollection { get; set; }
        private string _ClientId { get; set; }
        private string _ClientSecret { get; set; }
        private string _ApiKey { get; set; }
        private string _ApiSecret { get; set; }
        private string _AccessToken { get; set; }
        private DateTime _TokenExpirationTime { get; set; }

        public BCA(){
            this._Origin = "https://invoila.co.id";
            this._Host = "https://sandbox.bca.co.id";
            this._RelativeUrl_requestToken = "/api/oauth/token";
            this._RelativeUrl_checkBalance = "/banking/v3/corporates/BCAAPI2016/accounts";
            this._RelativeUrl_fundTransfer = "/banking/corporates/transfers";
            this._RelativeUrl_fundCollection = "/fund-collection";
            this._ClientId = "13d2beef-5f84-4f31-ad61-e45e822f8ed0";
            this._ClientSecret = "db444b9b-f35b-4468-82df-5866eb2ccbdc";
            this._ApiKey = "193b152c-41ef-4d47-adb6-e23735f3b4be";
            this._ApiSecret = "96e9c3f3-f1b4-45c4-8525-9a26f7c25cc8";
        }

        private RestRequest _CreateRequestHeader(Method method, string timestamp, string signature){

            var request = new RestRequest(method);
            request.AddHeader("Authorization", $"Bearer {this._AccessToken}");
            request.AddHeader("Origin", this._Origin);
            request.AddHeader("X-BCA-Key", this._ApiKey);
            request.AddHeader("X-BCA-Timestamp", timestamp);
            request.AddHeader("X-BCA-Signature", signature);
            
            request.AddHeader("Content-Type", "application/json");

            return request;
        }

        public FundTransferResponse FundTransfer(){
            var token = _GetToken();
            if(token == null) return null;

            var timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz");

            var relativeUrl = $"{this._RelativeUrl_fundTransfer}";

            var client = new RestClient($"{this._Host}{relativeUrl}");

            var transferRequest = new FundTransferRequest{
                CorporateID = "BCAAPI2016",
                SourceAccountNumber = "0201245680",
                TransactionID = "00000001",
                TransactionDate = "2020-02-18",
                ReferenceID = "12345/PO/2016",
                CurrencyCode = "IDR",
                Amount = "100000.00",
                BeneficiaryAccountNumber = "0201245681",
                Remark1 = "Transfer Test",
                Remark2 = "Online Transfer"
            };

            
            var signature = _GenerateRequestSignature("POST", relativeUrl, timestamp, JsonConvert.SerializeObject(transferRequest));
            var request = _CreateRequestHeader(Method.POST, timestamp, signature);

            request.AddJsonBody(transferRequest);

            try {
                IRestResponse response = client.Execute(request);
                var transferResponse = JsonConvert.DeserializeObject<FundTransferResponse>(response?.Content);

                return transferResponse;
            }
            catch(Exception e){
                Console.WriteLine(e);
            }

            return null;
        }

        public FundCollectionResponse FundCollection(){
            var token = _GetToken();
            if(token == null) return null;

            var timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz");

            var relativeUrl = $"{this._RelativeUrl_fundTransfer}";

            var client = new RestClient($"{this._Host}{relativeUrl}");

            var collectionRequest = new FundCollectionRequest{
                TransactionID = "0000000001",
                ReferenceNumber = "1111111110",
                RequestType = "B",
                DebitedAccount = "1234567890",
                Amount= "100200300.00",
                Currency= "IDR",
                CreditedAccount = "1234567891",
                EffectiveDate = "2020-02-19",
                TransactionDate = "2020-02-19",
                Remark1 = "Testing block 1",
                Remark2 = "Testing block 2",
                Email = "beneficiary@mail.com",
            };
         
            var signature = _GenerateRequestSignature("POST", relativeUrl, timestamp, JsonConvert.SerializeObject(collectionRequest));

            var request = _CreateRequestHeader(Method.POST, timestamp, signature);
            request.AddJsonBody(collectionRequest);

            try {
                IRestResponse response = client.Execute(request);
                var collectionResponse = JsonConvert.DeserializeObject<FundCollectionResponse>(response?.Content);

                return collectionResponse;
            }
            catch(Exception e){
                Console.WriteLine(e);
            }

            return null;
        }
        
        private BalanceInformationResponse _CheckBalance(string relativeUrlEncoded){
            var token = _GetToken();
            if(token == null) return null;

            var timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz");

            var client = new RestClient($"{this._Host}{relativeUrlEncoded}");

            var signature = _GenerateRequestSignature("GET", relativeUrlEncoded, timestamp);

            var request = _CreateRequestHeader(Method.GET, timestamp, signature);

            try {
                IRestResponse response = client.Execute(request);

                var balanceInfo = JsonConvert.DeserializeObject<BalanceInformationResponse>(response?.Content);

                return balanceInfo;
            }
            catch(Exception e){
                Console.WriteLine(e);
            }

            return null;
        }

        public BalanceInformationResponse CheckBalance(string[] accounts){

            var relativeUrl = $"{this._RelativeUrl_checkBalance}/{Uri.EscapeDataString(String.Join(',', accounts))}";
            return _CheckBalance(relativeUrl);
        }

        public BalanceInformationResponse CheckBalance(string account){
            
            var relativeUrlEncoded = $"{this._RelativeUrl_checkBalance}/{account.Replace(" ","")}";
            return _CheckBalance(relativeUrlEncoded);
        }
        
        private string _GetToken(){
            _SetTls();

            if(_TokenExpirationTime > DateTime.Now.AddMinutes(5)){
                return this._AccessToken;
            }

            var oauthCredentials = $"{this._ClientId}:{this._ClientSecret}";

            var client = new RestClient($"{this._Host}{this._RelativeUrl_requestToken}");
            var request = new RestRequest(Method.POST);

            request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(oauthCredentials)));
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            request.AddParameter("grant_type", "client_credentials");

            RequestTokenResponse requestTokenResponse = null;
            try
            {
                var response = client.Execute(request);
                requestTokenResponse = JsonConvert.DeserializeObject<RequestTokenResponse>(response?.Content);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            this._AccessToken = requestTokenResponse?.access_token;
            this._TokenExpirationTime = DateTime.Now.AddSeconds(requestTokenResponse.expires_in);

            Console.WriteLine("Access Token: "+this._AccessToken);
            return this._AccessToken;
        }

        private string _GenerateRequestSignature(string httpMethod, string relativeUrl, string timestamp, string reqBody = ""){

            HMACSHA256 hmac = new HMACSHA256(Encoding.ASCII.GetBytes(this._ApiSecret));

            reqBody = reqBody.Replace(" ","");
            var HexEncode = _Sha256(reqBody);
            
            var beforeHashed = Encoding.ASCII.GetBytes($"{httpMethod}:{relativeUrl}:{this._AccessToken}:{HexEncode}:{timestamp}");
            
            var hashed = hmac.ComputeHash(beforeHashed);
            
            var CalculatedHMAC = BitConverter.ToString(hashed).ToLower().Replace("-","");

            Console.WriteLine("API Secret: " + this._ApiSecret);
            Console.WriteLine("Relative URL: " + relativeUrl);
            Console.WriteLine("Timestamp: " + timestamp);
            Console.WriteLine("Req Body: " + reqBody);
            Console.WriteLine("Signature: " + CalculatedHMAC);
            Console.WriteLine("Body: " + reqBody);
            return CalculatedHMAC;
        }

        private void _SetTls()
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private string _Sha256(string randomString)
        {
            byte[] crypto = new SHA256Managed().ComputeHash(Encoding.ASCII.GetBytes(randomString));
            return BitConverter.ToString(crypto).ToLower().Replace("-", "");
        }
    }
}
