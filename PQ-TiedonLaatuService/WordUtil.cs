using PQ_TiedonLaatuService.Models;
using PQ_TiedonLaatuService.Models.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace PQ_TiedonLaatuService
{
    public class WordUtil
    {
        private readonly Vastuukouluttaja vastuukouluttaja;
        private readonly AlertType alertType;
        private readonly Opiskelija opiskelija;
           
        public WordUtil(Vastuukouluttaja vastuukouluttaja, AlertType alertType, Opiskelija opiskelija)
        {
            this.vastuukouluttaja = vastuukouluttaja;
            this.alertType = alertType;
            this.opiskelija = opiskelija;
        }

        public Dictionary<String,string> ReturnWords()
        {
            var words = new Dictionary<string, string>();
            words.Add("DateTimeNow", DateTime.Now.ToShortDateString());
            words.Add("ReceiverEmail", vastuukouluttaja.email);
            words.Add("AlertTypeName", alertType.Name);
            words.Add("StudentName", opiskelija.etunimi + " " + opiskelija.sukunimi);
            return words;
        }
    
    
    }
}
