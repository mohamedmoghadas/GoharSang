using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace System
{
    public static class ExtentionFunction
    {
        public static string Topersian(this DateTime dt){
        PersianCalendar _p = new PersianCalendar();
        int day=_p.GetDayOfMonth(dt);
        int month = _p.GetMonth(dt);
        int year = _p.GetYear(dt);
        return year + "/" + month + "/" + day;
    }
        public static DateTime  shamsitomiladi(this DateTime dt)
        {
            PersianCalendar _persian = new PersianCalendar();
            int _year =dt.Year;
            int _month = dt.Month;
            int _day = dt.Day;
            int _hour = dt.Hour;
            int _min = dt.Minute;
            int _sec = dt.Second;
            int _milisec = dt.Millisecond;
            return _persian.ToDateTime(_year, _month, _day, _hour, _min, _sec, _milisec);
        }


        public static string GetPersianDetial(this DateTime dt)
        {
            PersianCalendar _persian = new PersianCalendar();
            int day = _persian.GetDayOfMonth(dt);
            int Month = _persian.GetMonth(dt);
            int year = _persian.GetYear(dt);

            string str = " امروز ";
            DayOfWeek d = _persian.GetDayOfWeek(dt);
            switch (d)
            {
                case DayOfWeek.Friday:
                    { str += " جمعه "; break; }
                case DayOfWeek.Monday:
                    { str += " دوشنبه "; break; }
                case DayOfWeek.Saturday:
                    { str += " شنبه "; break; }
                case DayOfWeek.Sunday:
                    { str += " یکشنبه "; break; }
                case DayOfWeek.Thursday:
                    { str += " پنج شنبه "; break; }
                case DayOfWeek.Tuesday:
                    { str += " سه شنبه "; break; }
                case DayOfWeek.Wednesday:
                    { str += " چهار شنبه "; break; }
            }
            str += day;
            switch (Month)
            {
                case 1: { str += " فروردین ماه "; ; break; }
                case 2: { str += " اردیبهشت ماه "; ; break; }
                case 3: { str += " خرداد ماه "; ; break; }
                case 4: { str += " تیر ماه "; ; break; }
                case 5: { str += " مرداد ماه "; ; break; }
                case 6: { str += " شهریور ماه "; ; break; }
                case 7: { str += " مهر ماه "; ; break; }
                case 8: { str += " آبان ماه "; ; break; }
                case 9: { str += " آذر ماه "; ; break; }
                case 10: { str += " دی ماه "; ; break; }
                case 11: { str += " بهمن ماه "; ; break; }
                case 12: { str += " اسفند ماه "; ; break; }
            }
            str += " سال " + year;
            return str;
        }

    }
       
}