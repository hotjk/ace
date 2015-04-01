using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Demo.Contracts
{
    public static class BusinessStatusCode
    {
        public const int BadRequest = 400;
        public const int Unauthorized = 401;
        public const int Forbidden = 403;
        public const int NotFound = 404;
        public const int RequestTimeout = 408;
        public const int Conflict = 409;
        public const int Gone = 410;
    }
}
