using System.Management.Automation;
using System.Text;

namespace LibraryWifi
{
    public class WiFiDll
    {
        public static string wifi = "";
        public static string wifiCommand = "netsh interface show interface";
        static bool boolPrint = false;
        public bool BoolPrint { set { boolPrint = value; } }
        
        public WiFiDll()
        {
            boolPrint = false;
            using (PowerShell ps = PowerShell.Create())
            {
                ps.AddScript(wifiCommand);
                Console.OutputEncoding = Encoding.UTF8;
                try
                {
                    foreach (PSObject resultItem in ps.Invoke())
                    {
                        if (boolPrint) { Console.WriteLine(resultItem.ToString()); }
                        string outputLine = resultItem.ToString();
                        if (outputLine.Contains("Wi-Fi") || outputLine.Contains("Беспроводная сеть"))
                        {
                            if (boolPrint) { Console.WriteLine("Нашёл"); }
                            wifi = "Беспроводная сеть";
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine("Ошибка: " + ex.Message);
                    Console.WriteLine("Стек вызовов: " + ex.StackTrace);
                }

            }
        }
        public static void WifiStart(bool b,bool consoleH)
        {
            string str = wifi;
            string command;

            if (!b) { command = $"netsh interface set interface \"{str}\" disabled ;"; }
            else{ command = $"netsh interface set interface \"{str}\" enabled ;"; }
            
            using (PowerShell ps = PowerShell.Create())
            {
                try
                {
                    ps.AddScript(command);
                    ps.Invoke();

                    if (consoleH) { Console.WriteLine(command); }
                    
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); if (boolPrint) { Console.ReadLine(); } }
            }
        }
    }

}
