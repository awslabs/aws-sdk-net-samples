using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace ContactManager
{
    public class App : Application
    {
        public NavigationPage NavPage { get; set; }

        public App()
        {
            NavPage = new NavigationPage(new LoginPage());
            MainPage = NavPage;
        }

        public static Action<string> OnFacebookAuthSuccess;
        public static Action OnFacebookAuthFailed;

        public void UpdateMainPage(ContentPage page)
        {
            NavPage = new NavigationPage(page);
            MainPage = NavPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }


    }
}
