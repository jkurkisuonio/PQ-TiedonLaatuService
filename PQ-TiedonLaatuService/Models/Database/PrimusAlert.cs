using System;
using System.Collections.Generic;
using System.Text;

namespace PQ_TiedonLaatuService.Models.Database
{
    public class PrimusAlert
    {
        // Tallennetaan hälytys tietokantaan. Hälytys tyyppi, korttinumero, pvm ja vastaanottaja
        public int PrimusAlertID { get; set; }       
        public string CardNumber { get; set; }
        public DateTime SentDate { get; set; }
        public string ReceiverCardNumber { get; set; }


    }
}
