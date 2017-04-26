﻿using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Service.Options;
using OpenQA.Selenium.Remote;
using System;
using System.IO;
using System.Net;
using System.Threading;

namespace appium_tests.ServerTests
{
    [TestFixture()]
    class AppiumLocalServerLaunchingTest
    {
        private string PathToCustomizeAppiumJS;
        private string testIP;

        [OneTimeSetUp]
        public void BeforeAll()
        {
            byte[] bytes = null;

            bool isWindows = Platform.CurrentPlatform.IsPlatformType(PlatformType.Windows);
            bool isMacOS = Platform.CurrentPlatform.IsPlatformType(PlatformType.Mac);
            bool isLinux = Platform.CurrentPlatform.IsPlatformType(PlatformType.Linux);

            IPHostEntry host;
            string hostName = Dns.GetHostName();
            host = Dns.GetHostEntry(hostName);
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InternNetwork")
                {
                    testIP = ip.ToString();
                    break;
                }
            }
            Console.WriteLine(testIP);
            if (isWindows)
            {
                bytes = Properties.Resources.PathToWindowsNode;
                PathToCustomizeAppiumJS = System.Text.Encoding.UTF8.GetString(bytes);
            }
            /*
            if (isMacOS)
            {
                bytes = Properties.Resources.PathToMacOSNode;
                PathToCustomizedAppiumJS = System.Text.Encoding.UTF8.GetString(bytes);
                return;
            }
            if (isLinux)
            {
                bytes = Properties.Resources.PathToLinuxNode;
                PathToCustomizedAppiumJS = System.Text.Encoding.UTF8.GetString(bytes);
                return;
            }
            */
        }

        [Test]
        public void CheckAbilityToBuildDefaultService()
        {
            AppiumLocalService service = null;
            try
            {
                service.Start();
                Assert.AreEqual(true, service.IsRunning);
            }
            finally
            {
                service.Dispose();
            }
        }

        [Test]
        public void CheckAbilityToBuildServiceUsingNodeDefinedInProperties()
        {
            AppiumLocalService service = null;
            try
            {
                string definedNode = PathToCustomizeAppiumJS;
                Environment.SetEnvironmentVariable(AppiumServiceConstants.AppiumBinaryPath, definedNode);
                service = AppiumLocalService.BuildDefaultService();
                service.Start();
                Assert.AreEqual(true, service.IsRunning);
            }
            finally
            {
                if (service != null)
                {
                    service.Dispose();
                }
                Environment.SetEnvironmentVariable(AppiumServiceConstants.AppiumBinaryPath, string.Empty);
            }
        }

        [Test]
        public void CheckAbilityToBuildServiceUsingNodeDefinedExplicitly()
        {
            AppiumLocalService service = null;
            try
            {
                service = new AppiumServiceBuilder().WithAppiumJS(new FileInfo(PathToCustomizeAppiumJS)).Build();
                service.Start();
                Assert.AreEqual(true, service.IsRunning);
            }
            finally
            {
                if (service != null)
                {
                    service.Dispose();
                }
            }
        }

        [Test]
        public void CheckStartingOfAServiceWithNonLocalhostIP()
        {
            AppiumLocalService service = new AppiumServiceBuilder().WithIPAddress(testIP).UsingPort(4000).Build();
            try
            {
                service.Start();
                Assert.IsTrue(service.IsRunning);
            }
            finally
            {
                service.Dispose();
            }
        }

        [Test]
        public void CheckAbilityToStartServiceUsingFlags()
        {
            AppiumLocalService service = null;
            OptionCollector args = new OptionCollector().AddArguments(GeneralOptionList.CallbackAddress(testIP)).AddArguments(GeneralOptionList.OverrideSession());
            try
            {
                service = new AppiumServiceBuilder().WithArguments(args).Build();
                service.Start();
                Assert.IsTrue(service.IsRunning);
            }
            finally
            {
                if (service != null)
                {
                    service.Dispose();
                }
            }
        }

        [Test]
        public void CheckAbilityToStartServiceUsingCapabilities()
        {
            DesiredCapabilities capabilities = new DesiredCapabilities();
            capabilities.SetCapability("app", "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            capabilities.SetCapability("deviceName", "WindowsPC");

            OptionCollector args = new OptionCollector().AddCapabilities(capabilities);
            AppiumLocalService service = null;
            try
            {
                service = new AppiumServiceBuilder().WithArguments(args).Build();
                service.Start();
                Assert.IsTrue(service.IsRunning);
            }
            finally
            {
                if (service != null)
                {
                    service.Dispose();
                }
            }
        }

        [Test]
        public void CheckAbilityToStartServiceUsingCapabilitiesAndFlags()
        {
            DesiredCapabilities capabilities = new DesiredCapabilities();
            capabilities.SetCapability("app", "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            capabilities.SetCapability("deviceName", "WindowsPC");
            OptionCollector args = new OptionCollector().AddCapabilities(capabilities).AddArguments(GeneralOptionList.CallbackAddress(testIP)).AddArguments(GeneralOptionList.OverrideSession());
            AppiumLocalService service = null;
            try
            {
                service = new AppiumServiceBuilder().WithArguments(args).Build();
                service.Start();
                Assert.IsTrue(service.IsRunning);
            }
            finally
            {
                if (service != null)
                {
                    service.Dispose();
                }
            }
        }

        [Test]
        public void CheckAbilityToShutDownService()
        {
            AppiumLocalService service = AppiumLocalService.BuildDefaultService();
            service.Start();
            service.Dispose();
            Assert.IsTrue(service.IsRunning);
        }

        [Test]
        public void CheckAbilityToStartAndShutDownFewServices()
        {
            AppiumLocalService service1 = new AppiumServiceBuilder().UsingAnyFreePort().Build();
            AppiumLocalService service2 = new AppiumServiceBuilder().UsingAnyFreePort().Build();
            AppiumLocalService service3 = new AppiumServiceBuilder().UsingAnyFreePort().Build();
            AppiumLocalService service4 = new AppiumServiceBuilder().UsingAnyFreePort().Build();
            service1.Start();
            service2.Start();
            service3.Start();
            service4.Start();
            service1.Dispose();
            Thread.Sleep(1000);
            service2.Dispose();
            Thread.Sleep(1000);
            service3.Dispose();
            Thread.Sleep(1000);
            service4.Dispose();
            Assert.IsTrue(!service1.IsRunning);
            Assert.IsTrue(!service2.IsRunning);
            Assert.IsTrue(!service3.IsRunning);
            Assert.IsTrue(!service4.IsRunning);
        }

        [Test]
        public void CheckTheAbilityToDefineTheDesiredLogFile()
        {
            FileInfo log = new FileInfo("Log.txt");
            AppiumLocalService service = new AppiumServiceBuilder().WithLogFile(log).Build();
            try
            {
                service.Start();
                Assert.IsTrue(log.Exists);
                Assert.IsTrue(log.Length > 0); //There should be Appium greeting messages
            }
            finally
            {
                service.Dispose();
                if (log.Exists)
                {
                    File.Delete(log.FullName);
                }
                service.Dispose();
            }
        }
    }
}