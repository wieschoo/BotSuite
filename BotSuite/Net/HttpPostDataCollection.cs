//-----------------------------------------------------------------------
// <copyright file="HttpPostDataCollection.cs" company="Wieschoo &amp; enWare">
//     Copyright (c) Wieschoo &amp; enWare.
// </copyright>
// <project>BotSuite.Net</project>
// <purpose>framework for creating bots</purpose>
// <homepage>http://botsuite.net/</homepage>
// <license>http://botsuite.net/license/index/</license>
//-----------------------------------------------------------------------

namespace BotSuite.Net
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// collection-class for HttpPostData-instances
	/// </summary>
	public class HttpPostDataCollection : ICollection<HttpPostData>
	{
		/// <summary>
		/// a list of all postdata instances in this collection
		/// </summary>
		private List<HttpPostData> _PostData = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="HttpPostDataCollection"/> class
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
			{
				throw new ArgumentNullException("item");
			}

			if(!this.Contains(item))
			{
				this._PostData.Add(item);
			}
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
		/// <param name="item">an instance of the HttpPostData class</param>
		/// <returns>true, if this collection contains the given instance of the HttpPostData class, else false</returns>
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
		/// Gets the amount of items in the collection
		/// </summary>
		public Int32 Count
		{
			get
			{
				return this._PostData.Count;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the collection is read-only or not
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
			return this._PostData.Remove(item);
		}

		/// <summary>
		/// returns the IEnumerator&lt;HttpPostData&gt; of this collection
		/// </summary>
		/// <returns>an Enumerator of this collection</returns>
		public IEnumerator<HttpPostData> GetEnumerator()
		{
			return this._PostData.GetEnumerator();
		}

		/// <summary>
		/// returns the System.Collections.IEnumarator for this collection
		/// </summary>
		/// <returns>a non-generic IEnumerator of this collection</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (System.Collections.IEnumerator)this.GetEnumerator();
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
			foreach(HttpPostData data in this._PostData)
			{
				toString += data.ToString();
				if(data != this._PostData.Last())
				{
					toString += "&";
				}
			}

			return toString;
		}

		/// <summary>
		/// creates a HttpPostDataCollection instance from a POST data string
		/// </summary>
		/// <example>
		/// <code><![CDATA[
		/// var postData = "sender=wieschoo&message=hallo%20welt"
		/// var hpdc = HttpPostDataCollection.FromString(postData);
		/// ]]></code>
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
				{
					throw new ArgumentException("malformed postdata", "postdata");
				}

				col.Add(new HttpPostData(pdkvp[0], pdkvp[1]));
			}

			return col;
		}
	}
}
