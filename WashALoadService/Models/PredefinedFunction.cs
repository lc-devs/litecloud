using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WashALoadService.Models
{
    public class PredefinedFunction
    {
        public string function_code { get; set; }
        public string description { get; set; }
        public Section section { get; set; }
        public string menu_group { get; set; }
        public string page_path_filename { get; set; }
        public string css_icon { get; set; }
        public int ttlsubmenu { get; set; }
    }
}
