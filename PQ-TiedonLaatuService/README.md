This is the service part of Information quality alert system for Primus school administration. Schedule this to run once in a day to remind teachers and students for particular tasks to be completed in Wilma.


To Install:

1. Clone project from Git and open project with Visual Studio.
2. Use Update-Database at Nuget Command-line
3. Create commandline file to run Primusquery.exe - you can name it primusquery.cmd - with following content:
U:\Path\To\primusquery.exe address.of.your.primus.server 1222 primusqueryaccount primusqueryaccountpassword


4. Create file "appsettings.json" in root of your project with following content, and replace keys with appropriate ones:

{
  "application": {
    "SourceCmdFilename": "primuskysely.source",
    "DestinationCmdFileName": "primuskysely.cmd",
    "wilmaCompanySpesificKey": "xxxxxxxxxxxxxxxxxxxxxxxxxxx",
    "wilmaUrl": "https://wilma.xxxxxxx.fi/",
    "wilmaUsername": "xxxxxxx ",
    "wilmaPasswd": "xxxxxxxxx"
  },  
    "Logging": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft": "Warning",
        "Microsoft.Hosting.Lifetime": "Information"
      }
    },
    "AllowedHosts": "*",
  "ConnectionStrings": {
    "PrimusAlertContext": "Serv-er=localhost\\SQLEXPRESS;;Database=PrimusAlert;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
  }

(You need your organizations WilmaCompanySpesificKey, WilmaUrl for Url of your Wilma web-address, typically wilma.organizationname.fi, WilmaUsername & password for useraccount with appropriate rights to send Wilma messages, your wilma Url address, and SQL Server-connection string)

5. You also need to create a file that is referenced with key "SourceCmdFileName" inside that file, you should have appropriate command line to execute Primus queries, here's example:

\Path\to\primusquery.exe primus-server-address 1222 primusqueryaccount primusquerypasswd

Where \Path\to\primusquery.exe is the location of your primusquery.exe in server.
primus-server-address is ip or name address of Primus-server
1222 is default port for Primus
primusqueryaccount is your service account for running PrimusQueries
primusquerypasswd is password for your service account.

6. Now you should have everything configured for server-part of this software. You should now continue configuring userinterface and
install project PQ-TidenLaatu-UI
