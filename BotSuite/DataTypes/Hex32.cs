// -----------------------------------------------------------------------
//  <copyright file="Hex32.cs" company="Binary Overdrive">
//      Copyright (c) Binary Overdrive.
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>Framework for creating automation applications.</purpose>
//  <homepage>https://bitbucket.org/KarillEndusa/botsuite.net</homepage>
//  <license>https://bitbucket.org/KarillEndusa/botsuite.net/wiki/license</license>
// -----------------------------------------------------------------------

namespace BotSuite.DataTypes
{
	using System;

	/// <summary>
	///     Wrapper for Int32 that represents a hexadecimal number
	/// </summary>
	public struct Hex32 : IEquatable<Hex32>, IComparable<Hex32>
	{
		/// <summary>
		///     The hash code var.
		/// </summary>
		private readonly int _hashCodeVar;

		/// <summary>
		///     Gets or sets the int value.
		/// </summary>
		/// <value>
		///     The int value.
		/// </value>
		public int IntValue;

		/// <summary>
		///     Initializes a new instance of the <see cref="Hex32" /> struct.
		/// </summary>
		/// <param name="i">
		///     The i.
		/// </param>
		public Hex32(int i)
		{
			this._hashCodeVar = i;
			this.IntValue = i;
		}

		/// <summary>
		///     Hex32s the specified i.
		/// </summary>
		/// <param name="i">The i.</param>
		/// <returns></returns>
		public static implicit operator Hex32(int i)
		{
			return new Hex32(i);
		}

		/// <summary>
		///     Int32s the specified h.
		/// </summary>
		/// <param name="h">The h.</param>
		/// <returns></returns>
		public static implicit operator int(Hex32 h)
		{
			return h.IntValue;
		}

		/// <summary>
		///     Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		///     A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return "0x" + this.IntValue.ToString("X");
		}

		/// <summary>
		///     Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
		/// </returns>
		public override int GetHashCode()
		{
			return this._hashCodeVar * 0x00010000 + this._hashCodeVar;
		}

		/// <summary>
		///     Determines whether the specified <see cref="System.Object" /> is equal to this instance.
		/// </summary>
		/// <param name="obj">
		///     The <see cref="System.Object" /> to compare with this instance.
		/// </param>
		/// <returns>
		///     <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
		{
			return this.GetType() == obj.GetType() && this.Equals((Hex32)obj);
		}

		/// <summary>
		///     Gibt an, ob das aktuelle Objekt einem anderen Objekt des gleichen Typs entspricht.
		/// </summary>
		/// <param name="other">
		///     Ein Objekt, das mit diesem Objekt verglichen werden soll.
		/// </param>
		/// <returns>
		///     true, wenn das aktuelle Objekt gleich dem <paramref name="other" />-Parameter ist, andernfalls false.
		/// </returns>
		public bool Equals(Hex32 other)
		{
			return this.GetType() == other.GetType() && this.IntValue.Equals(other.IntValue);
		}

		/// <summary>
		///     Vergleicht das aktuelle Objekt mit einem anderen Objekt desselben Typs.
		/// </summary>
		/// <param name="other">
		///     Ein Objekt, das mit diesem Objekt verglichen werden soll.
		/// </param>
		/// <returns>
		///     Eine 32-Bit-Ganzzahl mit Vorzeichen, die die relative Reihenfolge der verglichenen Objekte angibt. Der Rückgabewert
		///     hat folgende Bedeutung:
		///     Wert
		///     Bedeutung
		///     Kleiner als 0 (null)
		///     Dieses Objekt ist kleiner als der <paramref name="other" />-Parameter.
		///     0 (null)
		///     Dieses Objekt ist gleich <paramref name="other" />.
		///     Größer als 0 (null)
		///     Dieses Objekt ist größer als <paramref name="other" />.
		/// </returns>
		public int CompareTo(Hex32 other)
		{
			return this.IntValue.CompareTo(other.IntValue);
		}

		/// <summary>
		///     +s the specified h1.
		/// </summary>
		/// <param name="h1">The h1.</param>
		/// <param name="h2">The h2.</param>
		/// <returns>The result of h1 + h2.</returns>
		public static Hex32 operator +(Hex32 h1, Hex32 h2)
		{
			return new Hex32(h1.IntValue + h2.IntValue);
		}

		/// <summary>
		///     -s the specified h1.
		/// </summary>
		/// <param name="h1">The h1.</param>
		/// <param name="h2">The h2.</param>
		/// <returns>The result of h1 - h2.</returns>
		public static Hex32 operator -(Hex32 h1, Hex32 h2)
		{
			return new Hex32(h1.IntValue - h2.IntValue);
		}

		/// <summary>
		///     *s the specified h1.
		/// </summary>
		/// <param name="h1">The h1.</param>
		/// <param name="h2">The h2.</param>
		/// <returns>The result of h1 * h2.</returns>
		public static Hex32 operator *(Hex32 h1, Hex32 h2)
		{
			return new Hex32(h1.IntValue * h2.IntValue);
		}

		/// <summary>
		///     /s the specified h1.
		/// </summary>
		/// <param name="h1">The h1.</param>
		/// <param name="h2">The h2.</param>
		/// <returns>The result of h1 / h2.</returns>
		public static Hex32 operator /(Hex32 h1, Hex32 h2)
		{
			return new Hex32(h1.IntValue / h2.IntValue);
		}

		/// <summary>
		///     Checks if h1 is equal to h2
		/// </summary>
		/// <param name="h1">The h1.</param>
		/// <param name="h2">The h2.</param>
		/// <returns>True, if h1 and h2 are equal, else false.</returns>
		public static bool operator ==(Hex32 h1, Hex32 h2)
		{
			return h1.Equals(h2);
		}

		/// <summary>
		///     Checks if h1 is not equal to h2
		/// </summary>
		/// <param name="h1">The h1.</param>
		/// <param name="h2">The h2.</param>
		/// <returns>True, if h1 and h2 are not equal, else false.</returns>
		public static bool operator !=(Hex32 h1, Hex32 h2)
		{
			return !(h1 == h2);
		}

		/// <summary>
		///     Checks if h1 is greater then h2
		/// </summary>
		/// <param name="h1">The h1.</param>
		/// <param name="h2">The h2.</param>
		/// <returns>True, if h1 is bigger then h2, else false.</returns>
		public static bool operator >(Hex32 h1, Hex32 h2)
		{
			return h1.CompareTo(h2) > 0;
		}

		/// <summary>
		///     Checks if h1 is smaller then h2
		/// </summary>
		/// <param name="h1">The h1.</param>
		/// <param name="h2">The h2.</param>
		/// <returns>True, if h1 is smaller then h2, else false.</returns>
		public static bool operator <(Hex32 h1, Hex32 h2)
		{
			return h1.CompareTo(h2) < 0;
		}
	}
}
