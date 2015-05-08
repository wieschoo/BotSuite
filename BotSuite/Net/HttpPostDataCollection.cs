// -----------------------------------------------------------------------
//  <copyright file="HttpPostDataCollection.cs" company="Binary Overdrive">
//      Copyright (c) Binary Overdrive.
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>Framework for creating automation applications.</purpose>
//  <homepage>https://bitbucket.org/KarillEndusa/botsuite.net</homepage>
//  <license>https://bitbucket.org/KarillEndusa/botsuite.net/wiki/license</license>
// -----------------------------------------------------------------------

namespace BotSuite.Net
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	///     collection-class for HttpPostData-instances
	/// </summary>
	public class HttpPostDataCollection : ICollection<HttpPostData>
	{
		/// <summary>
		///     a list of all postdata instances in this collection
		/// </summary>
		private readonly List<HttpPostData> _postData;

		/// <summary>
		///     Initializes a new instance of the <see cref="HttpPostDataCollection" /> class
		/// </summary>
		public HttpPostDataCollection()
		{
			this._postData = new List<HttpPostData>();
		}

		/// <summary>
		///     adds a HttpPostData instance to the collection
		/// </summary>
		/// <param name="item">
		///     the item to add to the collection
		/// </param>
		public void Add(HttpPostData item)
		{
			if(item == null)
			{
				throw new ArgumentNullException("item");
			}

			if(!this.Contains(item))
			{
				this._postData.Add(item);
			}
		}

		/// <summary>
		///     deletes all items from the collection
		/// </summary>
		public void Clear()
		{
			this._postData.Clear();
		}

		/// <summary>
		///     checks if a given item is already part of the collection
		/// </summary>
		/// <param name="item">
		///     an instance of the HttpPostData class
		/// </param>
		/// <returns>
		///     true, if this collection contains the given instance of the HttpPostData class, else false
		/// </returns>
		public bool Contains(HttpPostData item)
		{
			return this._postData.Contains(item);
		}

		/// <summary>
		///     copies the whole collection into a compatible one-dimensional array beginning at the given index of the target
		///     array
		/// </summary>
		/// <param name="array">
		///     the target array
		/// </param>
		/// <param name="arrayIndex">
		///     the index where to insert the items in the target array
		/// </param>
		public void CopyTo(HttpPostData[] array, int arrayIndex)
		{
			this._postData.CopyTo(array, arrayIndex);
		}

		/// <summary>
		///     Gets the amount of items in the collection
		/// </summary>
		public int Count
		{
			get
			{
				return this._postData.Count;
			}
		}

		/// <summary>
		///     Gets a value indicating whether the collection is read-only or not
		/// </summary>
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		///     removes the given item from the collection
		/// </summary>
		/// <param name="item">
		///     the item to remove from the collection
		/// </param>
		/// <returns>
		///     true, if removal was succesfull, else false (like if the item wasn't part of the collection)
		/// </returns>
		public bool Remove(HttpPostData item)
		{
			return this._postData.Remove(item);
		}

		/// <summary>
		///     returns the IEnumerator&lt;HttpPostData&gt; of this collection
		/// </summary>
		/// <returns>an Enumerator of this collection</returns>
		public IEnumerator<HttpPostData> GetEnumerator()
		{
			return this._postData.GetEnumerator();
		}

		/// <summary>
		///     returns the System.Collections.IEnumarator for this collection
		/// </summary>
		/// <returns>a non-generic IEnumerator of this collection</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>
		///     creates a string representation of this collection
		/// </summary>
		/// <returns>a string representation of this collection</returns>
		public override string ToString()
		{
			string returnValue = string.Empty;
			foreach(HttpPostData data in this._postData)
			{
				returnValue += data.ToString();
				if(data != this._postData.Last())
				{
					returnValue += "&";
				}
			}

			return returnValue;
		}

		/// <summary>
		///     creates a HttpPostDataCollection instance from a POST data string
		/// </summary>
		/// <param name="postdata">
		///     the POST data string
		/// </param>
		/// <returns>
		///     a HttpPostDataCollection instance from the given POST data string
		/// </returns>
		public static HttpPostDataCollection FromString(string postdata)
		{
			HttpPostDataCollection col = new HttpPostDataCollection();
			string[] pds = postdata.Split('&');
			foreach(string[] pdkvp in pds.Select(pd => pd.Split('=')))
			{
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