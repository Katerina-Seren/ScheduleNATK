using System;
using Xamarin.Forms;
namespace Schedule
{
    public class MainPageCS : TabbedPage
    {
        public MainPageCS()
        {
            var navigationPage = new NavigationPage(new SchedulePage());
            navigationPage.IconImageSource = "schedule.png";
            navigationPage.Title = "Расписание";

            Children.Add(new NotePage());
            Children.Add(navigationPage);
            Children.Add(new SettingsPage());
        }
    }
}
