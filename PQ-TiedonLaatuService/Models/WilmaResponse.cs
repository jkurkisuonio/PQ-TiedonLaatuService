using System;
using System.Collections.Generic;
using System.Text;

namespace PQ_TiedonLaatuService.Models
{
    public class WilmaResponse
    {
        public string LoginResult { get; set; }
        public string WilmaId { get; set; }
        public int ApiVersion { get; set; }
        public string FormKey { get; set; }
        public List<object> ConnectIds { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int PrimusId { get; set; }
        public string School { get; set; }
        public List<object> Exams { get; set; }
        public List<News> News { get; set; }
        public List<object> Groups { get; set; }
        public string Photo { get; set; }
        public bool EarlyEduUser { get; set; }
        public List<object> Roles { get; set; }
    }
}
