using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inventory.Common
{
    public class ServiceResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public string jsonData { get; set; }

        public ServiceResponse() { }

        public void SetValues(int _code, string _message, string _JSONdata)
        {
            code = _code;
            message = _message;
            jsonData = _JSONdata;
        }



    }


}
