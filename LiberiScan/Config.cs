namespace LiberiScan
{
    public class Config
    {
        private string pathConfig = "Config.txt";
        DateTime dateTimeOn = DateTime.MinValue;
        DateTime dateTimeOff = DateTime.MinValue;

        string configStandart = "сonsoleHidden = false\r\nprintData = true\r\ndateTimeOn = 00:06:00\r\ndateTimeOff = 00:00:00";
        public DateTime DateTimeOn //вкыл
        {
            get { return dateTimeOn; }
        }
        public DateTime DateTimeOff //выкл
        {
            get { return dateTimeOff; }
        }
        bool consoleHidden;
        bool printData;
        public bool PrintData
        {
            get { return printData; }
        }
        public bool ConsoleHidden
        {
            get { return consoleHidden; }
        }
        private List<string> listStr;


        public void DatePrint()
        {
            Console.WriteLine(dateTimeOff.ToString());
            Console.WriteLine(dateTimeOn.ToString());
        }
        public void Error()
        {

            if (dateTimeOn == DateTime.MinValue) { Console.WriteLine("Время старта не найдено"); }
            if (dateTimeOff == DateTime.MinValue) { Console.WriteLine("Время остановки не найдено"); }
        }
        public Config()
        {
            BuildingClass();
        }
        public Config(string str)
        {
            pathConfig = Path.Combine(str);
            BuildingClass();
        }
        void GenerateConfigTxt()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(pathConfig))
                {
                    sw.Write(configStandart);
                }
            }
            catch (Exception ex) { Console.WriteLine(ex); Console.WriteLine("Ошибка при создании конфигурационного фаила."); }
        }
        void BuildingClass()
        {
            printData = false;
            listStr = new List<string>();
            if (!File.Exists(pathConfig))
            {
                Console.WriteLine("Ошибка чтения конфига, возможно он отсутствует.");
                GenerateConfigTxt();
                if (printData) { Console.WriteLine("Создан новый конфигурационный файл"); }
            }
            try
            {
                using (StreamReader sr = new StreamReader(pathConfig))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        listStr.Add(line);
                    }
                }
            }
            catch { Console.WriteLine("Ошибка чтения конфига. Причина не известна"); }
        }

        public void ScanSetting()
        {

            if (listStr.Count == 0) { Console.WriteLine("Фаил конфигурации пуст."); return; }

            foreach (string line in listStr)
            {
                if (line.Contains("printData"))
                {
                    printData = ScanBool(line, "printData");
                }
                if (line.Contains("consoleHidder"))
                {
                    consoleHidden = ScanBool(line, "consoleHidden");
                }
                ScanTimeStart(line);
                ScanTimeStop(line);

            }


        }
        public void ScanSetting(string path)
        {
            this.pathConfig = Path.Combine(path);
            ScanSetting();
        }

        bool ScanBool(string line, string name)
        {
            if (line.Contains(name))
            {
                if (line.Contains("true"))
                {
                    return true;
                }
                if (line.Contains("false"))
                {
                    return false;
                }

            }
            return false;  
        }
        

        void ScanTimeStart(string line)
        {
            if (line.Contains("dateTimeOn"))
            {
                string bufStrTime = line.Replace("dateTimeOn = ", "").Replace("dateTimeOn=", "").Trim();
                DateTime dateTimeStartBuf;
                if (DateTime.TryParse(bufStrTime, out dateTimeStartBuf))
                {
                    // Установка dateTimeStart с сохранением только часов, минут и секунд
                    this.dateTimeOn = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                 dateTimeStartBuf.Hour, dateTimeStartBuf.Minute, dateTimeStartBuf.Second);

                    if (printData) { Console.WriteLine("Время старта: " + this.dateTimeOn.ToString("HH:mm:ss"));}
                }
                else
                {
                    Console.WriteLine("Ошибка чтения времени (dateTimeStart)");
                }
            }
        }
        void ScanTimeStop(string line)
        {
            if (line.Contains("dateTimeOff"))
            {
                string bufStrTime = line.Replace("dateTimeOff = ", "").Replace("dateTimeOff=", "").Trim();
                DateTime dateTimeStopBuf = DateTime.MinValue;
                if (DateTime.TryParse(bufStrTime, out dateTimeStopBuf))
                {

                    // Установка dateTimeStart с сохранением только часов, минут и секунд
                    this.dateTimeOff = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                        dateTimeStopBuf.Hour, dateTimeStopBuf.Minute, dateTimeStopBuf.Second);

                    if (printData) { Console.WriteLine("Время окончания: " + this.dateTimeOff.ToString("HH:mm:ss")); }
                }
                else
                {
                    Console.WriteLine("Ошибка чтения времени (dateTimeStop)");
                }
            }

        }
    }
}
