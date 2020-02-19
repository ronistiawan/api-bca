using System.Collections.Generic;

namespace ConsoleApp {

    public class RequestTokenResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string scope { get; set; }
    }

    public class BalanceInformationResponse {
        public List<BcaAccount> AccountDetailDataSuccess {get;set;}
        public List<BcaAccountFailed> AccountDetailDataFailed {get;set;}
    }

    public class BcaAccountFailed {
        public string English {get; set; }
        public string Indonesian {get; set; }
        public string AccountNumber {get; set; }
    }

    public class BcaAccount {
        public string AccountNumber {get; set; }         
        public string Currency {get; set; }         
        public string Balance {get; set; }         
        public string AvailableBalance {get; set; }            
        public string FloatAmount {get; set; }         
        public string HoldAmount {get; set; }          
        public string Plafon {get; set; }
    }

    public class FundTransferRequest {
        public string CorporateID {get; set; }
        public string SourceAccountNumber {get; set; }
        public string TransactionID { get; set; }
        public string TransactionDate {get; set; }
        public string ReferenceID {get; set; }
        public string CurrencyCode {get; set; }
        public string Amount {get; set; }
        public string BeneficiaryAccountNumber {get; set; }
        public string Remark1 {get; set; }
        public string Remark2 {get; set; }
    }
        
    public class FundTransferResponse {
        public string TransactionID {get; set; }
        public string TransactionDate {get; set; }
        public string ReferenceID {get; set; }
        public string Status {get; set; }
    }

    public class DomesticFundTransferRequest {
        public string TransactionID {get; set; }
        public string TransactionDate {get; set; }
        public string ReferenceID {get; set; }
        public string SourceAccountNumber {get; set; }
        public string BeneficiaryAccountNumber {get; set; }
        public string BeneficiaryBankCode {get; set; }
        public string BeneficiaryName {get; set; }
        public string Amount {get; set; }
        public string TransferType {get; set; }
        public string BeneficiaryCustType {get; set; }
        public string BeneficiaryCustResidence {get; set; }
        public string CurrencyCode {get; set; }
        public string Remark1 {get; set; }
        public string Remark2 {get; set; }
    }

    public class DomesticFundTransferResponse {
        public string TransactionID {get; set; }
        public string TransactionDate {get; set; }
        public string ReferenceID {get; set; }
        public string PPUNumber {get; set; }
    }

    //Collection
    public class FundCollectionRequest {
        public string TransactionID { get; set; }
        public string ReferenceNumber { get; set; }
        public string RequestType { get; set; }
        public string DebitedAccount { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
        public string CreditedAccount { get; set; }
        public string EffectiveDate { get; set; }
        public string TransactionDate { get; set; }
        public string Remark1 { get; set; }
        public string Remark2 { get; set; }
        public string Email { get; set; }
    }

    public class FundCollectionResponse {
        public string TransactionID { get; set; }
        public string ReferenceID { get; set; }
        public string DebitedAccount { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
        public string CreditedAccount { get; set; }
        public string EffectiveDate { get; set; }
        public string TransactionDate { get; set; }
        public string Status { get; set; }
    }
}
