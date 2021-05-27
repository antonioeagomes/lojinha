using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Pagamentos.AntiCorruption
{
    public class PayPalGateway : IPayPalGateway
    {
        public bool CommitTransaction(string cardHashKey, string orderId, decimal amount)
        {
            return Util.GenerateRandomNumber(2) == 1;
        }

        public string GetCardHashKey(string apiKey, string cartaoCredito)
        {
            return Util.GenerateRandomValue();
        }

        public string GetPayPalServiceKey(string apiKey, string encriptionKey)
        {
            return Util.GenerateRandomValue();
        }
    }
}
