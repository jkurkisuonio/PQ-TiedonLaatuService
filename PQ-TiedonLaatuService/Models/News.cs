using System;
using System.Collections.Generic;
using System.Text;

namespace PQ_TiedonLaatuService.Models
{
    public class News
    {
        public int Id { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Topic { get; set; }
        public string Description { get; set; }
        public string Creator { get; set; }
        public string Created { get; set; }
    }
}
