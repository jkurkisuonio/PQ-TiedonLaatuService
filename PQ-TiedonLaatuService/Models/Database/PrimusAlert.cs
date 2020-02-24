using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PQ_TiedonLaatuService.Models.Database
{
    public class PrimusAlert
    {
        // Tallennetaan hälytys tietokantaan. Hälytys tyyppi, korttinumero, pvm ja vastaanottaja
      
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PrimusAlertId { get; set; }       
        public string CardNumber { get; set; }
        public DateTime SentDate { get; set; }
        public string ReceiverCardNumber { get; set; }
    //  public int AlertTypeId { get; set; }
        public AlertType AlertType { get; set; }
      //  public int AlertReceiverId { get; set; }
        public AlertReceiver AlertReceiver { get; set; }


    }
}
