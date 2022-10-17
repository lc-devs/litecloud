using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WashALoadService.Models
{
    public class AccessMenuTemplate
    {
        public string template_code { get; set; }
        public string description { get; set; }
        public Section section { get; set; }
    }
}
