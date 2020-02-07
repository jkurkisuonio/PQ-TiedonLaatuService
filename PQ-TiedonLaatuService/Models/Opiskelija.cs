using System;
using System.Collections.Generic;
using System.Text;

namespace PQ_TiedonLaatuService.Models
{
    public class Opiskelija
    {
        public string korttinumero { get; set; }
        public string sukunimi { get; set; }
        public string etunimi { get; set; }
        public Vastuukouluttaja vastuukouluttaja { get; set; }
    }
}
