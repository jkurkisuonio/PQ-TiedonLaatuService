using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PQ_TiedonLaatuService.Data;
using PQ_TiedonLaatuService.Models;
using PQ_TiedonLaatuService.Models.Database;
using PQ_TiedonLaatuService.Service;
using System.Linq;

namespace PQ_TiedonLaatuService
{
    class Program
    {
        /// <summary>
        /// https://stackoverflow.com/questions/206323/how-to-execute-command-line-in-c-get-std-out-results
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            // Get the alert type - first one 
            // TODO: Iterate trough all alert types     
            AlertType alertType = new AlertType();
            using (PrimusAlertContext context = new PrimusAlertContext())
            {
                alertType = (from a in context.AlertTypes select a).FirstOrDefault();
            }


            // Configuration
            var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json");
            var config = builder.Build();
            var appConfig = config.GetSection("application").Get<Application>();
            Console.WriteLine("Application Name : {appConfig.Path2PQExe}");

            // Start the child process.
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "primuskysely.cmd";
            p.Start();
            // Do not wait for the child process to exit before
            // reading to the end of its redirected stream.
            // p.WaitForExit();
            // Read the output stream first and then wait.
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            string resultString = output.Replace(";\r\n", string.Empty);
            resultString = resultString.Replace("\r\n", string.Empty);
            resultString = resultString.Split("<<<< OUTPUT >>>>", StringSplitOptions.None)[1];

            XDocument doc = XDocument.Parse(resultString);

            List<Opiskelija> opiskelijat = new List<Opiskelija>();
            foreach (var opisk in doc.Descendants("opiskelija"))
            {
                var opiskelija = new Opiskelija();
                opiskelija.etunimi = opisk.Element("etunimi").Value;
                opiskelija.sukunimi = opisk.Element("sukunimi").Value;
                opiskelija.korttinumero = opisk.Element("korttinumero").Value;
                XElement vastuukouluttaja = opisk.Element("vastuukouluttaja");
                string email = vastuukouluttaja.Element("email").Value;
                string korttinumero = vastuukouluttaja.Element("korttinumero").Value;
                opiskelija.vastuukouluttaja = new Vastuukouluttaja { email = email, korttinumero = korttinumero };
                opiskelijat.Add(opiskelija);

                // Check whenether there is already a receiver.

                AlertReceiver receiver = GetReceiver(opiskelija.vastuukouluttaja);

                using (PrimusAlertContext context = new PrimusAlertContext()) {
                    var pa = new PrimusAlert()
                    {
                        CardNumber = opiskelija.korttinumero,
                        ReceiverCardNumber = receiver.CardNumber,
                        SentDate = DateTime.Now,
                       // AlertReceiverId = receiver.AlertReceiverId,                       
                       //AlertReceiver = receiver,
                        //AlertType = alertType,
                       // AlertTypeId = alertType.AlertTypeId

                    };

                    context.PrimusAlerts.Add(pa);
                    context.SaveChanges();
                }


            }
            // TODO: Sitten tallennetaan hälytys tietokantaan. Hälytys tyyppi, korttinumero, pvm ja vastaanottaja




            // Sitten lähetetään hälytys ottamalla yhteys Wilmaan/luomalla Wilma viesti.
            WilmaJson wilma = new WilmaJson(appConfig.wilmaUrl, appConfig.wilmaPasswd, appConfig.wilmaUsername, appConfig.wilmaCompanySpesificKey);
            string FormKey = String.Empty;
            // Luodaan sessio.
            try
            {
                string firstContact = wilma.Login(string.Empty);
                // Kirjaudutaan
                string loginWCookiesResult = wilma.LoginWCookies(appConfig.wilmaUrl + "login");
                WilmaResponse wilmaResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<WilmaResponse>(loginWCookiesResult);
                // FormKey = wilmaResponse.FormKey.Split(':')[2];
                FormKey = wilmaResponse.FormKey;


            }
            catch (Exception ex)
            {
                string firstContact = wilma.Login(string.Empty);
                // Kirjaudutaan
                string loginWCookiesResult = wilma.LoginWCookies(appConfig.wilmaUrl + "login");
                // Poimitaan formkey
                WilmaResponse wilmaResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<WilmaResponse>(loginWCookiesResult);
                // FormKey = wilmaResponse.FormKey.Split(':')[2];
                FormKey = wilmaResponse.FormKey;

            }
            // Sitten lähetetään hälytys

            // TODO: An userinteface for wilmaMessages and subjects.
            // TODO: Now hardcoded r_teacher - should use the appropriate one.
            foreach (Opiskelija op in opiskelijat)
            {
                var teacher = op.vastuukouluttaja.korttinumero;
                // TODO: Add personalizations to bodytext.

                WordUtil wordUtil = new WordUtil(op.vastuukouluttaja, alertType, op);
                ParserUtil parse = new ParserUtil(wordUtil.ReturnWords());
                string parsedMsgText = parse.ReplaceWithKeyWords(alertType.AlertMsgText);

                var wilmaViesti = new WilmaMsg { FormKey = FormKey, bodytext = parsedMsgText, Subject = alertType.AlertMsgSubject, r_teacher = "339" };

     

                try {
                    var result2 = wilma.Post("messages/compose", wilmaViesti);
                }
                catch (Exception ex)
                {
                    // TODO: Handle error messages and report 
                    //var result2 = wilma.Post("messages/compose/", wilmaViesti);

                }
            }
        }

        private static AlertReceiver GetReceiver(Vastuukouluttaja vastuukouluttaja)
        {
            using (PrimusAlertContext context = new PrimusAlertContext())
            {
                AlertReceiver q = (from a in context.AlertReceivers where a.CardNumber == vastuukouluttaja.korttinumero && a.Email == vastuukouluttaja.email select a).FirstOrDefault();
                if (q != null) return q;
                else
                {
                    // Create new one or update existing one.
                    // Update?
                    AlertReceiver q2 = (from a in context.AlertReceivers where a.CardNumber == vastuukouluttaja.korttinumero select a).FirstOrDefault();
                    if (q2 != null)
                    {
                        // Update email address and return existing datarow.
                        q2.Email = vastuukouluttaja.email;
                        context.SaveChanges();
                        return q2;
                    }
                    else
                    {
                        // Create new one.
                        var a = new AlertReceiver { CardNumber = vastuukouluttaja.korttinumero, Email = vastuukouluttaja.email };
                        context.AlertReceivers.Add(a);
                        context.SaveChanges();
                        return a;
                    }

                }
            }



            static void CreateDbIfNotExists(IHost host)
            {
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    try
                    {
                        var context = services.GetRequiredService<PrimusAlertContext>();
                        context.Database.EnsureCreated();
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred creating the DB.");
                    }
                }
            }







            static void Main2(string[] args)
            {
                // Configuration
                var builder = new ConfigurationBuilder()
                     .SetBasePath(Directory.GetCurrentDirectory())
                     .AddJsonFile("appsettings.json");

                var config = builder.Build();

                var appConfig = config.GetSection("application").Get<Application>();

                Console.WriteLine("Application Name : {appConfig.Path2PQExe}");



                Process cmd = new Process();
                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.RedirectStandardInput = true;
                cmd.StartInfo.RedirectStandardOutput = true;
                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.UseShellExecute = false;
                cmd.Start();
                string komentorivi = '\u0022' + appConfig.Path2PQExe + @"\primusquery.exe" + '\u0022' + " " + '\u0022' + appConfig.Path2PQConfiguration + '\u0022';

                Console.WriteLine("Komentorivi: " + komentorivi);
                cmd.StandardInput.WriteLine(komentorivi);
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
                cmd.WaitForExit();

                // Operoi tiedostosta tyhjät rivit pois.                    
                string myPath = appConfig.Path2WorkDir + "halytys1.csv".ToString();
                //if (File.Exists(appConfig.Path2WorkDir + "opsot2019.csv"))          
                if (File.Exists((myPath)))
                {
                    Console.WriteLine(appConfig.Path2WorkDir);
                    string text = System.IO.File.ReadAllText((@appConfig.@Path2WorkDir) + "halytys1.csv", Encoding.UTF8);
                    var resultString = Regex.Replace(text, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
                    System.IO.File.WriteAllText(appConfig.Path2PQResultDir + "halytys1.csv", resultString, Encoding.UTF8);
                    // POISTETAAN TYÖTIEDOSTO.
                    var fullPath = appConfig.Path2WorkDir + "halytys1.csv";
                    File.Delete(appConfig.Path2WorkDir + "halytys1.csv");
                    // OTETAAN BACKUP VARSINAISESTA.
                    File.Copy(appConfig.Path2PQResultDir + "halytys.csv", appConfig.Path2PQResultDir + "halytys.bak", true);
                    var fullPath2 = appConfig.Path2PQResultDir + "halytys1.csv";
                    var fullPathBak = appConfig.Path2PQResultDir + "halytys.bak";
                }
                else
                {
                    // Virhe! Ei saatu tiedostoa, lähetä sähköpostia ja kirjoita lokiin.
                    var resultString = "VIRHE! Yhteys Primukseen ei toimi, tai muu virhe. Ei voitu muodostaa opsot.csv tiedostoa.  TODETTU: " + DateTime.Now + Environment.NewLine + " VARMUUSKOPIO EDELLISESTÄ TOIMIVASTA Opsot.csv tiedostosta löytyy tästä hakemistosta nimellä opsot.bak";
                    var fullPath = appConfig.Path2PQResultDir + "halytys1.csv";
                    System.IO.File.WriteAllText(appConfig.Path2PQResultDir + "opsot.csv", resultString, Encoding.UTF8);
                }
            }
        }


    } 
}
