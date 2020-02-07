using System;
using System.Collections.Generic;
using System.Text;

namespace PQ_TiedonLaatuService.Models
{
    public class WilmaMsg
    {
        public string FormKey { get; set; }
        public string bodytext { get; set; }
        public string Subject { get; set; }
        public bool ShowRecipients { get; set; }
        public bool CollatedReplies { get; set; }
        public string r_teacher { get; set; }
        public string r_student { get; set; }
        public string r_guardian { get; set; }
        public string r_personnel { get; set; }
    }
}
