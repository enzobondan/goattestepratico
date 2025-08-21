using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace financeControl.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; } = string.Empty;
        public int ExpireHours { get; set; } = 24;
    }
}