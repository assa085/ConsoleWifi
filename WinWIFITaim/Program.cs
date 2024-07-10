using System.Management.Automation;
using System.Text;
using LibraryTime;
using LibraryWifi;
using System;
using System.Security.Cryptography.X509Certificates;
using Timer = LibraryTime.Timer;
using LiberiScan;

class Program
{
    static void Main(string[] args)
    {
        WiFiDll wiFiDll = new WiFiDll();

        Config config = new Config("Config.txt");
        config.ScanSetting();
        wiFiDll.BoolPrint = config.PrintData;

        if (config.ConsoleHidden) { Console.Clear(); }
        
        Timer timer = new Timer(config.DateTimeOn, config.DateTimeOff, config.PrintData);
        timer.Start(() =>
        {
            WiFiDll.WifiStart(true,config.ConsoleHidden); // Логика для включения WIFI
        }, () =>
        {
            WiFiDll.WifiStart(false,config.ConsoleHidden); // Логика для выключения WIFI
        });




    }
   
    

}