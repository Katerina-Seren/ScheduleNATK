using System;
namespace Schedule
{
    public class ScheduleClass
    {
        public string date { get; set; }//дата
        public string day { get; set; }//день недели
        public int numpar { get; set; }//номер пары
        public string time{ get; set; }//время 
        public string shed { get; set;}//расписание

        public override string ToString()
        {
            //return date+"\r\n"+day +"\r\n"+numpar+ "\r\n"+time+"\r\n"+shed;
            return date + " " + day + " " + numpar + " " + time + " " + shed;
        }
    }
}
