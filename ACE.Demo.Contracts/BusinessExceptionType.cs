using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Demo.Contracts
{
    public static class BusinessExceptionType
    {
        // Account
        public const int AccountBalanceOverflow = 50001;
        public const int AccountExist = 50101;
        public const int AccountActivityNoUser = 50201;
        // Project
        public const int ProjectBalanceOverflow = 51001;
        // User
        public const int UserBalanceOverflow = 52001;
        // Investment
        public const int InvestmentNotExist = 53001;
        public const int InvestmentPaied = 53002;
    }
}
