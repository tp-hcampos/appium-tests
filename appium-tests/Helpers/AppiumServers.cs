using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Service.Options;
using OpenQA.Selenium.Remote;
using System;

namespace appium_tests.Helpers
{
    class AppiumServers
    {
        private static AppiumLocalService LocalService;

        public static Uri DriverURI = new Uri("http://127.0.0.1:4723/wd/hub");

        public static Uri LocalServiceURIWindows
        {
            get
            {
                if (LocalService == null)
                {
                    AppiumServiceBuilder builder = new AppiumServiceBuilder().WithLogFile(new System.IO.FileInfo("Log"));
                    LocalService = builder.Build();
                }
                if (LocalService.IsRunning)
                {
                    LocalService.Start();
                }
                return LocalService.ServiceUrl;
            }
        }

        public static void StopLocalService()
        {
            if (LocalService != null && LocalService.IsRunning)
            {
                LocalService.Dispose();
                LocalService = null;
            }
        }
    }
}