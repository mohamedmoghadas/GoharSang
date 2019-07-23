using FreeControls;
using System;
using System.Globalization;

namespace System
{
    public class clsPersianDate
    {
        public static string MiladiToShamsi(DateTime? _date)
        {
            PersianCalendar pc = new PersianCalendar();
            try
            {
                string now = "";
                string m = pc.GetMonth(_date.Value) <= 9 ? "0" + pc.GetMonth(_date.Value).ToString() : pc.GetMonth(_date.Value).ToString();
                string d = pc.GetDayOfMonth(_date.Value) <= 9 ? "0" + pc.GetDayOfMonth(_date.Value).ToString() : pc.GetDayOfMonth(_date.Value).ToString();
                now = pc.GetYear(_date.Value).ToString() + "/" + m + "/" + d;
                return now;
            }
            catch { return ""; }
        }

        public static PersianDate MiladiToShamsi2(DateTime _date)
        {
            PersianCalendar pc = new PersianCalendar();
            PersianDate now = new PersianDate();
            try
            {
                string m = pc.GetMonth(_date) <= 9 ? "0" + pc.GetMonth(_date).ToString() : pc.GetMonth(_date).ToString();
                string d = pc.GetDayOfMonth(_date) <= 9 ? "0" + pc.GetDayOfMonth(_date).ToString() : pc.GetDayOfMonth(_date).ToString();
                now = new PersianDate(pc.GetYear(_date), int.Parse(m), int.Parse(d));
                return now;
            }
            catch { return now; }
        }

        public static DateTime? ShamsiToMiladi(string _date)
        {
            if (_date != null)
            {
                int year = int.Parse(_date.Substring(0, 4));
                int month = int.Parse(_date.Substring(5, 2));
                int day = int.Parse(_date.Substring(8, 2));
                PersianCalendar p = new PersianCalendar();
                DateTime date = p.ToDateTime(year, month, day, 0, 0, 0, 0);
                return date;
            }
            else
            {
                return null;
            }
        }

        public static TimeSpan GenerateTime(string _time)
        {
            TimeSpan result = new TimeSpan(0, 0, 0);
            try
            {
                string[] _time_parts = _time.Split(':');
                result = new TimeSpan(int.Parse(_time_parts[0]), int.Parse(_time_parts[1]), 0);
            }
            catch (Exception)
            {
            }
            return result;
        }
    }
}