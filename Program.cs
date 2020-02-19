using System;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleApp
{
    class Program
    {
        public static void Main()
        {
            var bca = new BCA();

            var accountBalances = bca.CheckBalance("0201245680");
            // var accountBalances = bca.CheckBalance(new string[]{"0201245680","0063001004","1111111111"});
            // var fundTransferResponse = bca.FundTransfer();
            // var collectionResponse = bca.FundCollection();
        }
    }
}
