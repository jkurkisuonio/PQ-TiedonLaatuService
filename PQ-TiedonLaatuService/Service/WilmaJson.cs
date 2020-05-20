﻿using System;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using PQ_TiedonLaatuService.Models;
using SharedLibraries.ClientUtilities;
using Nancy.Json;

namespace PQ_TiedonLaatuService.Service
{
    public class WilmaJson
    {
        private readonly string wilmaUrl;
        private readonly string passwd;
        private readonly string username;
        private readonly string companySpesificKey;
        private string hash;
        private IndexJson values;
        private HttpWebRequest request;
        private HttpWebResponse response;
        private LoginResultJson loginResultJson;

        private static readonly HttpClient client = new HttpClient();


        public WilmaJson(string wilmaUrl, string passwd, string username, string companySpesificKey)
        {
            this.wilmaUrl = wilmaUrl;
            this.passwd = passwd;
            this.username = username;
            this.companySpesificKey = companySpesificKey;
        }

        /// <summary>
        /// Suorittaa kyselyn Wilman Json rajapintaan ja palauttaa merkkijonon.
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public string Login(string service)
        {
            // Initiate the HttpWebRequest with session support with CookiedFactory
            if (String.IsNullOrEmpty(service)) request = CookiedRequestFactory.CreateHttpWebRequest(wilmaUrl + "index_json");
            else request = CookiedRequestFactory.CreateHttpWebRequest(wilmaUrl + service);

            // Serialize the response to json

            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";

            // Get the login status from the service
            response = (HttpWebResponse)request.GetResponse();
            var reader = new StreamReader(response.GetResponseStream());
            string jsonResponseString = reader.ReadToEnd();
            values = JsonConvert.DeserializeObject<IndexJson>(jsonResponseString);
            reader.Close();
            response.Close();

            // DO the HASH
            ComputeHash(values);

            return jsonResponseString;
        }

        public string Post(string service, WilmaMsg msg)
        {
            string recipients = String.Empty;
            
            if (!String.IsNullOrEmpty(msg.r_teacher)) {
                recipients = "&r_teacher=" + msg.r_teacher;            
            }
            if (!String.IsNullOrEmpty(msg.r_personnel)) {
                recipients += "&r_personnel=" + msg.r_personnel;
            }
            string loginParameters = "format=json&bodytext=" + msg.bodytext + "&Subject=" + msg.Subject + recipients + "&Formkey=" + msg.FormKey;
            
            // Initiate the HttpWebRequest with session support with CookiedFactory
            if (String.IsNullOrEmpty(service)) request = CookiedRequestFactory.CreateHttpWebRequest(wilmaUrl + "messages/compose");            
            else request = CookiedRequestFactory.CreateHttpWebRequest(wilmaUrl + service + "?" + loginParameters);

            // Serialize the response to json
            
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";

            // Serialize the msg to json
            var serializer = new JavaScriptSerializer();
            var jsonRequestString = serializer.Serialize(msg);
            //var bytes = Encoding.UTF8.GetBytes(jsonRequestString);
            var bytes = Encoding.UTF8.GetBytes(loginParameters);


            // Send the json data to the Rest service
            var postStream = request.GetRequestStream();
            postStream.Write(bytes, 0, bytes.Length);
            postStream.Close();



            // Get the login status from the service
            var response = (HttpWebResponse)request.GetResponse();
            var reader = new StreamReader(response.GetResponseStream());
            string jsonResponseString = reader.ReadToEnd();
           // values = JsonConvert.DeserializeObject<IndexJson>(jsonResponseString);
            reader.Close();
            response.Close();

            // DO the HASH
            ComputeHash(values);

            return jsonResponseString;
        }



        /// <summary>
        /// Kirjaudutaan evästeiden kanssa 
        /// </summary>
        /// <param name="LoginUrl"></param>
        /// <returns></returns>
        public string LoginWCookies(string LoginUrl)
        {
            string loginParameters = "Login=" + username + "&Password=" + passwd + "&SessionId=" + values.SessionID + "&ApiKey=" + "sha1:" + hash + "&format=json";
            var bytes = Encoding.UTF8.GetBytes(loginParameters);

            request = CookiedRequestFactory.CreateHttpWebRequest(LoginUrl + "?" + loginParameters);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";


            // Send the json data to the Rest service
            var postStream = request.GetRequestStream();
            postStream.Write(bytes, 0, bytes.Length);
            postStream.Close();

            // Get the login status from the service
            HttpWebResponse response;
            try {
                 response = (HttpWebResponse)request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream());
                var jsonResponseString = reader.ReadToEnd();
                values = JsonConvert.DeserializeObject<IndexJson>(jsonResponseString);
                reader.Close();
                response.Close();
                return jsonResponseString;
            }
            catch (Exception ex)
            {
                return String.Empty;
            }

        }

        public void ComputeHash(IndexJson indexJson)
        {
            string plain;
            // Hash                     
            byte[] temp;
            plain = username + "|" + indexJson.SessionID + "|" + companySpesificKey;
            SHA1 sha = new SHA1CryptoServiceProvider();
            // This is one implementation of the abstract class SHA1.
            temp = sha.ComputeHash(Encoding.UTF8.GetBytes(plain));

            //storing hashed value into byte data type
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < temp.Length; i++)
            {
                sb.Append(temp[i].ToString("x2"));
            }
            hash = sb.ToString();
        }
    }
}
