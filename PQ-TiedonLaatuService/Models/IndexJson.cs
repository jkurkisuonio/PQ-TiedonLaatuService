using System;
using System.Collections.Generic;
using System.Text;

namespace PQ_TiedonLaatuService.Models
{
    public class IndexJson
    {
        public string LoginResult { get; set; }
        public string SessionID { get; set; }
        public string ApiVersion { get; set; }
        public List<string> Workers { get; set; }
    }
}
