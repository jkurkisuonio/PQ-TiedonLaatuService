using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PQ_TiedonLaatuService.Models.Database
{
    public class PrimusAlert
    {
        // Tallennetaan hälytys tietokantaan. Hälytys tyyppi, korttinumero, pvm ja vastaanottaja
      
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }       
        public string CardNumber { get; set; }
        public DateTime SentDate { get; set; }      
     
        [ForeignKey("AlertType")]
        public int AlertTypeId { get; set; }
        public AlertType AlertType { get; set; }
        [ForeignKey("AlertReceiver")]
        public int AlertReceiverId { get; set; }
        public AlertReceiver AlertReceiver { get; set; }


    }
}
