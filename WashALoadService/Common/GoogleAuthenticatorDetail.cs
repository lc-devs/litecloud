using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WashALoadService.Common
{
    public class GoogleAuthenticatorDetail
    {
        public string qrCodeUrl { get; set; }
        public string manualCode { get; set; }
    }
}
