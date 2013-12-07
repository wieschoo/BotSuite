// -----------------------------------------------------------------------
//  <copyright file="Browser.cs" company="HoovesWare">
//      Copyright (c) HoovesWare
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>framework for creating bots</purpose>
//  <homepage>http://botsuite.net/</homepage>
//  <license>http://botsuite.net/license/index/</license>
// -----------------------------------------------------------------------

namespace BotSuite
{
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Windows.Forms;
	using Microsoft.Win32;

	/// <summary>
	///     just decoration pattern for WebBrowser
	/// </summary>
	public class Browser
	{
		/// <summary>
		///     The time to wait.
		/// </summary>
		private const int WaitTime = 100000;

		/// <summary>
		///     intern instance of browser object
		/// </summary>
		private readonly WebBrowser instance;

		/// <summary>
		///     The full loaded.
		/// </summary>
		private bool fullLoaded;

		/// <summary>
		///     Initializes a new instance of the <see cref="Browser" /> class.
		///     initialise browser
		/// </summary>
		/// <param name="webBrowser">
		///     the webbrowser to use
		/// </param>
		/// <returns>
		/// </returns>
		public Browser(WebBrowser webBrowser)
		{
			this.instance = webBrowser;
			this.instance.DocumentCompleted += this.SetBrowserCompleted;
			this.fullLoaded = true;
			UseNewIE();
		}

		/// <summary>
		///     internal call; will be raise if site is loaded
		/// </summary>
		/// <param name="sender">
		///     the sender
		/// </param>
		/// <param name="e">
		///     the <see cref="WebBrowserDocumentCompletedEventArgs" />
		/// </param>
		protected void SetBrowserCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			this.fullLoaded = true;
		}

		/// <summary>
		///     tests if element exists
		/// </summary>
		/// <param name="id">
		///     the element id
		/// </param>
		/// <returns>
		///     if the elements exists
		/// </returns>
		public bool ElementExists(string id)
		{
			HtmlDocument htmlDocument = this.instance.Document;
			if (htmlDocument == null)
			{
				return false;
			}

			HtmlElement elementToGet = htmlDocument.GetElementById(id);
			return elementToGet != null;
		}

		/// <summary>
		///     causes a left click at an element
		/// </summary>
		/// <param name="id">
		///     id of element
		/// </param>
		public void ClickElementById(string id)
		{
			HtmlDocument htmlDocument = this.instance.Document;
			if (htmlDocument != null)
			{
				HtmlElement tmp = htmlDocument.GetElementById(id);
				if (tmp != null)
				{
					tmp.InvokeMember("click");
				}
			}
		}

		/// <summary>
		///     fill an inputfield
		/// </summary>
		/// <param name="id">
		///     id of input field
		/// </param>
		/// <param name="value">
		///     value to fill in
		/// </param>
		public void FillInputById(string id, string value)
		{
			HtmlDocument htmlDocument = this.instance.Document;
			if (htmlDocument != null)
			{
				HtmlElement elementById = htmlDocument.GetElementById(id);
				if (elementById != null)
				{
					elementById.SetAttribute("value", value);
				}
			}
		}

		/// <summary>
		///     navigate to an url and wait until it is completely loaded
		/// </summary>
		/// <param name="page">
		///     the page
		/// </param>
		public void NavigateTo(string page)
		{
			this.instance.Navigate(page);
			this.fullLoaded = false;
			this.WaitTillLoad();
		}

		/// <summary>
		///     get the inner Text of an element
		/// </summary>
		/// <param name="id">
		///     id of element
		/// </param>
		/// <returns>
		///     inner text
		/// </returns>
		public string GetInnerTextById(string id)
		{
			HtmlDocument htmlDocument = this.instance.Document;
			if (htmlDocument != null)
			{
				HtmlElement elementById = htmlDocument.GetElementById(id);
				if (elementById != null)
				{
					return elementById.InnerText;
				}
			}

			return null;
		}

		/// <summary>
		///     The wait till load.
		/// </summary>
		private void WaitTillLoad()
		{
			WebBrowserReadyState loadStatus;
			int counter = 0;
			while (true)
			{
				loadStatus = this.instance.ReadyState;
				Application.DoEvents();

				if ((counter > WaitTime) || (loadStatus == WebBrowserReadyState.Uninitialized)
					|| (loadStatus == WebBrowserReadyState.Loading) || (loadStatus == WebBrowserReadyState.Interactive))
				{
					break;
				}

				counter++;
			}

			counter = 0;
			while (true)
			{
				loadStatus = this.instance.ReadyState;
				Application.DoEvents();
				if ((loadStatus == WebBrowserReadyState.Complete) && this.fullLoaded)
				{
					break;
				}

				counter++;
			}
		}

		/// <summary>
		///     force the application to use IE8 or IE9
		/// </summary>
		private static void UseNewIE()
		{
			RegistryKey key =
				Registry.CurrentUser.OpenSubKey(
					"Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION",
					true)
				?? Registry.CurrentUser.CreateSubKey(
					"Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION");

			if (key != null)
			{
				key.SetValue(Process.GetCurrentProcess().MainModule.ModuleName, 9999, RegistryValueKind.DWord);
				key.Close();
			}
		}

		/// <summary>
		/// gets an <see cref="HtmlElement"/> by its ID
		/// </summary>
		/// <param name="id">the id of the element to get</param>
		/// <returns>an <see cref="HtmlElement"/></returns>
		public HtmlElement GetElementById(string id)
		{
			HtmlDocument htmlDocument = this.instance.Document;
			if (htmlDocument != null)
			{
				HtmlElement element = htmlDocument.GetElementById(id);
				return element;
			}

			return null;
		}

		/// <summary>
		/// Gets elements by css class name
		/// </summary>
		/// <param name="className">Name of the class.</param>
		/// <returns></returns>
		public List<HtmlElement> GetElementsByClassName(string className)
		{
			List<HtmlElement> htmlElements = new List<HtmlElement>();

			HtmlDocument htmlDocument = this.instance.Document;
			if (htmlDocument != null)
			{
				htmlElements.AddRange(htmlDocument.All.Cast<HtmlElement>().Where(htmlElement => htmlElement.GetAttribute("className") == className));
			}

			return htmlElements;
		}
	}
}