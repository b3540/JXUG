using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace MStopWatch
{
    public class App : Application
    {
        public App()
        {
            // The root page of your application
            this.MainPage = new MyPage(); // ViewModel
            // this.MainPage = new MyPageV();  // View パターン
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
