/* **************************************************************
 * Name:      BotSuite.NET
 * Purpose:   Framework for creating bots
 * Homepage:  http://www.wieschoo.com
 * Copyright: (c) 2013 wieschoo & enWare
 * License:   http://www.wieschoo.com/botsuite/license/
 * *************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;
using System.Text.RegularExpressions;


namespace BotSuite
{
    /// <summary>
    /// just decoration pattern for WebBrowser
    /// </summary>
    public class Browser
    {
        /// <summary>
        /// intern instance of browser object
        /// </summary>
        private System.Windows.Forms.WebBrowser Instance;
        private bool FullLoaded;

        /// <summary>
        /// initialise browser
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Browser(System.Windows.Forms.WebBrowser i)
        {
            Instance = i;
            Instance.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(SetBrowserCompleted);
            FullLoaded = true;
            UseNewIE();
        }
        /// <summary>
        /// internal call; will be raise if site is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SetBrowserCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            FullLoaded = true;
        }
        /// <summary>
        /// tests if element exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ElementExists(string id)
        {
            HtmlElement ElementToGet = Instance.Document.GetElementById(id);
            return (ElementToGet!= null);
        }

        /// <summary>
        /// causes a left click at an element
        /// </summary>
        /// <param name="id">id of element</param>
        /// <returns></returns>
        public void ClickElementById(string id)
        {
            HtmlElement tmp = Instance.Document.GetElementById(id);
            tmp.InvokeMember("click");
        }

        /// <summary>
        /// fill an inputfield
        /// </summary>
        /// <param name="id">id of input field</param>
        /// <param name="value">value to fill in</param>
        /// <returns></returns>
        public void FillInputById(string id, string value)
        {
            Instance.Document.GetElementById(id).SetAttribute("value", value);
        }
//         public void SelectOption(string id)
//         {
//             //             Instance.Document.GetElementById(id).GetElementsByName("waluta1")[0].SetAttribute("selected", "true");
//             //             Instance.Document.GetElementById(id).Children.Ge
//         }


        /// <summary>
        /// navigate to an url and wait until it is completely loaded
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public void NavigateTo(string page)
        {
            Instance.Navigate(page);
            FullLoaded = false;
            WaitTillLoad();
        }

        /// <summary>
        /// get the inner Text (as an integer) of an element
        /// </summary>
        /// <param name="id">id of element</param>
        /// <returns>inner number</returns>
        public int GetInnerNumberById(string id)
        {
            string text = Instance.Document.GetElementById(id).InnerText;

            Regex r = new Regex("[0-9]");
            MatchCollection mc = r.Matches(text);
            string retVal = string.Empty;
            for (int i = 0; i < mc.Count; i++)
            { retVal += mc[i].Value; }
            return Convert.ToInt32(retVal);

        }

        /// <summary>
        /// get the inner Text of an element
        /// </summary>
        /// <param name="id">id of element</param>
        /// <returns>inner text</returns>
        public string GetInnerTextById(string id)
        {
            return Instance.Document.GetElementById(id).InnerText;
        }

        private void WaitTillLoad()
        {
            WebBrowserReadyState loadStatus;
            //wait till beginning of loading next page 
            int waittime = 100000;
            int counter = 0;
            while (true)
            {
                loadStatus = Instance.ReadyState;
                Application.DoEvents();

                if ((counter > waittime) || (loadStatus == WebBrowserReadyState.Uninitialized) || (loadStatus == WebBrowserReadyState.Loading) || (loadStatus == WebBrowserReadyState.Interactive))
                {
                    break;
                }
                counter++;
            }

            //wait till the page get loaded.
            counter = 0;
            while (true)
            {
                loadStatus = Instance.ReadyState;
                Application.DoEvents();
                if ((loadStatus == WebBrowserReadyState.Complete) && (FullLoaded == true))
                {
                    break;
                }
                counter++;
            }
        }


        /// <summary>
        /// force the application to use IE8 or IE9
        /// </summary>
        /// <returns></returns>
        protected void UseNewIE()
        {

            RegistryKey key = null;
            try
            {
                key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", true);
            }
            catch (Exception)
            {
                key = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION");
            }
            key.SetValue(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName, 9999, RegistryValueKind.DWord);
            key.Close();

        }
    }
}

// private const string RegistryPath = "SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION";

// private void InstallIgnoringCompatibilityView()
// {
// 	RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(RegistryPath, true);
// 	if (registryKey == null) registryKey = Registry.LocalMachine.CreateSubKey(RegistryPath);
// 
// 	registryKey.SetValue(Process.GetCurrentProcess().ProcessName + ".exe", 9000);
// }

// private void UninstallIgnoringCompatibilityView()
// {
// 	RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(RegistryPath, true);
// 	if (registryKey == null) registryKey = Registry.LocalMachine.CreateSubKey(RegistryPath);
// 
// 	registryKey.DeleteValue(Process.GetCurrentProcess().ProcessName + ".exe");
// }
