using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace PQ_TiedonLaatuService.Models
{
    [JsonObject("application")]
    public class Application
    {
        [JsonProperty("Path2PQExe")]
        public string Path2PQExe { get; set; }

        [JsonProperty("Path2PQConfiguration")]
        public string Path2PQConfiguration { get; set; }
        [JsonProperty("Path2WorkDir")]
        public string Path2WorkDir { get; set; }        
        [JsonProperty("Path2PQResultDir")]
        public string Path2PQResultDir { get; set; }
        [JsonProperty("wilmaUrl")]
        public string wilmaUrl { get; set; }
        [JsonProperty("wilmaPasswd")]
        public string wilmaPasswd { get; set; }
        [JsonProperty("wilmaUsername")]
        public string wilmaUsername { get; set; }
        [JsonProperty("wilmaCompanySpesificKey")]
        public string wilmaCompanySpesificKey { get; set; }

    }
}
