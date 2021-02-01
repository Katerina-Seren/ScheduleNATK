using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Globalization;
using System.Data;
using MySql.Data.MySqlClient;

namespace Schedule
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupsSchedule : ContentPage
    {
        public IList<ScheduleClass> ScheduleClass { get; private set; }

        string Group = "";

        DataTable dt = new DataTable();

        public void ViewGroupsSchedule(string group)
        {
            Group = group;
            using (MySqlConnection conn = DBUtils.GetDBConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM 1c_shedule";
                    MySqlDataReader reader = cmd.ExecuteReader();
                    dt.Load(reader);
                    reader.Close();
                }
                catch (Exception e)
                {
                    string x = ("Error: " + e.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            ThisGroup.Text = "Расписание группы " + group;
            BindingContext = this;
            DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            var todaydayweek = today.DayOfWeek;
            switch (todaydayweek)
            {
                case DayOfWeek.Monday:
                    getdateweek(0, 1, 2, 3, 4, 5);
                    break;
                case DayOfWeek.Tuesday:
                    getdateweek(6, 0, 1, 2, 3, 4);
                    break;
                case DayOfWeek.Wednesday:
                    getdateweek(5, 6, 0, 1, 2, 3);
                    break;
                case DayOfWeek.Thursday:
                    getdateweek(4, 5, 6, 0, 1, 2);
                    break;
                case DayOfWeek.Friday:
                    getdateweek(3, 4, 5, 6, 0, 1);
                    break;
                case DayOfWeek.Saturday:
                    getdateweek(2, 3, 4, 5, 6, 0);
                    break;
                case DayOfWeek.Sunday:
                    getdateweek(1, 2, 3, 4, 5, 6);
                    break;
            }

            //если не передаем дату (то есть при открытии формы до нажатия на кнопку)
            //назначаем data значение понедельник (если сегодня воскресенье)

            if (today.DayOfWeek == DayOfWeek.Sunday)
                today.AddDays(1);

            getSched(today);
        }

        public void getdateweek(int Mon, int Tue, int Wed, int Thu, int Fri, int Sat)
        {
            Monday.Text = "ПН\n" + DateTime.Now.AddDays(Mon).ToString("dd.MM");
            Tuesday.Text = "ВТ\n" + DateTime.Now.AddDays(Tue).ToString("dd.MM");
            Wednesday.Text = "СР\n" + DateTime.Now.AddDays(Wed).ToString("dd.MM");
            Thursday.Text = "ЧТ\n" + DateTime.Now.AddDays(Thu).ToString("dd.MM");
            Friday.Text = "ПТ\n" + DateTime.Now.AddDays(Fri).ToString("dd.MM");
            Saturday.Text = "СБ\n" + DateTime.Now.AddDays(Sat).ToString("dd.MM");
        }

        public GroupsSchedule()
        {
            InitializeComponent();
        }

        public void getColorsDay(DateTime date)
        {
            Label[] Labels = new Label[] { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday };
            //перебрать коллекцию label и выделить цветом текущий день
            foreach (Label label in Labels)
            {
                label.BackgroundColor = Color.FromHex("E3E3E3");
                label.TextColor = Color.FromHex("393D3F");
            }
            for (int i = 0; i < Labels.Length; i++)
            {
                //в том label, который содержит передаваемую в date дату закрашиваем синим
                if (Labels[i].Text.IndexOf(date.ToString("dd.MM")) > 0)
                {
                    Labels[i].BackgroundColor = Color.FromHex("006EA6");
                    Labels[i].TextColor = Color.FromHex("FFFFFF");
                }
            }
        }

        public void getSched(DateTime data)
        {
            
            getColorsDay(data);
            Label[] Labels = new Label[] { FirstLesson, SecondLesson, ThirdLesson, FourthLesson, FifthLesson, SixthLesson };
            Frame[] frames = new Frame[] { FrameFirstLesson, FrameSecondLesson, FrameThirdLesson, FrameFourthLesson, FrameFifthLesson, FrameSixthLesson };
            //фильтрация и сортировка
            DataRow[] schedthis = dt.Select("gruppa = '" + Group + "' AND data = '" + data.ToString("yyyy-MM-dd HH:mm:ss") + "'", "para");

            //очиещение label до начала заполнения
            for (int i = 1; i < Labels.Length - 1; i++)
            {
                Labels[i].Text = "";
                Labels[i].Margin = new Thickness(10);
                frames[i].IsVisible = false;
            }
            FrameZamena.IsVisible = false;

            if (schedthis.Length == 0)
                Labels[0].Text = "Нет расписания";

            else              
                //заполнение labels расписанием 
                for (int i = 0; i < schedthis.Length; i++)
                {

                    if (Convert.ToBoolean(schedthis[i]["zamena"]))
                    {
                        FrameZamena.IsVisible = true;
                        ZamenaLabel.Text = "Изменения в расписании";
                    }

                        string[] time = schedthis[i]["vremya"].ToString().Split(';');
                    if (i != 0 && schedthis[i]["podgruppa"].ToString() == "2")
                    {
                        Labels[i - 1].Text = Labels[i - 1].Text + "\n" +
                        schedthis[i]["prepod"].ToString() + "\n" +
                        schedthis[i]["auditoria"].ToString();
                        Labels[i].Margin = new Thickness(0);
                    }
                    else
                    {
                        Labels[i].Text =
                            schedthis[i]["para"].ToString() + " пара   " + time[0] + " - " + time[1] + "\n" +
                            schedthis[i]["disciplina"].ToString() + "\n" +
                            schedthis[i]["prepod"].ToString() + "\n" +
                            schedthis[i]["auditoria"].ToString();
                        var parent = Labels[i].Parent;
                        frames[i].IsVisible = true;
                    }
                }

        }

        

        void MondayTapped(object sender, EventArgs args)
        {
            int dd = Convert.ToInt32(Monday.Text.ToString().Substring(3, 2));
            int MM = Convert.ToInt32(Monday.Text.ToString().Substring(6, 2));
            DateTime date = new DateTime(DateTime.Now.Year, MM, dd, 0, 0, 0);
            getSched(date);

        }

        void TuesdayTapped(object sender, EventArgs args)
        {
            int dd = Convert.ToInt32(Tuesday.Text.ToString().Substring(3, 2));
            int MM = Convert.ToInt32(Tuesday.Text.ToString().Substring(6, 2));
            DateTime date = new DateTime(DateTime.Now.Year, MM, dd, 0, 0, 0);
            getSched(date);
        }

        void WednesdayTapped(object sender, EventArgs args)
        {
            int dd = Convert.ToInt32(Wednesday.Text.ToString().Substring(3, 2));
            int MM = Convert.ToInt32(Wednesday.Text.ToString().Substring(6, 2));
            DateTime date = new DateTime(DateTime.Now.Year, MM, dd, 0, 0, 0);
            getSched(date);
        }

        void ThursdayTapped(object sender, EventArgs args)
        {
            int dd = Convert.ToInt32(Thursday.Text.ToString().Substring(3, 2));
            int MM = Convert.ToInt32(Thursday.Text.ToString().Substring(6, 2));
            DateTime date = new DateTime(DateTime.Now.Year, MM, dd, 0, 0, 0);
            getSched(date);
        }

        void FridayTapped(object sender, EventArgs args)
        {
            int dd = Convert.ToInt32(Friday.Text.ToString().Substring(3, 2));
            int MM = Convert.ToInt32(Friday.Text.ToString().Substring(6, 2));
            DateTime date = new DateTime(DateTime.Now.Year, MM, dd, 0, 0, 0);
            getSched(date);
        }

        void SaturdayTapped(object sender, EventArgs args)
        {
            int dd = Convert.ToInt32(Saturday.Text.ToString().Substring(3, 2));
            int MM = Convert.ToInt32(Saturday.Text.ToString().Substring(6, 2));
            DateTime date = new DateTime(DateTime.Now.Year, MM, dd, 0, 0, 0);
            getSched(date);
        }

    }
}
