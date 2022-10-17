using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WashALoadService.Models
{
    public class SystemUser
    {
		public string user_id { get; set; }
		public string user_name { get; set; }
		public int admin { get; set; }
		public Section section { get; set; }
		public int active_user { get; set; }
		public string user_password { get; set; }		
		public string menu_template { get; set; }
		public string session_authentication_key { get; set; }
		public Site oSite { get; set; }
	}
}
