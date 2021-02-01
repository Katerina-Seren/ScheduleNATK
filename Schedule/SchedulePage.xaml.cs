using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Data;
using MySql.Data.MySqlClient;
using System.Linq;

namespace Schedule
{
    public partial class SchedulePage : ContentPage
    {

        List<string> Groups = new List<string>();

        DataTable dt = new DataTable();
        public SchedulePage()
        {
            InitializeComponent();

            //подключение к бд

            using (MySqlConnection conn = DBUtils.GetDBConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT DISTINCT gruppa FROM 1c_shedule order by gruppa;";
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
            Groups = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                Groups.Add(row["gruppa"].ToString());
            }
            listView.ItemsSource = Groups;

        }


        async void GroupsScheduleItemTapped(object senser, ItemTappedEventArgs e)
        {
            string tappedItem = e.Item.ToString();
            GroupsSchedule groupsSchedule = new GroupsSchedule();
            await Navigation.PushAsync(groupsSchedule);
            groupsSchedule.ViewGroupsSchedule(tappedItem);
        }

        void Search_TextChanged(System.Object sender, EventArgs e)
        {

            SearchBar searchBar = (SearchBar)sender;
            listView.ItemsSource = Groups.Where(x => x.ToLower().Contains(searchBar.Text.ToLower()));
        }
    }
}
