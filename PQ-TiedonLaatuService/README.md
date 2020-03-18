To Install:

1. Clone project from Git
2. Use Update-Database at Nuget Command-line
3. Create commandline file to run Primusquery.exe - you can name it primusquery.cmd - with following content:
U:\Path\To\primusquery.exe address.of.your.primus.server 1222 primusqueryaccount primusqueryaccountpassword


4. Create file "appsettings.json" in root of your project With following content:

{
  "application": {
    "Path2PQExe": "U:\\Directory\\Path\\",
    "CmdFilename": "primusquery.cmd",
    "Path2PQConfiguration": "PQ\\jani\\alarm1.txt",
    "Path2PQResultDir": "PQ\\results\\",
    "Path2WorkDir": "PQ\\results\\",
    "wilmaCompanySpesificKey": "yourCompanySpesificKeyHere",
    "wilmaUrl": "https://urlForWilma",
    "wilmaUsername": "usernameForWilmaServiceAccount",
    "wilmaPasswd": "passwordForWilmaServiceAccount"
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
    "PrimusAlertContext": "Server=localhost\\SQLEXPRESS;;Database=PrimusAlert;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
  }
