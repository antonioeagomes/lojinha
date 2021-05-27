using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Pagamentos.AntiCorruption
{
    public interface IConfigurationManager
    {
        string GetValue(string node);
    }
}
