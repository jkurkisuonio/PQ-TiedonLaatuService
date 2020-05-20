using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PQ_TiedonLaatuService.Models.Database
{
    public class AlertType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string QueryString { get; set; }
        public string QueryName { get; set; }
        public string AlertMsgHeader { get; set; }
        public string AlertMsgText { get; set; }
        public string AlertMsgSignature { get; set; }
        public string AlertMsgSubject { get; set; }
        public bool IsInUse { get; set; }
        public bool OnlyOnce { get; set; }
        public virtual ICollection<PrimusAlert> PrimusAlerts { get; set; }
    }
}
