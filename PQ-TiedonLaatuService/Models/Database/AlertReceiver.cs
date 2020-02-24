using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PQ_TiedonLaatuService.Models.Database
{
    public class AlertReceiver
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AlertReceiverId { get; set; }
        public string Email { get; set; }
        public string CardNumber { get; set; }
        public ICollection<PrimusAlert> PrimusAlerts { get; set; }
    }
}
