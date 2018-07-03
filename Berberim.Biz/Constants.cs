using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berberim.Biz
{
    public static class Constants
    {
        public struct StatuID
        {
            public const int Admin = 1;
            public const int Barber = 2;
            public const int Customer = 3;
        }
        public struct RecordStatu
        {
            public const int Deleted = 1;
            public const int Active = 2;
            public const int Passive = 3;
        }
        public static string ContactMail => ConfigurationManager.AppSettings["ContactMail"];

        public struct SendMail
        {
            public const int Error = 1;
            public const int Succes = 2;
        }
    }
}
