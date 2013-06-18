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

namespace BotSuite.Net
{
	/// <summary>
	/// collection-class for HttpPostData-instances
	/// </summary>
	public class HttpPostDataCollection : ICollection<HttpPostData>
	{
		private List<HttpPostData> _PostData = null;

		/// <summary>
		/// constructor for the HttpPostDataCollection
		/// </summary>
		public HttpPostDataCollection()
		{
			this._PostData = new List<HttpPostData>();
		}

		/// <summary>
		/// adds a HttpPostData instance to the collection
		/// </summary>
		/// <param name="item">the item to add to the collection</param>
		public void Add(HttpPostData item)
		{
			if(item == null)
				throw new ArgumentNullException("item");
			if(!this.Contains(item))
				this._PostData.Add(item);
		}

		/// <summary>
		/// deletes all items from the collection
		/// </summary>
		public void Clear()
		{
			this._PostData.Clear();
		}

		/// <summary>
		/// checks if a given item is already part of the collection
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public Boolean Contains(HttpPostData item)
		{
			return this._PostData.Contains(item);
		}

		/// <summary>
		/// copies the whole collection into a compatible one-dimensional array beginning at the given index of the target array
		/// </summary>
		/// <param name="array">the target array</param>
		/// <param name="arrayIndex">the index where to insert the items in the target array</param>
		public void CopyTo(HttpPostData[] array, Int32 arrayIndex)
		{
			this._PostData.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// the amount of items in the collection
		/// </summary>
		public Int32 Count
		{
			get
			{
				return this._PostData.Count;
			}
		}

		/// <summary>
		/// if the collection is read-only or not
		/// </summary>
		public Boolean IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// removes the given item from the collection
		/// </summary>
		/// <param name="item">the item to remove from the collection</param>
		/// <returns>true, if removal was succesfull, else false (like if the item wasn't part of the collection)</returns>
		public Boolean Remove(HttpPostData item)
		{
			return _PostData.Remove(item);
		}

		/// <summary>
		/// returns the IEnumerator&lt;HttpPostData&gt; of this collection
		/// </summary>
		/// <returns></returns>
		public IEnumerator<HttpPostData> GetEnumerator()
		{
			return _PostData.GetEnumerator();
		}

		/// <summary>
		/// returns the System.Collections.IEnumarator for this collection
		/// </summary>
		/// <returns></returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (System.Collections.IEnumerator)GetEnumerator();
		}

		/// <summary>
		/// creates a string representation of this collection
		/// </summary>
		/// <example>
		/// <code>
		/// var hpdc = new HttpPostDataCollection();
		/// hpdc.Add(new HttpPostData("sender", "wieschoo");
		/// hpdc.Add(new HttpPostData("message", "hallo%20welt");
		/// var postData = hpdc.ToString();
		/// </code>
		/// </example>
		/// <returns>a string representation of this collection</returns>
		public override String ToString()
		{
			String toString = String.Empty;
			foreach(HttpPostData data in _PostData)
			{
				toString += data.ToString();
				if(data != _PostData.Last())
					toString += "&";
			}
			return toString;
		}

		/// <summary>
		/// creates a HttpPostDataCollection instance from a POST data string
		/// </summary>
		/// <example>
		/// <code>
		/// var postData = "sender=wieschoo&message=hallo%20welt"
		/// var hpdc = HttpPostDataCollection.FromString(postData);
		/// </code>
		/// </example>
		/// <param name="postdata">the POST data string</param>
		/// <returns>a HttpPostDataCollection instance from the given POST data string</returns>
		public static HttpPostDataCollection FromString(String postdata)
		{
			HttpPostDataCollection col = new HttpPostDataCollection();
			string[] pds = postdata.Split('&');
			string[] pdkvp = null;
			foreach(string pd in pds)
			{
				pdkvp = pd.Split('=');
				if(pdkvp.Length != 2)
					throw new ArgumentException("malformed postdata", "postdata");
				col.Add(new HttpPostData(pdkvp[0], pdkvp[1]));
			}
			return col;
		}
	}
}
