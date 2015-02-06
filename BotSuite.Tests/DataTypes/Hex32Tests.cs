// -----------------------------------------------------------------------
//  <copyright file="Hex32Tests.cs" company="Binary Overdrive">
//      Copyright (c) Binary Overdrive.
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>Framework for creating automation applications.</purpose>
//  <homepage>https://bitbucket.org/KarillEndusa/botsuite.net</homepage>
//  <license>https://bitbucket.org/KarillEndusa/botsuite.net/wiki/license</license>
// -----------------------------------------------------------------------

namespace BotSuite.Tests.DataTypes
{
	using BotSuite.DataTypes;
	using NUnit.Framework;

	/// <summary>
	/// The hex 32 tests.
	/// </summary>
	[TestFixture]
	public class Hex32Tests
	{
		/// <summary>
		/// The create hex 32 with valid int value_ should give correct hex value.
		/// </summary>
		[Test]
		[TestCase(int.MinValue, ExpectedResult = "0x80000000")]
		[TestCase(-5, ExpectedResult = "0xFFFFFFFB")]
		[TestCase(-1, ExpectedResult = "0xFFFFFFFF")]
		[TestCase(0, ExpectedResult = "0x0")]
		[TestCase(1, ExpectedResult = "0x1")]
		[TestCase(5, ExpectedResult = "0x5")]
		[TestCase(10, ExpectedResult = "0xA")]
		[TestCase(11, ExpectedResult = "0xB")]
		[TestCase(12, ExpectedResult = "0xC")]
		[TestCase(13, ExpectedResult = "0xD")]
		[TestCase(14, ExpectedResult = "0xE")]
		[TestCase(15, ExpectedResult = "0xF")]
		[TestCase(100, ExpectedResult = "0x64")]
		[TestCase(1000, ExpectedResult = "0x3E8")]
		[TestCase(int.MaxValue, ExpectedResult = "0x7FFFFFFF")]
		public string CreateHex32WithValidIntValue_ShouldGiveCorrectHexValue(int value)
		{
			Hex32 hex = new Hex32(value);
			return hex.ToString();
		}

		/// <summary>
		/// The convert hex 32 to int_ should give correct int value.
		/// </summary>
		[Test]
		public void ConvertHex32ToInt_ShouldGiveCorrectIntValue()
		{
			const int value = 32;
			Hex32 testHex = new Hex32(value);

			Assert.AreEqual(value, testHex.IntValue, "Should return correct integer value.");
		}

		/// <summary>
		/// The creating int with valid hex value_ should give correct integer.
		/// </summary>
		[Test]
		public void CreatingIntWithValidHexValue_ShouldGiveCorrectInteger()
		{
			int intValueFromHex = (Hex32)20;

			Assert.AreEqual(20, intValueFromHex, "Expected equal values.");
		}

		/// <summary>
		/// The implicitly creating hex_ should return correct hex.
		/// </summary>
		[Test]
		public void ImplicitlyCreatingHex_ShouldReturnCorrectHex()
		{
			Hex32 test = 5;

			Assert.AreEqual(5, test.IntValue, "Expected true.");
		}

		/// <summary>
		/// The equaling the same hex_ should return true.
		/// </summary>
		[Test]
		public void EqualingTheSameHex_ShouldReturnTrue()
		{
			Hex32 hexValue = new Hex32(20);
			Hex32 hexValue2 = new Hex32(20);

			Assert.IsTrue(hexValue.Equals(hexValue2), "Expected true.");
		}

		/// <summary>
		/// The equaling to different object type_ should return false.
		/// </summary>
		[Test]
		public void EqualingToDifferentObjectType_ShouldReturnFalse()
		{
			Hex32 hexValue = new Hex32(10);
			const string StringValue = "10";

			Assert.IsFalse(hexValue.Equals(StringValue), "Expected false.");
		}

		/// <summary>
		/// The equaling different hex_ should return false.
		/// </summary>
		[Test]
		public void EqualingDifferentHex_ShouldReturnFalse()
		{
			Hex32 hexValue = new Hex32(10);
			Hex32 hexValue2 = new Hex32(20);

			Assert.IsFalse(hexValue.Equals(hexValue2), "Expected false.");
		}

		/// <summary>
		/// The compare hex to smaller_ should return more than zero.
		/// </summary>
		[Test]
		public void CompareHexToSmaller_ShouldReturnMoreThanZero()
		{
			Hex32 hexValue = new Hex32(20);
			Hex32 hexValue2 = new Hex32(10);

			Assert.IsTrue(hexValue.CompareTo(hexValue2) > 0, "hexValue.CompareTo(hexValue2) > 0");
		}

		/// <summary>
		/// The compare hex to bigger_ should return less than zero.
		/// </summary>
		[Test]
		public void CompareHexToBigger_ShouldReturnLessThanZero()
		{
			Hex32 hexValue = new Hex32(10);
			Hex32 hexValue2 = new Hex32(20);

			Assert.IsTrue(hexValue.CompareTo(hexValue2) < 0, "hexValue.CompareTo(hexValue2) < 0");
		}

		/// <summary>
		/// The compare hex to equal_ should return zero.
		/// </summary>
		[Test]
		public void CompareHexToEqual_ShouldReturnZero()
		{
			Hex32 hexValue = new Hex32(10);
			Hex32 hexValue2 = new Hex32(10);

			Assert.IsTrue(hexValue.CompareTo(hexValue2) == 0, "hexValue.CompareTo(hexValue2) == 0");
		}

		/// <summary>
		/// The add two hex_ should return correct value.
		/// </summary>
		[Test]
		public void AddTwoHex_ShouldReturnCorrectValue()
		{
			Hex32 hexValue = new Hex32(5);
			Hex32 hexValue2 = new Hex32(5);

			Assert.AreEqual(10, (hexValue + hexValue2).IntValue);
		}

		/// <summary>
		/// The subtract two hex_ should return correct value.
		/// </summary>
		[Test]
		public void SubtractTwoHex_ShouldReturnCorrectValue()
		{
			Hex32 hexValue = new Hex32(10);
			Hex32 hexValue2 = new Hex32(5);

			Assert.AreEqual(5, (hexValue - hexValue2).IntValue);
		}

		/// <summary>
		/// The multiply two hex_ should return correct value.
		/// </summary>
		[Test]
		public void MultiplyTwoHex_ShouldReturnCorrectValue()
		{
			Hex32 hexValue = new Hex32(10);
			Hex32 hexValue2 = new Hex32(5);

			Assert.AreEqual(50, (hexValue * hexValue2).IntValue, "Expected 50.");
		}

		/// <summary>
		/// The divide two hex_ should return correct value.
		/// </summary>
		[Test]
		public void DivideTwoHex_ShouldReturnCorrectValue()
		{
			Hex32 hexValue = new Hex32(10);
			Hex32 hexValue2 = new Hex32(5);

			Assert.AreEqual(2, (hexValue / hexValue2).IntValue, "Expected 2.");
		}

		/// <summary>
		/// The greater than two hex_ should return correct value.
		/// </summary>
		[Test]
		public void GreaterThanTwoHex_ShouldReturnCorrectValue()
		{
			Hex32 hexValue = new Hex32(10);
			Hex32 hexValue2 = new Hex32(5);

			Assert.IsTrue(hexValue > hexValue2, "Expected true.");
		}

		/// <summary>
		/// The smaller than two hex_ should return correct value.
		/// </summary>
		[Test]
		public void SmallerThanTwoHex_ShouldReturnCorrectValue()
		{
			Hex32 hexValue = new Hex32(10);
			Hex32 hexValue2 = new Hex32(5);

			Assert.IsTrue(hexValue2 < hexValue, "Expected true.");
		}

		/// <summary>
		/// The hex hash code_ should be equal to same hex number hash code.
		/// </summary>
		[Test]
		public void HexHashCode_ShouldBeEqualToSameHexNumberHashCode()
		{
			Hex32 hexValue = new Hex32(10);
			Hex32 hexValue2 = new Hex32(10);

			Assert.AreEqual(hexValue.GetHashCode(), hexValue2.GetHashCode(), "Expected Equal hashcodes.");
		}

		/// <summary>
		/// The equaling two same hex_ should return true.
		/// </summary>
		[Test]
		public void EqualingTwoSameHex_ShouldReturnTrue()
		{
			Hex32 hexValue = new Hex32(10);
			Hex32 hexValue2 = new Hex32(10);

			Assert.IsTrue(hexValue == hexValue2, "hexValue == hexValue2");
		}

		/// <summary>
		/// The not equaling two same hex_ should return false.
		/// </summary>
		[Test]
		public void NotEqualingTwoSameHex_ShouldReturnFalse()
		{
			Hex32 hexValue = new Hex32(10);
			Hex32 hexValue2 = new Hex32(10);

			Assert.IsFalse(hexValue != hexValue2, "hexValue != hexValue2");
		}
	}
}