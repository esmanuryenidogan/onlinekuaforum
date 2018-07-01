using System;
using System.Collections.Generic;
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
    }
}
