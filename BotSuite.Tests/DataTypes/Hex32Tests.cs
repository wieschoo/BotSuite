// -----------------------------------------------------------------------
//  <copyright file="Hex32Tests.cs" company="Binary Overdrive">
//      Copyright (c) Binary Overdrive.
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>Framework for creating automation applications.</purpose>
//  <homepage>https://bitbucket.org/KarillEndusa/botsuite.net</homepage>
//  <license>https://bitbucket.org/KarillEndusa/botsuite.net/wiki/license</license>
// -----------------------------------------------------------------------



using BotSuite.DataTypes;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BotSuite.Tests.DataTypes
{
    /// <summary>
    /// The hex 32 tests.
    /// </summary>
    [TestClass]
    public class Hex32Tests
    {
        /// <summary>
        /// The create hex 32 with valid int value_ should give correct hex value.
        /// </summary>
        [TestMethod]
        public void CreateHex32WithValidIntValue_ShouldGiveCorrectHexValue()
        {
            Hex32 testHex5 = new Hex32(5);
            Hex32 testHex10 = new Hex32(10);
            Hex32 testHex11 = new Hex32(11);
            Hex32 testHex12 = new Hex32(12);
            Hex32 testHex13 = new Hex32(13);
            Hex32 testHex14 = new Hex32(14);
            Hex32 testHex15 = new Hex32(15);

            Assert.AreEqual("0x5", testHex5.ToString());
            Assert.AreEqual("0xA", testHex10.ToString());
            Assert.AreEqual("0xB", testHex11.ToString());
            Assert.AreEqual("0xC", testHex12.ToString());
            Assert.AreEqual("0xD", testHex13.ToString());
            Assert.AreEqual("0xE", testHex14.ToString());
            Assert.AreEqual("0xF", testHex15.ToString());
        }

        /// <summary>
        /// The convert hex 32 to int_ should give correct int value.
        /// </summary>
        [TestMethod]
        public void ConvertHex32ToInt_ShouldGiveCorrectIntValue()
        {
            const int IntValue = 32;

            Hex32 testHex = new Hex32(IntValue);

            Assert.AreEqual(IntValue, testHex.IntValue, "Should return correct integer value.");
        }

        /// <summary>
        /// The creating int with valid hex value_ should give correct integer.
        /// </summary>
        [TestMethod]
        public void CreatingIntWithValidHexValue_ShouldGiveCorrectInteger()
        {
            int intValueFromHex = (Hex32)20;

            Assert.AreEqual(20, intValueFromHex, "Expected equal values.");
        }

        /// <summary>
        /// The implicitly creating hex_ should return correct hex.
        /// </summary>
        [TestMethod]
        public void ImplicitlyCreatingHex_ShouldReturnCorrectHex()
        {
            Hex32 test = 5;

            Assert.AreEqual(5, test.IntValue, "Expected true.");
        }

        /// <summary>
        /// The equaling the same hex_ should return true.
        /// </summary>
        [TestMethod]
        public void EqualingTheSameHex_ShouldReturnTrue()
        {
            Hex32 hexValue = new Hex32(20);
            Hex32 hexValue2 = new Hex32(20);

            Assert.IsTrue(hexValue.Equals(hexValue2), "Expected true.");
        }

        /// <summary>
        /// The equaling to different object type_ should return false.
        /// </summary>
        [TestMethod]
        public void EqualingToDifferentObjectType_ShouldReturnFalse()
        {
            Hex32 hexValue = new Hex32(10);
            const string StringValue = "10";

            Assert.IsFalse(hexValue.Equals(StringValue), "Expected false.");
        }

        /// <summary>
        /// The equaling different hex_ should return false.
        /// </summary>
        [TestMethod]
        public void EqualingDifferentHex_ShouldReturnFalse()
        {
            Hex32 hexValue = new Hex32(10);
            Hex32 hexValue2 = new Hex32(20);

            Assert.IsFalse(hexValue.Equals(hexValue2), "Expected false.");
        }

        /// <summary>
        /// The compare hex to smaller_ should return more than zero.
        /// </summary>
        [TestMethod]
        public void CompareHexToSmaller_ShouldReturnMoreThanZero()
        {
            Hex32 hexValue = new Hex32(20);
            Hex32 hexValue2 = new Hex32(10);

            Assert.IsTrue(hexValue.CompareTo(hexValue2) > 0, "hexValue.CompareTo(hexValue2) > 0");
        }

        /// <summary>
        /// The compare hex to bigger_ should return less than zero.
        /// </summary>
        [TestMethod]
        public void CompareHexToBigger_ShouldReturnLessThanZero()
        {
            Hex32 hexValue = new Hex32(10);
            Hex32 hexValue2 = new Hex32(20);

            Assert.IsTrue(hexValue.CompareTo(hexValue2) < 0, "hexValue.CompareTo(hexValue2) < 0");
        }

        /// <summary>
        /// The compare hex to equal_ should return zero.
        /// </summary>
        [TestMethod]
        public void CompareHexToEqual_ShouldReturnZero()
        {
            Hex32 hexValue = new Hex32(10);
            Hex32 hexValue2 = new Hex32(10);

            Assert.IsTrue(hexValue.CompareTo(hexValue2) == 0, "hexValue.CompareTo(hexValue2) == 0");
        }

        /// <summary>
        /// The add two hex_ should return correct value.
        /// </summary>
        [TestMethod]
        public void AddTwoHex_ShouldReturnCorrectValue()
        {
            Hex32 hexValue = new Hex32(5);
            Hex32 hexValue2 = new Hex32(5);

            Assert.AreEqual(10, (hexValue + hexValue2).IntValue);
            Assert.AreEqual("0xA", (hexValue + hexValue2).ToString());
        }

        /// <summary>
        /// The subtract two hex_ should return correct value.
        /// </summary>
        [TestMethod]
        public void SubtractTwoHex_ShouldReturnCorrectValue()
        {
            Hex32 hexValue = new Hex32(10);
            Hex32 hexValue2 = new Hex32(5);

            Assert.AreEqual(5, (hexValue - hexValue2).IntValue);
            Assert.AreEqual("0x5", (hexValue - hexValue2).ToString());
        }

        /// <summary>
        /// The multiply two hex_ should return correct value.
        /// </summary>
        [TestMethod]
        public void MultiplyTwoHex_ShouldReturnCorrectValue()
        {
            Hex32 hexValue = new Hex32(10);
            Hex32 hexValue2 = new Hex32(5);

            Assert.AreEqual(50, (hexValue * hexValue2).IntValue, "Expected 50.");
        }

        /// <summary>
        /// The divide two hex_ should return correct value.
        /// </summary>
        [TestMethod]
        public void DivideTwoHex_ShouldReturnCorrectValue()
        {
            Hex32 hexValue = new Hex32(10);
            Hex32 hexValue2 = new Hex32(5);

            Assert.AreEqual(2, (hexValue / hexValue2).IntValue, "Expected 2.");
        }

        /// <summary>
        /// The greater than two hex_ should return correct value.
        /// </summary>
        [TestMethod]
        public void GreaterThanTwoHex_ShouldReturnCorrectValue()
        {
            Hex32 hexValue = new Hex32(10);
            Hex32 hexValue2 = new Hex32(5);

            Assert.IsTrue(hexValue > hexValue2, "Expected true."); 
        }

        /// <summary>
        /// The smaller than two hex_ should return correct value.
        /// </summary>
        [TestMethod]
        public void SmallerThanTwoHex_ShouldReturnCorrectValue()
        {
            Hex32 hexValue = new Hex32(10);
            Hex32 hexValue2 = new Hex32(5);

            Assert.IsTrue(hexValue2 < hexValue, "Expected true.");
        }

        /// <summary>
        /// The hex hash code_ should be equal to same hex number hash code.
        /// </summary>
        [TestMethod]
        public void HexHashCode_ShouldBeEqualToSameHexNumberHashCode()
        {
            Hex32 hexValue = new Hex32(10);
            Hex32 hexValue2 = new Hex32(10);

            Assert.AreEqual(hexValue.GetHashCode(), hexValue2.GetHashCode(), "Expected Equal hashcodes."); 
        }

        /// <summary>
        /// The equaling two same hex_ should return true.
        /// </summary>
        [TestMethod]
        public void EqualingTwoSameHex_ShouldReturnTrue()
        {
            Hex32 hexValue = new Hex32(10);
            Hex32 hexValue2 = new Hex32(10);

            Assert.IsTrue(hexValue == hexValue2, "hexValue == hexValue2");
        }

        /// <summary>
        /// The not equaling two same hex_ should return false.
        /// </summary>
        [TestMethod]
        public void NotEqualingTwoSameHex_ShouldReturnFalse()
        {
            Hex32 hexValue = new Hex32(10);
            Hex32 hexValue2 = new Hex32(10);

            Assert.IsFalse(hexValue != hexValue2, "hexValue != hexValue2");
        }
    }
}
