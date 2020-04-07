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
            // Collect messages to same teacher - so that only one daily resume message can be send to each teacher.
            var teacherMessages = new Dictionary<string, List<WilmaMsg>>();


            // Get the alert type - first one 
            // TODO: Iterate trough all alert types     
            
            var alertTypes = new List<AlertType>();

            using (PrimusAlertContext context = new PrimusAlertContext())
            {
                alertTypes = (from a in context.AlertTypes select a).ToList();
            }

            // Configuration
            var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json");
            var config = builder.Build();
            Application appConfig = config.GetSection("application").Get<Application>();


            // Loop through alerttypes
            foreach (var alertType in alertTypes)
            {
                // If Alerttype is disabled or has empty columns for some reason - skip it.
                if (!alertType.IsInUse || String.IsNullOrEmpty(alertType.Name) || String.IsNullOrEmpty(alertType.QueryName)  ||
                    String.IsNullOrEmpty(alertType.AlertMsgText) || String.IsNullOrEmpty(alertType.AlertMsgSubject)) continue;

               
                Console.WriteLine("Application Name : {appConfig.Path2PQExe}");

                // Start the child process.
                Process p = new Process();
                // Redirect the output stream of the child process.
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;

                // Read original command file with credentials and server address, file stored in root of your compiled app.
                string fileContent = File.ReadAllText(appConfig.SourceCmdFilename);
                // Append PrimusQuery name to it.
                fileContent += " " + alertType.QueryName;
                // Write file with command in it
                File.WriteAllText(appConfig.DestinationCmdFileName, fileContent);
                p.StartInfo.FileName = appConfig.DestinationCmdFileName;
                p.StartInfo.Arguments = alertType.QueryName;
                p.Start();
                // Do not wait for the child process to exit before
                // reading to the end of its redirected stream.
                // p.WaitForExit();
                // Read the output stream first and then wait.
                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                string resultString = output.Replace(";\r\n", string.Empty);
                resultString = resultString.Replace("\r\n", string.Empty);

                // Result from primusquery must start with string <<<< OUTPUT >>>> !
                string[] resultStrings = resultString.Split("<<<< OUTPUT >>>>", StringSplitOptions.None);
                if (resultString.Count() > 1) resultString = resultStrings[1];
                else
                {
                    // Error, did not get appropriate response from Primus
                    // TODO: Log error to Windows Log and program-wide Logging solution - if available.
                    Console.WriteLine("Error. Did not get apropriate response from Primus.");
                    // Using Error Code 24 to indicate not enough lines.
                    // Microsoft conventions: https://docs.microsoft.com/fi-fi/windows/win32/debug/system-error-codes--0-499-?redirectedfrom=MSDN
                    Environment.Exit(24);
                }

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

                    using (PrimusAlertContext context = new PrimusAlertContext())
                    {
                        var pa = new PrimusAlert()
                        {
                            CardNumber = opiskelija.korttinumero,
                            SentDate = DateTime.Now,
                            AlertReceiverId = receiver.Id,
                            //AlertReceiver = receiver,
                            //AlertType = alertType,
                            AlertTypeId = alertType.Id

                        };

                        context.PrimusAlerts.Add(pa);
                        context.SaveChanges();
                    }


                }
                // TODO: Sitten tallennetaan hälytys tietokantaan. Hälytys tyyppi, korttinumero, pvm ja vastaanottaja



                string FormKey = String.Empty;

                foreach (Opiskelija op in opiskelijat)
                {
                    string teacher = op.vastuukouluttaja.korttinumero;

                  

                    WordUtil wordUtil = new WordUtil(op.vastuukouluttaja, alertType, op, appConfig.wilmaUrl);
                    ParserUtil parse = new ParserUtil(wordUtil.ReturnWords());
                    string parsedMsgText = parse.ReplaceWithKeyWords(alertType.AlertMsgText);

                    // DEBUG: Tanja Personnel (106) Ope (338) + Jani (27)  Ope (339)
                    WilmaMsg wilmaViesti2 = new WilmaMsg { FormKey = FormKey, bodytext = parsedMsgText, Subject = alertType.AlertMsgSubject, r_personnel = "106", r_teacher = "339" };


                    if (teacherMessages.ContainsKey(teacher))
                    {
                        // Allready
                        teacherMessages[teacher].Add(wilmaViesti2);
                    }
                    else
                    {
                        // New teacher
                        teacherMessages.Add(teacher, new List<WilmaMsg> { wilmaViesti2 });

                    }


                    // Sitten lähetetään hälytys ottamalla yhteys Wilmaan/luomalla Wilma viesti.
                    // TODO: Collect email alerts into one Wilma message per receiver - otherwise too much messages.
                    // Sitten lähetetään hälytys
                    // TODO: An userinteface for wilmaMessages and subjects.
                    // TODO: Now hardcoded r_teacher - should use the appropriate one.

                    //WilmaJson wilma = new WilmaJson(appConfig.wilmaUrl, appConfig.wilmaPasswd, appConfig.wilmaUsername, appConfig.wilmaCompanySpesificKey);
                
                    //// Luodaan sessio.
                    //try
                    //{
                    //    string firstContact = wilma.Login(string.Empty);
                    //    // Kirjaudutaan
                    //    string loginWCookiesResult = wilma.LoginWCookies(appConfig.wilmaUrl + "login");
                    //    WilmaResponse wilmaResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<WilmaResponse>(loginWCookiesResult);
                    //    // FormKey = wilmaResponse.FormKey.Split(':')[2];
                    //    FormKey = wilmaResponse.FormKey;


                    //}
                    //catch (Exception ex)
                    //{
                    //    string firstContact = wilma.Login(string.Empty);
                    //    // Kirjaudutaan
                    //    string loginWCookiesResult = wilma.LoginWCookies(appConfig.wilmaUrl + "login");
                    //    // Poimitaan formkey
                    //    WilmaResponse wilmaResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<WilmaResponse>(loginWCookiesResult);
                    //    // FormKey = wilmaResponse.FormKey.Split(':')[2];
                    //    FormKey = wilmaResponse.FormKey;

                    //}





                    //try
                    //{
                    //    //  var result2 = wilma.Post("messages/compose", wilmaViesti);
                    //    var result3 = wilma.Post("messages/compose", wilmaViesti2);
                    //}
                    //catch (Exception ex)
                    //{
                    //    // TODO: Handle error messages and report 
                    //    //var result2 = wilma.Post("messages/compose/", wilmaViesti);

                    //}
                }
            }

            // Send messages through Wilma
            SendMessages(teacherMessages, appConfig);


        }

        private static void SendMessages(Dictionary<string, List<WilmaMsg>> teacherMessages, Application appConfig)
        {
            string FormKey = String.Empty;
            // First Connect to Wilma.
            WilmaJson wilma = new WilmaJson(appConfig.wilmaUrl, appConfig.wilmaPasswd, appConfig.wilmaUsername, appConfig.wilmaCompanySpesificKey);
            // First Open Session With Wilma:
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

            // Then iterate through messages and send messages

            WilmaMsg firstMsg = new WilmaMsg();
            string result3 = String.Empty;
            
            foreach (var teacher in teacherMessages)
            { 
                try
                    {                    
                    // If only one message to teacher today:
                        if (teacher.Value.Count() == 1) 
                        {
                           result3 = wilma.Post("messages/compose", teacher.Value.FirstOrDefault());
                        }
                        else
                        {
                            // Form message from multiple messages but send only one message.
                            var msgBody = new StringBuilder();
                            foreach (var mesg in teacher.Value)
                            {
                                msgBody.Append(mesg.bodytext + " " + Environment.NewLine);
                            }
                             firstMsg = teacher.Value.FirstOrDefault();
                            firstMsg.bodytext = msgBody.ToString();
                            firstMsg.FormKey = FormKey;
                            result3 = wilma.Post("messages/compose", firstMsg);
                        }
                    }
                    catch (Exception ex)
                    {
                    // TODO: Handle error messages and report 
                    //var result2 = wilma.Post("messages/compose/", wilmaViesti);
                    // Console.WriteLine("Result3: " + result3);
                    Console.WriteLine("FirstMsg teacher: " + firstMsg.r_teacher);
                    Console.WriteLine(ex.Message);
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
