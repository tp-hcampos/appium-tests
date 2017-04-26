using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Interfaces;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;

namespace appium_tests.PageObjects
{
    class StandardCalculatorScreen
    {
        // Element locators
        [FindsBy(How = How.Name, Using = "One")]
        private IWebElement oneBtn;

        [FindsBy(How = How.Name, Using = "Two")]
        private IWebElement twoBtn;

        [FindsBy(How = How.Name, Using = "Three")]
        private IWebElement threeBtn;

        [FindsBy(How = How.Name, Using = "Four")]
        private IWebElement fourBtn;

        [FindsBy(How = How.Name, Using = "Five")]
        private IWebElement fiveBtn;

        [FindsBy(How = How.Name, Using = "Six")]
        private IWebElement sixBtn;

        [FindsBy(How = How.Name, Using = "Seven")]
        private IWebElement sevenBtn;

        [FindsBy(How = How.Name, Using = "Eight")]
        private IWebElement eightBtn;

        [FindsBy(How = How.Name, Using = "Nine")]
        private IWebElement nineBtn;

        [FindsBy(How = How.Name, Using = "Zero")]
        private IWebElement zeroBtn;

        [FindsBy(How = How.Name, Using = "Plus")]
        private IWebElement plusBtn;

        [FindsBy(How = How.Name, Using = "Divide by")]
        private IWebElement divideBtn;

        [FindsBy(How = How.Name, Using = "Minus")]
        private IWebElement minusBtn;

        [FindsBy(How = How.Name, Using = "Multiply by")]
        private IWebElement multiplyBtn;

        [FindsBy(How = How.Name, Using = "Equals")]
        private IWebElement equalsBtn;

        [FindsBy(How = How.Name, Using = "Clear")]
        private IWebElement clearBtn;
    }
}
