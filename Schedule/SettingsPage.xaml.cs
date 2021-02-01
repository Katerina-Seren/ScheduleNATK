using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Schedule
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
		}

        async void SiteLabelClicked(object sender, EventArgs args)
        {
            await Browser.OpenAsync("https://natk.ru/", BrowserLaunchMode.External);
        }

        async void EmailLabelClicked(object sender, EventArgs args)
        {
            var message = new EmailMessage
            {
                To = new List<string> { "serenkoei@icloud.com" }
            };
            await Email.ComposeAsync(message);
        }       

    }
}
