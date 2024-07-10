using System;
public class Config()
{
    private string pathConfig = "Config.txt";
    DateTime? dateTimeStart { get; set; }
    DateTime? dataTimeStop { get; set; }
    bool printData { get; set; }
    private List<string> listStr;
    public Config()
    {
        printData = false;
        listStr = new List<string>();
        if (!File.Exists(pathConfig))
        {
            Console.WriteLine("Ошибка чтения конфига, возможно он отсутствует.");
            return;
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
    void ScanSetting()
    {
        foreach (string line in listStr)
        {
            ScanBull(line);
            ScanTimeStart(line);

        }
    }
    void ScanBull(string line)
    {
        if (line.Contains(printData))
        {
            if (line.Contains("true"))
            {
                this.printData = true;
            }
            if (line.Contains("false"))
            {
                this.printData = false;
            }
        }
    }
    void ScanTimeStart(string line)
    {
        if (line.Contains("dateTimeStart"))
        {
            string bufStrTime = line.Replace("dateTimeStart = ", "").Replace("dateTimeStart=", "").Trim();

            if (DateTime.TryParse(bufStrTime, out dateTimeStart))
            {
                // Установка dateTimeStart с сохранением только часов, минут и секунд
                dateTimeStart = new DateTime(dateTimeStart.Hour, dateTimeStart.Minute, dateTimeStart.Second);

                if (printData) { Console.WriteLine("Время старта: " + dateTimeStart.ToString("HH:mm:ss")); }
            }
            else
            {
                Console.WriteLine("Ошибка чтения времени (dateTimeStart)");
            }
        }
        else {
            dateTimeStart = null;
        }
    }
    void ScanTimeStop(string line)
    {
        if (line.Contains("dateTimeStop"))
        {
            string bufStrTime = line.Replace("dateTimeStop = ", "").Replace("dateTimeStop=", "").Trim();

            if (DateTime.TryParse(bufStrTime, out dateTimeStart))
            {
                // Установка dateTimeStart с сохранением только часов, минут и секунд
                dateTimeStop = new DateTime(dateTimeStop.Hour, dateTimeStop.Minute, dateTimeStop.Second);

                if (printData) { Console.WriteLine("Время старта: " + dateTimeStart.ToString("HH:mm:ss")); }
            }
            else
            {
                Console.WriteLine("Ошибка чтения времени (dateTimeStart)");
            }
        }
        else
        {
            dataTimeStop = null;
        }

    }

}
}

