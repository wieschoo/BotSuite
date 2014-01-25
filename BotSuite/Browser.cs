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
	///     Wrapper for <see cref="WebBrowser"/>
	/// </summary>
	public class Browser
	{
		private const string HtmlElementMemberClick = "click";

		/// <summary>
		///     intern instance of browser object
		/// </summary>
		private readonly WebBrowser browser;

		/// <summary>
		///     The full loaded.
		/// </summary>
		private bool loaded;

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
			this.browser = webBrowser;
			this.browser.DocumentCompleted += this.BrowserDocumentCompleted;
			this.loaded = true;
			UseNewIE();
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

			if(key != null)
			{
				key.SetValue(Process.GetCurrentProcess().MainModule.ModuleName, 9999, RegistryValueKind.DWord);
				key.Close();
			}
		}

		/// <summary>
		///     internal call, will be raised when site is loaded
		/// </summary>
		/// <param name="sender">
		///     the sender
		/// </param>
		/// <param name="e">
		///     the <see cref="WebBrowserDocumentCompletedEventArgs" />
		/// </param>
		protected void BrowserDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			this.loaded = true;
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
			HtmlDocument htmlDocument = this.browser.Document;
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
			HtmlDocument htmlDocument = this.browser.Document;
			if (htmlDocument != null)
			{
				HtmlElement tmp = htmlDocument.GetElementById(id);
				if (tmp != null)
				{
					tmp.InvokeMember(HtmlElementMemberClick);
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
			HtmlDocument htmlDocument = this.browser.Document;
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
			HtmlDocument htmlDocument = this.browser.Document;
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
		///     navigate to an url and wait until it is completely loaded
		/// </summary>
		/// <param name="page">
		///     the page
		/// </param>
		public void NavigateTo(string page)
		{
			this.loaded = false;
			this.browser.Navigate(page);

			while(!this.loaded)
			{
				Application.DoEvents();
				System.Threading.Thread.Sleep(50);
			}
		}

		/// <summary>
		///		gets an <see cref="HtmlElement"/> by its ID
		/// </summary>
		/// <param name="id">the id of the element to get</param>
		/// <returns>an <see cref="HtmlElement"/></returns>
		public HtmlElement GetElementById(string id)
		{
			HtmlDocument htmlDocument = this.browser.Document;
			if (htmlDocument != null)
			{
				HtmlElement element = htmlDocument.GetElementById(id);
				return element;
			}

			return null;
		}

		/// <summary>
		///		Gets elements by css class name
		/// </summary>
		/// <param name="className">Name of the class.</param>
		/// <returns>a list of <see cref="HtmlElement"/> objects with the given class</returns>
		public List<HtmlElement> GetElementsByClassName(string className)
		{
			List<HtmlElement> htmlElements = new List<HtmlElement>();

			HtmlDocument htmlDocument = this.browser.Document;
			if (htmlDocument != null)
			{
				htmlElements.AddRange(htmlDocument.All.Cast<HtmlElement>().Where(htmlElement => htmlElement.GetAttribute("className") == className));
			}

			return htmlElements;
		}

		/// <summary>
		///		Get elements by tag name
		/// </summary>
		/// <param name="tagName">name of the tag</param>
		/// <returns>a list of <see cref="HtmlElement"/> objects with the given tag name</returns>
		public List<HtmlElement> GetElementsByTagName(string tagName)
		{
			List<HtmlElement> htmlElements = new List<HtmlElement>();

			HtmlDocument htmlDocument = this.browser.Document;
			if (htmlDocument != null)
			{
				htmlElements.AddRange(htmlDocument.GetElementsByTagName(tagName).Cast<HtmlElement>());
			}

			return htmlElements;
		}
	}
}