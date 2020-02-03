using System;
using System.Collections.Generic;
using System.Text;

namespace TestTaskDatesCommon.Payloads
{
    public class LoginResultPayload
    {
        public string access_token { get; set; }
        public string username { get; set; }
        public string errorText { get; set; }
    }
}
