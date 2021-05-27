using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Pagamentos.AntiCorruption
{
    public interface IPayPalGateway
    {
        string GetPayPalServiceKey(string apiKey, string encriptionKey);
        string GetCardHashKey(string serviceKey, string cartaoCredito);
        bool CommitTransaction(string cardHashKey, string orderId, decimal amount);
    }
}
