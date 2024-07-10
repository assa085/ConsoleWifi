using System;
using System.Management.Automation;
using LibraryWifi;


class Program
{
    static void Main()
    {
        WiFiDll wiFiDll = new WiFiDll();
        WiFiDll.WifiStart(true, true);
    }
}