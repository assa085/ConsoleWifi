using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace LibraryTime
{

    public class Timer
    {
        private DateTime dateTimeOn;
        private DateTime dateTimeOff;
        private readonly bool boolPrint;

        public delegate void StartDelegate();
        public delegate void StopDelegate();

        public Timer(DateTime dStart, DateTime dStop, bool boolPrint)
        {
            this.boolPrint = boolPrint;
            dateTimeOn = SystemTimeZoneCorrect(dStart);
            dateTimeOff = SystemTimeZoneCorrect(dStop);
        }

        private bool IfStartInStop(DateTime dtStart, DateTime dtStop)
        {
            double dtStartSec = dtStart.TimeOfDay.TotalSeconds;
            double dtStopSec = dtStop.TimeOfDay.TotalSeconds;
            double dt = DateTime.Now.TimeOfDay.TotalSeconds;

            if (dtStopSec < dtStartSec) // Событие Stop происходит на следующий день
            {
                return dt >= dtStartSec || dt < dtStopSec;
            }
            else // Событие Stop происходит в тот же день
            {
                return dt >= dtStartSec && dt < dtStopSec;
            }
        }

        public void Start(StartDelegate DStart, StopDelegate DStop)
        {
            var timerThread = new Thread(() =>
            {
                while (true)
                {
                    if (IfStartInStop(dateTimeOn, dateTimeOff))
                    {
                        DStart?.Invoke();
                        Print($"Start action triggered at {DateTime.Now.TimeOfDay}");
                    }
                    else
                    {
                        DStop?.Invoke();
                        Print($"Stop action triggered at {DateTime.Now.TimeOfDay}");
                    }

                    Thread.Sleep(GetSleepDuration());
                }
            });
            timerThread.Start();
        }

        private DateTime SystemTimeZoneCorrect(DateTime dateTime)
        {
            TimeZoneInfo systemTimeZone = TimeZoneInfo.Local;
            DateTime correctedTime = TimeZoneInfo.ConvertTime(dateTime, systemTimeZone);
            Print($"{dateTime} converted to {correctedTime} in local time zone.");
            return correctedTime;
        }

        private int GetSecondsUntilEventInRange(DateTime date)
        {
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            int currentSeconds = (int)currentTime.TotalSeconds;
            int targetSeconds = (int)date.TimeOfDay.TotalSeconds;

            return targetSeconds - currentSeconds;
        }

        private void Print(string message)
        {
            if (boolPrint)
            {
                Console.WriteLine(message);
            }
        }

        private int GetSleepDuration()
        {
            int sleepDuration = 1000; 
            int secondsUntilOn = GetSecondsUntilEventInRange(dateTimeOn);
            int secondsUntilOff = GetSecondsUntilEventInRange(dateTimeOff);

            if (secondsUntilOn >= 61 || secondsUntilOff >= 61)
            {
                sleepDuration = 3000;
            }

            return sleepDuration;
        }
    }

    
}



