using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Store.Pagamentos.AntiCorruption
{
    // Simula o GetEnvironment
    public class ConfigurationManager : IConfigurationManager
    {
        public string GetValue(string node)
        {
            return Util.GenerateRandomValue();
        }
    }

    public static class Util
    {
        public static string GenerateRandomValue()
        {
            return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }

        public static int GenerateRandomNumber(int maxValue)
        {
            return new Random().Next(maxValue);
        }
    }
}
