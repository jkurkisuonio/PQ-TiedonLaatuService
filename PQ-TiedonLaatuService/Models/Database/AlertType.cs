using System;
using System.Collections.Generic;
using System.Text;

namespace PQ_TiedonLaatuService.Models.Database
{
    public class AlertType
    {
        // PrimaryKey
        public int AlertTypeId { get; set; }
        public string CardNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string QueryString { get; set; }
        public string AlertMsgText { get; set; }
        public string AlertMsgSubject { get; set; }
        public bool IsInUse { get; set; }

        public virtual ICollection<PrimusAlert> PrimusAlerts { get; set; }
    }
}
